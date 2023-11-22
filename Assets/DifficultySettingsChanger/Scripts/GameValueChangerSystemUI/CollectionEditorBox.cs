using System.Collections.Generic;
using TimberApi.UiBuilderSystem;
using TimberApi.UiBuilderSystem.PresetSystem;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace DifficultySettingsChanger
{
    public class CollectionEditorBox : IPanelController
    {
        private readonly GameValueChangerUiPresetFactory _gameValueChangerUiPresetFactory;
        private readonly UiPresetFactory _uiPresetFactory;
        private readonly PanelStack _panelStack;
        private readonly UIBuilder _uiBuilder;
        
        private CollectionSaveableGameValueChanger _collectionSaveableGameValueChanger;

        private List<GameValueChanger> _gameValueChangers;

        CollectionEditorBox(GameValueChangerUiPresetFactory gameValueChangerUiPresetFactory, UiPresetFactory uiPresetFactory, PanelStack panelStack, UIBuilder uiBuilder)
        {
            _gameValueChangerUiPresetFactory = gameValueChangerUiPresetFactory;
            _uiPresetFactory = uiPresetFactory;
            _panelStack = panelStack;
            _uiBuilder = uiBuilder;
        }

        public void SetCollectionSaveableGameValueChanger(CollectionSaveableGameValueChanger collectionSaveableGameValueChanger)
        {
            _collectionSaveableGameValueChanger = collectionSaveableGameValueChanger;

            // var list = new List<GameValueChanger>();
            // foreach (var item in (IEnumerable<object>)_collectionSaveableGameValueChanger.FieldRef.Value)
            // {
            //     list.Add(new GameValueChanger(
            //         new FieldRef(
            //         () => item,
            //         value => { }),
            //         "",
            //         "",
            //         "",
            //         "",
            //         false));
            // }
        }
            
        public VisualElement GetPanel()
        {
            var settingChangerContainer = _gameValueChangerUiPresetFactory.GetGameValueChangerContainer("SHOULDNT BE USED");
            
            foreach (var gameValueChanger in _collectionSaveableGameValueChanger.GameValueChangers)
            {
                settingChangerContainer.Add(_gameValueChangerUiPresetFactory.GetUiPreset(gameValueChanger));
            }

            var button = _uiPresetFactory.Buttons().ButtonGame(locKey: "Tobbert.DifficultySettingsChanger.ApplySettings", name: "SettingsApplierButton");
            button.style.marginTop = new Length(40, LengthUnit.Pixel);
            settingChangerContainer.Add(button);
            
            var box = _uiBuilder.CreateBoxBuilder()
                .SetBoxInCenter()
                .AddCloseButton("CloseButton")
                
                .AddHeader("Tobbert.DifficultySettingsChanger.Header")
                .SetWidth(new Length(800, LengthUnit.Pixel))
                .SetHeight(new Length(600, LengthUnit.Pixel))

                .AddComponent(settingChangerContainer)
                
                .BuildAndInitialize();

            box.Q<Button>("SettingsApplierButton").RegisterCallback<ClickEvent>(_ => OnUIConfirmed());
            box.Q<Button>("CloseButton").RegisterCallback<ClickEvent>(_ => OnUICancelled());

            return box;
        }

        public bool OnUIConfirmed()
        {
            _panelStack.Pop(this);
            _collectionSaveableGameValueChanger = null;
            return true;
        }

        public void OnUICancelled()
        {
            _panelStack.Pop(this);
            _collectionSaveableGameValueChanger = null;
        }
    }
}