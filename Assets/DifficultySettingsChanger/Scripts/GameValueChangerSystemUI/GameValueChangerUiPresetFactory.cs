using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DifficultySettingsChanger.GameValueChangerSystem;
using DifficultySettingsChanger.GameValueChangerSystemUI.Components;
using TimberApi.UiBuilderSystem.PresetSystem;
using Timberborn.CoreUI;
using UnityEngine;
using UnityEngine.UIElements;

namespace DifficultySettingsChanger.GameValueChangerSystemUI
{
    public class GameValueChangerUiPresetFactory
    {
        private readonly VisualElementInitializer _visualElementInitializer;
        private readonly VisualElementLoader _visualElementLoader;
        private readonly UiPresetFactory _uiPresetFactory;

        private GameValueChangerUiPresetFactory(
            VisualElementInitializer visualElementInitializer, 
            VisualElementLoader visualElementLoader, 
            UiPresetFactory uiPresetFactory)
        {
            _visualElementInitializer = visualElementInitializer;
            _visualElementLoader = visualElementLoader;
            _uiPresetFactory = uiPresetFactory;
        }

        public VisualElement GetObjectsContainer(string headerLocKey)
        {
            var root = new VisualElement();
            root.AddToClassList("mods-box__objects");
            root.style.width = new Length(Screen.width / 4f, LengthUnit.Pixel);
            var header = _uiPresetFactory.Labels().GameTextHeading(headerLocKey);
            root.Add(header);
            root.Add(CreateScrollView());
            return root;
        }
        
        public VisualElement GetGameValueChangerContainer(string headerLocKey)
        {
            var root = new VisualElement();
            root.AddToClassList("mods-box__properties");
            root.style.width = new Length(Screen.width / 3f, LengthUnit.Pixel);
            var header = _uiPresetFactory.Labels().GameTextHeading(headerLocKey);
            root.Add(header);
            root.Add(CreateScrollView());
            return root;
        }

        public VisualElement GetGroupHeader(string header)
        {
            return _uiPresetFactory.Labels().GameTextHeading(text: header);
        }

        public ObjectWrapper GetObjectWrapper(string header, IGrouping<string, GameValueChanger> groupedValueChangers, ScrollView propertiesRoot, ScrollView gameValueChangerContainer)
        {
            var container = CreateContainer();
            container.Add(CreateLabel(header));
            var button = CreateButton("Open object");
            container.Add(button);
            // var propertiesSubContainer = new VisualElement();
            // propertiesRoot.Add(propertiesSubContainer);
            var objectWrapper = ObjectWrapper.Create(this, container, button, groupedValueChangers, propertiesRoot, gameValueChangerContainer);
            return objectWrapper;
        }
        
        public VisualElement GetPropertyWrapper(GameValueChanger header, ScrollView gameValueChangerContainer)
        {
            var container = CreateContainer();
            container.Add(CreateLabel(header.LabelText));
            var button = CreateButton("Open property");
            button.clicked += () =>
            {
                gameValueChangerContainer.Clear();
                gameValueChangerContainer.Add(GetUiPreset(header));
            };
            container.Add(button);
            return container;

        }
        
        public VisualElement GetUiPreset(GameValueChanger gameValueChanger)
        {
            switch (gameValueChanger.FieldRef.Value)
            {
                case string:
                    return LabelAndTextField(gameValueChanger);
                case int:
                    return LabelAndIntField(gameValueChanger);
                case float:
                    return LabelAndFloatField(gameValueChanger);
                case bool:
                    return LabelCheckbox(gameValueChanger);
                case IEnumerable:
                    return LabelCollectionBoxButton(gameValueChanger);
                case ValueType:
                    return LabelValueTypeBoxButton(gameValueChanger);
                default:
                    Plugin.Log.LogError($"UiTemplate for type '{gameValueChanger.FieldRef.Value.GetType()}' is not supported. Check to make sure you are using the correct preset.");
                    return new VisualElement();
            }
        }
        
        private VisualElement LabelAndTextField(GameValueChanger gameValueChanger)
        {
            var container = CreateContainer();

            container.Add(CreateLabel(gameValueChanger.LabelText));

            var textField = CreateTextField();
            textField.style.minWidth = new Length(60, LengthUnit.Pixel);
            
            textField.SetValueWithoutNotify((string)gameValueChanger.FieldRef.Value);
            textField.RegisterCallback<FocusOutEvent>(_ => gameValueChanger.FieldRef.Value = textField.value);
            
            container.Add(textField);
            return container;
        }

