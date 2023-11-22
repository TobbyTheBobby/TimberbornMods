using System;
using Timberborn.BaseComponentSystem;
using Timberborn.CharactersUI;
using Timberborn.CoreUI;
using Timberborn.WorkerTypesUI;
using UnityEngine.UIElements;

namespace ChooChoo
{
  internal class PassengerView
  {
    private readonly WorkerTypeHelper _workerTypeHelper;
    private readonly CharacterButton _characterButton;
    private readonly Label _name;

    public VisualElement Root { get; }

    public PassengerView(
      WorkerTypeHelper workerTypeHelper,
      VisualElement root,
      CharacterButton characterButton,
      Label name)
    {
      _workerTypeHelper = workerTypeHelper;
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
