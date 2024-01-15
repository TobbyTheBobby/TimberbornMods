using Timberborn.CharactersUI;
using Timberborn.CoreUI;
using Timberborn.WorkerTypesUI;
using UnityEngine.UIElements;

namespace ChooChoo.PassengerSystemUI
{
    internal class PassengerViewFactory
    {
        private readonly VisualElementLoader _visualElementLoader;
        private readonly CharacterButtonFactory _characterButtonFactory;
        private readonly WorkerTypeHelper _workerTypeHelper;

        public PassengerViewFactory(
            VisualElementLoader visualElementLoader,
            CharacterButtonFactory characterButtonFactory,
            WorkerTypeHelper workerTypeHelper)
        {
            _visualElementLoader = visualElementLoader;
            _characterButtonFactory = characterButtonFactory;
            _workerTypeHelper = workerTypeHelper;
        }

        public PassengerView Create()
        {
            var visualElement1 = _visualElementLoader.LoadVisualElement("Game/EntityPanel/WorkerView");
            var characterButton = _characterButtonFactory.Create(visualElement1.Q<Button>("CharacterButton"));
            visualElement1.Q<Button>("WorkerView").RegisterCallback((EventCallback<ClickEvent>)(_ => characterButton.ClickAction()));
            return new PassengerView(_workerTypeHelper, visualElement1, characterButton, visualElement1.Q<Label>("Name"));
        }
    }
}