        private VisualElement LabelAndIntField(GameValueChanger gameValueChanger)
        {
            var container = CreateContainer();

            container.Add(CreateLabel(gameValueChanger.LabelText));

            var integerField = CreateIntegerField();
            integerField.style.minWidth = new Length(60, LengthUnit.Pixel);

            Timberborn.CoreUI.TextFields.InitializeIntegerField(integerField, (int)gameValueChanger.FieldRef.Value, afterEditingCallback: value => gameValueChanger.FieldRef.Value = value);
            
            container.Add(integerField);
            return container;
        }
        
        private VisualElement LabelAndFloatField(GameValueChanger gameValueChanger)
        {
            var container = CreateContainer();

            container.Add(CreateLabel(gameValueChanger.LabelText));

            var textField = CreateTextField();
            textField.style.minWidth = new Length(60, LengthUnit.Pixel);
            
            TextFields.InitializeFloatTextField(textField, (float)gameValueChanger.FieldRef.Value, afterEditingCallback: value => gameValueChanger.FieldRef.Value = (float)value);
            
            container.Add(textField);
            return container;
        }

        private VisualElement LabelCheckbox(GameValueChanger gameValueChanger)
        {
            var container = CreateContainer();
            
            container.Add(CreateLabel(gameValueChanger.LabelText));

            var checkbox = _uiPresetFactory.Toggles().Checkbox();
            
            checkbox.SetValueWithoutNotify((bool)gameValueChanger.FieldRef.Value);
            checkbox.RegisterValueChangedCallback(newValue =>
            {
                gameValueChanger.FieldRef.Value = newValue.newValue;
            });

            container.Add(checkbox);
            return container;
        }
        
        private VisualElement LabelCollectionBoxButton(GameValueChanger gameValueChanger)
        {
            if (gameValueChanger is not CollectionSaveableGameValueChanger collectionSaveableGameValueChanger)
                throw new InvalidOperationException($"GameValueChanger with {nameof(LabelCollectionBoxButton)} template was not a CollectionSavableGameValueChanger.");

            var container = CreateContainer();
            
            container.Add(CreateLabel(collectionSaveableGameValueChanger.LabelText));

            CollapseButton.Create(
                this,
                CreateButton("Open Collection"), 
                () => collectionSaveableGameValueChanger.GameValueChangers,
                container);
            
            return container;
        }
        
        private VisualElement LabelValueTypeBoxButton(GameValueChanger gameValueChanger)
        {
            if (gameValueChanger is not ValueTypeSaveableGameValueChanger valueTypeSaveableGameValueChanger)
                throw new InvalidOperationException($"GameValueChanger with {nameof(LabelValueTypeBoxButton)} template was not a ValueTypeSaveableGameValueChanger.");

            var container = CreateContainer();
            
            container.Add(CreateLabel(valueTypeSaveableGameValueChanger.LabelText));
            
            CollapseButton.Create(
                this,
                CreateButton("Open Value Type"), 
                () => (List<GameValueChanger>)valueTypeSaveableGameValueChanger.Fields,
                container);
            
            return container;
        }

        private NineSliceVisualElement CreateContainer()
        {
            var container = new NineSliceVisualElement();
            container.AddToClassList("mods-box__game-value-changer-line");
            return container;
        }

        private ScrollView CreateScrollView()
        {
            var gameValueChangerContainer = new ScrollView
            {
                style =
                {
                    width = new Length(100, LengthUnit.Percent),
                    height = new Length(100, LengthUnit.Percent),

                    justifyContent = new StyleEnum<Justify>(Justify.Center),
                    alignItems = new StyleEnum<Align>(Align.Center),
                    
                    marginTop = new Length(20, LengthUnit.Pixel)
                }
            };
            // gameValueChangerContainer.AddToClassList("mods-box__game-value-changer");
            gameValueChangerContainer.AddToClassList("scroll--green-decorated");
            return gameValueChangerContainer;
        }

        private Button CreateButton(string text)
        {
            var button = _uiPresetFactory.Buttons().ButtonGame(text: text);
            button.style.maxHeight = new Length(20, LengthUnit.Pixel);
            button.style.width = new Length(100, LengthUnit.Pixel);
            return button;
        }
        
        private VisualElement CreateLabel(string text)
        {
            return _uiPresetFactory.Labels().GameText(text: text);
        }

        private TextField CreateTextField()
        {
            // var input = _uiPresetFactory.TextFields().InGameTextField(new Length(50, LengthUnit.Percent), new Length(30, LengthUnit.Pixel));
            var visualElement = _visualElementLoader.LoadVisualElement("Core/InputBox");
            var input = visualElement.Q<TextField>("Input");
            input.style.height = new Length(30, LengthUnit.Pixel);
            input.style.width = new Length(50, LengthUnit.Percent);
            return input;
        }
        
        private IntegerField CreateIntegerField()
        {
            return _visualElementLoader.LoadVisualElement("Game/BatchControl/PopulationDistributorBatchControlRowItem").Q<IntegerField>("MinimumValue");
        }
    }
}