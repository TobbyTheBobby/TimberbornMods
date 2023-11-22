using System.Collections.Generic;
using Timberborn.Common;
using Timberborn.ConstructibleSystem;
using Timberborn.GameDistricts;
using Timberborn.SingletonSystem;

namespace ChooChoo
{
  public class GoodsStationRegistry : ILoadableSingleton
  {
    private readonly EventBus _eventBus;
    private readonly List<GoodsStation> _allGoodsStations = new();
    private readonly List<GoodsStation> _finishedGoodsStations = new();

    public GoodsStationRegistry(EventBus eventBus) => _eventBus = eventBus;

    public ReadOnlyList<GoodsStation> AllGoodsStations => _allGoodsStations.AsReadOnlyList();

    public ReadOnlyList<GoodsStation> FinishedGoodsStations => _finishedGoodsStations.AsReadOnlyList();

    public void Load()
    {
      _eventBus.Register(this);
    }

    [OnEvent]
    public void OnConstructibleEnteredUnfinishedState(ConstructibleEnteredUnfinishedStateEvent constructibleEnteredUnfinishedStateEvent)
    {
      if (!constructibleEnteredUnfinishedStateEvent.Constructible.TryGetComponentFast(out GoodsStation component))
        return;
      AddDistrictCenter(component);
    }

    [OnEvent]
    public void OnConstructibleExitedUnfinishedState(ConstructibleExitedUnfinishedStateEvent constructibleExitedUnfinishedStateEvent)
    {
      if (!constructibleExitedUnfinishedStateEvent.Constructible.TryGetComponentFast(out GoodsStation component))
        return;
      RemoveDistrictCenter(component);
    }

    [OnEvent]
    public void OnConstructibleEnteredFinishedState(ConstructibleEnteredFinishedStateEvent constructibleEnteredFinishedStateEvent)
    {
      if (!constructibleEnteredFinishedStateEvent.Constructible.TryGetComponentFast(out GoodsStation component))
        return;
      RegisterFinishedDistrictCenter(component);
    }

    [OnEvent]
    public void OnConstructibleExitedFinishedState(ConstructibleExitedFinishedStateEvent constructibleExitedFinishedStateEvent)
    {
      if (!constructibleExitedFinishedStateEvent.Constructible.TryGetComponentFast(out GoodsStation component))
        return;
      UnregisterFinishedDistrictCenter(component);
    }

    private void RegisterFinishedDistrictCenter(GoodsStation districtCenter)
    {
      AddDistrictCenter(districtCenter);
      _finishedGoodsStations.Add(districtCenter);
      _eventBus.Post(new DistrictCenterRegistryChangedEvent());
    }

    private void UnregisterFinishedDistrictCenter(GoodsStation districtCenter)
    {
      RemoveDistrictCenter(districtCenter);
      _finishedGoodsStations.Remove(districtCenter);
      _eventBus.Post(new DistrictCenterRegistryChangedEvent());
    }

    private void AddDistrictCenter(GoodsStation districtCenter)
    {
      _allGoodsStations.Add(districtCenter);
    }

    private void RemoveDistrictCenter(GoodsStation districtCenter)
    {
      _allGoodsStations.Remove(districtCenter);
    }
  }
}
