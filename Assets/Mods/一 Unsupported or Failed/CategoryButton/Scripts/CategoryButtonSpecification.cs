using System.Collections.Generic;
using System.Collections.Immutable;

namespace CategoryButton
{
    public class CategoryButtonSpecification 
    {
        public CategoryButtonSpecification(
            string name,
            string toolGroup,
            int toolOrder,
            string buttonIcon,
            IEnumerable<string> buildings,
            string displayNameLocKey)
        {
            Name = name;
            ToolGroup = toolGroup;
            ToolOrder = toolOrder;
            ButtonIcon = buttonIcon;
            Buildings = buildings.ToImmutableArray();;
            DisplayNameLocKey = displayNameLocKey;
        }

        public string Name { get; }
        public string ToolGroup { get; }
        public int ToolOrder { get; }
        public string ButtonIcon { get; }
        public ImmutableArray<string> Buildings { get; }
        public string DisplayNameLocKey { get; }
    }
}