using System;
using System.Collections;
using TimberApi.DependencyContainerSystem;
using TimberApi.UiBuilderSystem.PresetSystem;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace DifficultySettingsChanger
{
    public class GameValueChangerUiPresetFactory
    {
        private readonly VisualElementInitializer _visualElementInitializer;
        // private readonly HierarchicalManager _hierarchicalManager;
        private readonly VisualElementLoader _visualElementLoader;
        private readonly UiPresetFactory _uiPresetFactory;
        private readonly PanelStack _panelStack;
        
        
        GameValueChangerUiPresetFactory(
            VisualElementInitializer visualElementInitializer, 
            // HierarchicalManager hierarchicalManager, 
            VisualElementLoader visualElementLoader, 
            UiPresetFactory uiPresetFactory, 
            PanelStack panelStack)
        {
            _visualElementInitializer = visualElementInitializer;
            // _hierarchicalManager = hierarchicalManager;
            _visualElementLoader = visualElementLoader;
            _uiPresetFactory = uiPresetFactory;
            _panelStack = panelStack;
        }

        public VisualElement GetGameValueChangerContainer(string headerLocKey)
        {
            var root = new VisualElement();
            root.AddToClassList("mods-box__game-value-changer");
            
            var header = _uiPresetFactory.Labels().GameTextHeading(headerLocKey);
            // _visualElementInitializer.InitializeVisualElement(header);
            root.Add(header);
            
            var gameValueChangerContainer = new ScrollView()
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
            
            root.Add(gameValueChangerContainer);
            
            return root;
        }

        public VisualElement GetGroupHeader(string header)
        {
            return _uiPresetFactory.Labels().GameTextHeading(text: header);
        }
        
        public VisualElement GetUiPreset(GameValueChanger gameValueChanger)
        {
            try
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
                        throw new ArgumentOutOfRangeException($"UiTemplate for type '{gameValueChanger.FieldRef.Value.GetType()}' is not supported.");
                }
            }
            catch (Exception e)
            {
                Plugin.Log.LogError("Something went wrong during UI generation. Check to make sure you are using the correct preset.");
                throw e;
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

            var button = CreateButton("Open Collection");

            var hierarchicalManager = DependencyContainer.GetInstance<HierarchicalManager>();
            var newIndex = hierarchicalManager.CurrentIndex;
            Plugin.Log.LogInfo("newIndex: " + newIndex);
            
            button.clicked += () =>
            {
                var settingChangerContainer = GetGameValueChangerContainer(collectionSaveableGameValueChanger.FieldName);
                var settingChangerContainerScrollView = settingChangerContainer.Q<ScrollView>();
                foreach (var fieldValueChanger in collectionSaveableGameValueChanger.GameValueChangers)
                    settingChangerContainerScrollView.Add(GetUiPreset(fieldValueChanger));

                var hierarchicalLayer = new HierarchicalLayer(newIndex, settingChangerContainer);
                hierarchicalManager.OpenNewLayer(hierarchicalLayer);
            };

            container.Add(button);
            return container;
        }
        
        private VisualElement LabelValueTypeBoxButton(GameValueChanger gameValueChanger)
        {
            if (gameValueChanger is not ValueTypeSaveableGameValueChanger valueTypeSaveableGameValueChanger)
                throw new InvalidOperationException($"GameValueChanger with {nameof(LabelValueTypeBoxButton)} template was not a ValueTypeSaveableGameValueChanger.");

            var container = CreateContainer();
            
            container.Add(CreateLabel(valueTypeSaveableGameValueChanger.LabelText));

            var button = CreateButton("Open Value Type");

            var hierarchicalManager = DependencyContainer.GetInstance<HierarchicalManager>();
            var newIndex = hierarchicalManager.CurrentIndex;
            Plugin.Log.LogInfo("newIndex: " + newIndex);
            
            button.clicked += () =>
            {
                var settingChangerContainer = GetGameValueChangerContainer(valueTypeSaveableGameValueChanger.FieldName);
                var settingChangerContainerScrollView = settingChangerContainer.Q<ScrollView>();
                foreach (var fieldValueChanger in valueTypeSaveableGameValueChanger.Fields)
                    settingChangerContainerScrollView.Add(GetUiPreset(fieldValueChanger));
                
                var hierarchicalLayer = new HierarchicalLayer(newIndex, settingChangerContainer);
                hierarchicalManager.OpenNewLayer(hierarchicalLayer);
            };

            container.Add(button);
            return container;
        }

        private VisualElement CreateContainer()
        {
            var container = new NineSliceVisualElement();
            container.AddToClassList("mods-box__game-value-changer-line");
            return container;
        }

        private Button CreateButton(string text)
        {
            var button = _uiPresetFactory.Buttons().ButtonGame(text: text);
            button.style.maxHeight = new Length(20, LengthUnit.Pixel);
            return button;
        }
        
        private VisualElement CreateLabel(string text)
        {
            return _uiPresetFactory.Labels().GameText(text: text);
        }

        private TextField CreateTextField()
        {
            // return _visualElementLoader.LoadVisualElement("Game/BatchControl/PopulationDistributorBatchControlRowItem").Q<TextField>("MinimumValue");
            return new TextField();
        }
        
        private IntegerField CreateIntegerField()
        {
            return _visualElementLoader.LoadVisualElement("Game/BatchControl/PopulationDistributorBatchControlRowItem").Q<IntegerField>("MinimumValue");
        }
    }
}