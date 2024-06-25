using Timberborn.BaseComponentSystem;
using Timberborn.ToolSystem;

namespace PipetteTool
{
    public interface IPipetteTool
    {
        void SetToolGroup(ToolGroup toolGroup);
        void PostProcessInput();
        // void AddToolButtonToDictionary(BaseComponent gameObject, ToolButton toolButton);
        void OnSelectableObjectSelected(BaseComponent hitObject);
    }
}