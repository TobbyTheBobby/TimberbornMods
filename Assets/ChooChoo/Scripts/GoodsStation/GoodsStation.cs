using Bindito.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.BuildingsBlocking;
using Timberborn.ConstructibleSystem;
using Timberborn.EntitySystem;
using Timberborn.InventorySystem;

namespace ChooChoo
{
  public class GoodsStation : BaseComponent, IRegisteredComponent, IFinishedStateListener, IPausableComponent
  {
    public static readonly int Capacity = 200;
    private GoodsStationsRepository _goodsStationsRepository;

    private GoodsStationReceivingInventory _goodsStationReceivingInventory;
    private GoodsStationSendingInventory _goodsStationSendingInventory;

    private GoodsStationDistributionSettings _goodsStationDistributionSettings;
    
    public TrainDestination TrainDestinationComponent { get; private set; }

    public Inventory SendingInventory => _goodsStationSendingInventory.Inventory;
    public Inventory ReceivingInventory => _goodsStationReceivingInventory.Inventory;
    public GoodsStationDistributionSettings GoodsStationDistributionSettings => _goodsStationDistributionSettings;
    
    public int MaxCapacity => Capacity;

    [Inject]
    public void InjectDependencies(GoodsStationsRepository goodsStationsRepository)
    {
      _goodsStationsRepository = goodsStationsRepository;
    }

    public void Awake()
    {
      _goodsStationDistributionSettings = GetComponentFast<GoodsStationDistributionSettings>();
      TrainDestinationComponent = GetComponentFast<TrainDestination>();
      _goodsStationReceivingInventory = GetComponentFast<GoodsStationReceivingInventory>();
      _goodsStationSendingInventory = GetComponentFast<GoodsStationSendingInventory>();
      enabled = false;
    }

    public void OnEnterFinishedState()
    {
      enabled = true;
      SendingInventory.Enable();
      ReceivingInventory.Enable();
      _goodsStationsRepository.Register(this);
    }

    public void OnExitFinishedState()
    {
      SendingInventory.Disable();
      ReceivingInventory.Disable();
      enabled = false;
      _goodsStationsRepository.UnRegister(this);
    }
    
    
  }
}
