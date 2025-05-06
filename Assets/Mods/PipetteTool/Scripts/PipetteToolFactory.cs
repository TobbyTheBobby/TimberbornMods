using TimberApi.Tools.ToolSystem;
using Timberborn.ToolSystem;

namespace PipetteTool
{
    public class PipetteToolFactory : IToolFactory
    {
        private readonly IPipetteTool _pipetteTool;
        
        public string Id => "PipetteTool";
        
        public PipetteToolFactory(IPipetteTool pipetteTool)
        {
            _pipetteTool = pipetteTool;
        }
        
        public Tool Create(ToolSpec toolSpecification, ToolGroup toolGroup = null)
        {
            _pipetteTool.SetToolGroup(toolGroup);
            return (Tool)_pipetteTool;
        }
    }
}