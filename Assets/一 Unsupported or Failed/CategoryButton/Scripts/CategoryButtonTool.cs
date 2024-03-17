using Timberborn.BlockObjectTools;
using Timberborn.BlockSystem;
using Timberborn.CoreUI;
using Timberborn.InputSystem;
using Timberborn.ToolSystem;
using UnityEngine.UIElements;
using Tool = Timberborn.ToolSystem.Tool;

namespace CategoryButton
{
  public class CategoryButtonTool : Tool, IInputProcessor
  {
    private readonly BlockObjectToolDescriber _blockObjectToolDescriber;
    private readonly ToolManager _toolManager;
    private readonly InputService _inputService;
    private readonly CategoryButtonService _categoryButtonService;

    private PlaceableBlockObject _prefab;
    public VisualElement ToolButtonsVisualElement;
    public CategoryButtonComponent ToolBarCategoryComponent;

    public Tool ActiveTool = null;
    private bool _active;
    private MouseController _mouseController;
    
    public CategoryButtonTool(BlockObjectToolDescriber blockObjectToolDescriber, ToolManager toolManager, InputService inputService, CategoryButtonService categoryButtonService)
    {
      _blockObjectToolDescriber = blockObjectToolDescriber;
      _toolManager = toolManager;
      _inputService = inputService;
      _categoryButtonService = categoryButtonService;
    }

    public override bool DevModeTool => _prefab.DevModeTool;

    public void SetFields(PlaceableBlockObject prefab, VisualElement visualElement, ToolGroup toolGroup, CategoryButtonComponent toolBarCategory)
    {
      _prefab = prefab;
      ToolButtonsVisualElement = visualElement;
      ToolGroup = toolGroup;
      ToolBarCategoryComponent = toolBarCategory;
      _mouseController = (MouseController)_categoryButtonService.GetPrivateField(_inputService, "_mouse");
    }

    public override void Enter()
    {
      _inputService.AddInputProcessor(this);
      _active = true;
      _categoryButtonService.SetDescriptionPanelHeight(60);
      _categoryButtonService.UpdateScreenSize(this);
      ToolButtonsVisualElement.ToggleDisplayStyle(true);
      
      if (ActiveTool != null)
        _toolManager.SwitchTool(ActiveTool);
    }

    public override void Exit()
    {
      _inputService.RemoveInputProcessor(this);
      
      if (_active) _categoryButtonService.SetDescriptionPanelHeight(0);
      _active = false;

      ToolButtonsVisualElement.ToggleDisplayStyle(false);
    }

    public bool ProcessInput()
    {
      // if (!_inputService.IsShiftHeld) return false;
      //
      // int index = ToolBarCategoryComponent.ToolList.IndexOf(ActiveTool);
      //
      // if (_mouseController.ScrollWheelAxis > 0)
      // {
      //   while (index + 1 < ToolBarCategoryComponent.ToolList.Count && ToolBarCategoryComponent.ToolList[index + 1].Locked)
      //   {
      //     index += 1;
      //   }
      //   if (index + 1 < ToolBarCategoryComponent.ToolList.Count() && !ToolBarCategoryComponent.ToolList[index + 1].Locked)
      //   {
      //     _toolManager.SwitchTool(ToolBarCategoryComponent.ToolList[index + 1]);
      //   }
      // }
      // else if (_mouseController.ScrollWheelAxis < 0)
      // {
      //   while (index - 1 >= 0 && ToolBarCategoryComponent.ToolList[index - 1].Locked)
      //   {
      //     index -= 1;
      //   }
      //   if (index - 1 >= 0 && !ToolBarCategoryComponent.ToolList[index - 1].Locked)
      //   {
      //     _toolManager.SwitchTool(ToolBarCategoryComponent.ToolList[index - 1]);
      //   }
      // }

      return false;
    }

    public override ToolDescription Description()
    {
      var builder = _blockObjectToolDescriber.DescribePrefab(_prefab);
      
      return builder.Build();
    }
  }
}
