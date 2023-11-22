using System;
using Timberborn.Persistence;

namespace TobbyTools.CustomTutorialSystem
{
    public class CustomTutorialStepSettingsDeserializer : IObjectSerializer<CustomTutorialStepSettings>
    {
        public void Serialize(CustomTutorialStepSettings value, IObjectSaver objectSaver) => throw new NotSupportedException();

        public Obsoletable<CustomTutorialStepSettings> Deserialize(IObjectLoader objectLoader)
        {
            return (Obsoletable<CustomTutorialStepSettings>) new CustomTutorialStepSettings(
                objectLoader.GetValueOrNullable(new PropertyKey<int>("RequiredAmount")),
                objectLoader.Get(new ListKey<string>("PrefabNames")).ToArray(),
                objectLoader.GetValueOrNull(new PropertyKey<string>("GoodId")));
        }
    }
}
 