using System.Collections.Generic;
using Timberborn.Common;
using Timberborn.ConstructibleSystem;
using Timberborn.SingletonSystem;

namespace TobbyTools.BuildingRegistrySystem
{
    public class BuildingRegistry<T> : ILoadableSingleton
    {
        private readonly EventBus _eventBus;
        private readonly List<T> _all = new();
        private readonly List<T> _finished = new();

        public BuildingRegistry(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public ReadOnlyList<T> All => _all.AsReadOnlyList();

        public ReadOnlyList<T> Finished => _finished.AsReadOnlyList();

        public void Load()
        {
            _eventBus.Register(this);
        }

        [OnEvent]
        public void OnConstructibleEnteredUnfinishedState(ConstructibleEnteredUnfinishedStateEvent constructibleEnteredUnfinishedStateEvent)
        {
            if (!constructibleEnteredUnfinishedStateEvent.Constructible.TryGetComponentFast(out T component))
                return;
            AddBuilding(component);
        }

        [OnEvent]
        public void OnConstructibleExitedUnfinishedState(ConstructibleExitedUnfinishedStateEvent constructibleExitedUnfinishedStateEvent)
        {
            if (!constructibleExitedUnfinishedStateEvent.Constructible.TryGetComponentFast(out T component))
                return;
            RemoveBuilding(component);
        }

        [OnEvent]
        public void OnConstructibleEnteredFinishedState(ConstructibleEnteredFinishedStateEvent constructibleEnteredFinishedStateEvent)
        {
            if (!constructibleEnteredFinishedStateEvent.Constructible.TryGetComponentFast(out T component))
                return;
            RegisterFinishedBuilding(component);
        }

        [OnEvent]
        public void OnConstructibleExitedFinishedState(ConstructibleExitedFinishedStateEvent constructibleExitedFinishedStateEvent)
        {
            if (!constructibleExitedFinishedStateEvent.Constructible.TryGetComponentFast(out T component))
                return;
            UnregisterFinishedBuilding(component);
        }

        private void RegisterFinishedBuilding(T building)
        {
            AddBuilding(building);
            _finished.Add(building);
            _eventBus.Post(new BuildingRegistryChangedEvent<T>());
        }

        private void UnregisterFinishedBuilding(T building)
        {
            RemoveBuilding(building);
            _finished.Remove(building);
            _eventBus.Post(new BuildingRegistryChangedEvent<T>());
        }

        private void AddBuilding(T building)
        {
            _all.Add(building);
        }

        private void RemoveBuilding(T building)
        {
            _all.Remove(building);
        }
    }
}