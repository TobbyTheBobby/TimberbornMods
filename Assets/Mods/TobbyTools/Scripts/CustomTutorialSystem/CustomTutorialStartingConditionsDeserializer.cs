using System;
using Timberborn.Persistence;

namespace TobbyTools.CustomTutorialSystem
{
    public class CustomTutorialStartingConditionsDeserializer : IObjectSerializer<CustomTutorialStartingConditions>
    {
        public void Serialize(CustomTutorialStartingConditions value, IObjectSaver objectSaver) => throw new NotSupportedException();

        public Obsoletable<CustomTutorialStartingConditions> Deserialize(IObjectLoader objectLoader)
        {
            return (Obsoletable<CustomTutorialStartingConditions>) new CustomTutorialStartingConditions(
                objectLoader.Get(new PropertyKey<bool>("RunAtStart")),
                objectLoader.Get(new PropertyKey<string>("Faction")));
        }
    }
}
