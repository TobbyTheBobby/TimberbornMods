using System.Collections.Generic;
using Timberborn.CoreUI;
using TobbyTools.NewGameModeValueSystem.UiPresets;
using UnityEngine.UIElements;

namespace TobbyTools.NewGameModeValueSystem
{
    public class SettingSection
    {
        public string SectionLocKey { get; }
        public List<INewGameModeValue> NewGameModeValues { get; }
        public bool Enabled { get; private set; } = true;

        public VisualElement Root;
        public VisualElement SectionWrapperRoot;
        

        public SettingSection(string sectionLocKey, List<INewGameModeValue> newGameModeValues)
        {
            SectionLocKey = sectionLocKey;
            NewGameModeValues = newGameModeValues;
            Initialize();
        }

        public void Initialize()
        {
            Root = SettingSectionHeaderPreset.GetVisualElement(SectionLocKey, Enabled, SetSectionState);
            SectionWrapperRoot = new VisualElement();
            foreach (var newGameModeValue in NewGameModeValues) 
                SectionWrapperRoot.Add(newGameModeValue.GetVisualElement());
        }

        private void SetSectionState(bool newState)
        {
            Enabled = newState;
            foreach (var newGameModeValue in NewGameModeValues) 
                newGameModeValue.SetState(newState);
            SectionWrapperRoot.ToggleDisplayStyle(newState);
        }
    }
}