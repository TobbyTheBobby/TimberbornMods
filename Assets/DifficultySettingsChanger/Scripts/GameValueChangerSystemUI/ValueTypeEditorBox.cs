using DifficultySettingsChanger.GameValueChangerSystem;
using TimberApi.UiBuilderSystem;
using TimberApi.UiBuilderSystem.PresetSystem;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace DifficultySettingsChanger.GameValueChangerSystemUI
{
    public class ValueTypeEditorBox : IPanelController
    {
        private readonly GameValueChangerUiPresetFactory _gameValueChangerUiPresetFactory;
        private readonly UiPresetFactory _uiPresetFactory;
        private readonly PanelStack _panelStack;
        private readonly UIBuilder _uiBuilder;
        
        private ValueTypeSaveableGameValueChanger _valueTypeSaveableGameValueChanger;

        ValueTypeEditorBox(GameValueChangerUiPresetFactory gameValueChangerUiPresetFactory, UiPresetFactory uiPresetFactory, PanelStack panelStack, UIBuilder uiBuilder)
        {
            _gameValueChangerUiPresetFactory = gameValueChangerUiPresetFactory;
            _uiPresetFactory = uiPresetFactory;
            _panelStack = panelStack;
            _uiBuilder = uiBuilder;
        }

        public void SetValueTypeSaveableGameValueChanger(ValueTypeSaveableGameValueChanger valueTypeSaveableGameValueChanger)
        {
            _valueTypeSaveableGameValueChanger = valueTypeSaveableGameValueChanger;
        }
            
        public VisualElement GetPanel()
        {
            var settingChangerContainer = _gameValueChangerUiPresetFactory.GetGameValueChangerContainer("SHOULDNT BE USED");
            
            foreach (var fieldValueChanger in _valueTypeSaveableGameValueChanger.Fields)
            {
                settingChangerContainer.Add(_gameValueChangerUiPresetFactory.GetUiPreset(fieldValueChanger));
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
            _valueTypeSaveableGameValueChanger = null;
            return true;
        }

        public void OnUICancelled()
        {
            _panelStack.Pop(this);
            _valueTypeSaveableGameValueChanger = null;
        }
    }
}