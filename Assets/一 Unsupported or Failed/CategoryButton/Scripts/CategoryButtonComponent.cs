using System.Collections.Generic;
using System.Linq;
using Timberborn.ToolSystem;
using UnityEngine;

namespace CategoryButton
{
    public class CategoryButtonComponent : MonoBehaviour
    {
        public List<string> ToolBarButtonNames;
        
        public List<ToolButton> ToolButtons = new();
        
        public List<Tool> ToolList = new();
        
        public void SetToolList()
        {
            ToolList = ToolButtons.Select(button => button.Tool).ToList();
        }
    }
}
