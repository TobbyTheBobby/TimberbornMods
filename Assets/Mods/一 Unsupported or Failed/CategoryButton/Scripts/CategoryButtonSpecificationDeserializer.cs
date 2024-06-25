using System;
using Timberborn.Persistence;

namespace CategoryButton
{
    public class CategoryButtonSpecificationDeserializer : IObjectSerializer<CategoryButtonSpecification>
    {
        public void Serialize(CategoryButtonSpecification value, IObjectSaver objectSaver) => throw new NotSupportedException();

        public Obsoletable<CategoryButtonSpecification> Deserialize(IObjectLoader objectLoader)
        {
            return (Obsoletable<CategoryButtonSpecification>) new CategoryButtonSpecification(
                objectLoader.Get(new PropertyKey<string>("Name")),
                objectLoader.Get(new PropertyKey<string>("ToolGroup")), 
                objectLoader.Get(new PropertyKey<int>("ToolOrder")),
                objectLoader.Get(new PropertyKey<string>("ButtonIcon")),
                objectLoader.Get(new ListKey<string>("Buildings")),
                objectLoader.Get(new PropertyKey<string>("DisplayNameLocKey")));
        }
    }
}
