using System;
using Timberborn.BaseComponentSystem;
using Timberborn.CharactersUI;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace ChooChoo.PassengerSystemUI
{
    internal class PassengerView
    {
        private readonly CharacterButton _characterButton;
        private readonly Label _name;

        public VisualElement Root { get; }

        public PassengerView(
            VisualElement root,
            CharacterButton characterButton,
            Label name)
        {
            _characterButton = characterButton;
            Root = root;
            _name = name;
        }

        public void Fill(BaseComponent user, Action onClick, string description)
        {
            _characterButton.ShowFilled(user, onClick);
            _name.text = description;
            Root.SetEnabled(true);
            Root.ToggleDisplayStyle(true);
        }

        public void ShowEmpty()
        {
            // _characterButton.Root.ToggleDisplayStyle(false);
            _name.text = "";
            Root.SetEnabled(false);
            Root.ToggleDisplayStyle(false);
        }
    }
}