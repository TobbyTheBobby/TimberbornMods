using Timberborn.BaseComponentSystem;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.InputSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace ChooChoo
{
  internal class TrainDestroyerFragment : IEntityPanelFragment, IInputProcessor
  {
    public static readonly string DeleteObjectKey = "DeleteObject";
    public static readonly string SkipDeleteConfirmationKey = "SkipDeleteConfirmation";
    private static readonly string DeletePromptLocKey = "Tobbert.Train.Destroy";
    private readonly VisualElementLoader _visualElementLoader;
    private readonly DeleteTrainBoxShower _deleteTrainBoxShower;
    private readonly InputService _inputService;
    private Destroyable _destroyable;
    private Button _button;
    private VisualElement _root;

    public TrainDestroyerFragment(
      VisualElementLoader visualElementLoader,
      DeleteTrainBoxShower deleteTrainBoxShower,
      InputService inputService)
    {
      _visualElementLoader = visualElementLoader;
      _deleteTrainBoxShower = deleteTrainBoxShower;
      _inputService = inputService;
    }

    public VisualElement InitializeFragment()
    {
      _root = _visualElementLoader.LoadVisualElement("Common/EntityPanel/DeleteObjectFragment");
      _button = _root.Q<Button>("Button");
      _button.clicked += OnClick;
      _root.ToggleDisplayStyle(false);
      _inputService.AddInputProcessor(this);
      return _root;
    }

    public void ShowFragment(BaseComponent entity)
    {
      _destroyable = entity.GetComponentFast<Destroyable>();
      if (!(bool) (Object) _destroyable)
        return;
      _root.ToggleDisplayStyle(true);
    }

    public void ClearFragment()
    {
      _destroyable = null;
      _root.ToggleDisplayStyle(false);
    }

    public void UpdateFragment()
    {
      if (!(bool) (Object) _destroyable)
        return;
      _button.SetEnabled(SelectedEntityIsDestroyable());
    }

    public bool ProcessInput()
    {
      if (_destroyable && _inputService.IsKeyHeld(SkipDeleteConfirmationKey))
      {
        DeleteBuilding();
        return true;
      }
      
      if (!(bool) (Object) _destroyable || !_inputService.IsKeyDown(DeleteObjectKey))
        return false;
      ShowDialogBox();
      return true;
    }

    private void OnClick()
    {
      if (_inputService.IsKeyHeld(SkipDeleteConfirmationKey))
        DeleteBuilding();
      else
        ShowDialogBox();
    }

    private void ShowDialogBox()
    {
      if (!SelectedEntityIsDestroyable())
        return;
      _deleteTrainBoxShower.Show(DeleteBuilding, DeletePromptLocKey);
    }

    private void DeleteBuilding()
    {
      if (!SelectedEntityIsDestroyable())
        return;
      _destroyable.Destroy();
    }

    private bool SelectedEntityIsDestroyable() => (bool) (Object) _destroyable;
  }
}
