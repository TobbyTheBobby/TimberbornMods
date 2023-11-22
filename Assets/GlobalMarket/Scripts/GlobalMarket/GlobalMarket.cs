using Timberborn.Goods;
using Timberborn.Persistence;
using Timberborn.SingletonSystem;

namespace GlobalMarket
{
  public class GlobalMarket : ISaveableSingleton, ILoadableSingleton
  {
    private static readonly SingletonKey GlobalMarketKey = new(nameof (GlobalMarket));
    
    private static readonly PropertyKey<GoodRegistry> GlobalMarketStorageKey = new(nameof (GlobalMarketStorage));
    
    private static readonly PropertyKey<GoodRegistry> GlobalMarketReservedStorageKey = new(nameof (GlobalMarketReservedStorage));
    
    private static readonly PropertyKey<GoodRegistry> GlobalMarketReservedCapacityKey = new(nameof (GlobalMarketReservedCapacity));
    
    private readonly GoodRegistryObjectSerializer _goodRegistryObjectSerializer;
    
    private readonly ISingletonLoader _singletonLoader;

    public static GoodRegistry GlobalMarketStorage = new();
    
    public static GoodRegistry GlobalMarketReservedStorage = new();
    
    public static GoodRegistry GlobalMarketReservedCapacity = new();

    public GlobalMarket(GoodRegistryObjectSerializer goodRegistryObjectSerializer, ISingletonLoader singletonLoader)
    {
      _goodRegistryObjectSerializer = goodRegistryObjectSerializer;
      _singletonLoader = singletonLoader;
    }

    public void Save(ISingletonSaver singletonSaver)
    {
      var singleton = singletonSaver.GetSingleton(GlobalMarketKey);
      singleton.Set(GlobalMarketStorageKey, GlobalMarketStorage, _goodRegistryObjectSerializer);
      singleton.Set(GlobalMarketReservedStorageKey, GlobalMarketReservedStorage, _goodRegistryObjectSerializer);
      singleton.Set(GlobalMarketReservedCapacityKey, GlobalMarketReservedCapacity, _goodRegistryObjectSerializer);
    }

    public void Load()
    {
      if (!_singletonLoader.HasSingleton(GlobalMarketKey))
        return;
      
      var singleton = _singletonLoader.GetSingleton(GlobalMarketKey);
      
      if (singleton.Has(GlobalMarketStorageKey))
        GlobalMarketStorage = singleton.Get(GlobalMarketStorageKey, _goodRegistryObjectSerializer);
      
      if (singleton.Has(GlobalMarketReservedStorageKey)) 
        GlobalMarketReservedStorage = singleton.Get(GlobalMarketReservedStorageKey, _goodRegistryObjectSerializer);
      
      if (singleton.Has(GlobalMarketReservedCapacityKey))
        GlobalMarketReservedCapacity = singleton.Get(GlobalMarketReservedCapacityKey, _goodRegistryObjectSerializer);
    }
  }
}
