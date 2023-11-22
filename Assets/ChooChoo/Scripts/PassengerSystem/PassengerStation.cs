using System.Collections.Generic;
using System.Linq;
using Bindito.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.BlockSystemNavigation;
using Timberborn.ConstructibleSystem;
using Timberborn.Coordinates;
using Timberborn.EntitySystem;
using Timberborn.GameDistricts;
using Timberborn.PrefabSystem;
using Timberborn.SingletonSystem;
using Timberborn.TimeSystem;
using UnityEngine;

namespace ChooChoo
{
  public class PassengerStation : BaseComponent, IFinishedStateListener, IRegisteredComponent
  {
    [SerializeField]
    private Vector3Int teleportationEdge;
    [SerializeField]
    private float movementSpeedMultiplier = 1f;
    [SerializeField]
    private bool uiEnabled;
    [SerializeField]
    private bool connectsTwoWay;
    [SerializeField] 
    private int maxPassengers;

    private PassengerStationLinkRepository _passengerStationLinkRepository;
    private IDayNightCycle _dayNightCycle;
    private EventBus _eventBus;
    
    private PassengerStationDistrictObject _passengerStationDistrictObject;
    private BlockObjectNavMeshSettings _blockObjectNavMeshSettings;
    private BlockObjectNavMesh _blockObjectNavMesh;
    private DistrictBuilding _districtBuilding;
    private BlockObject _blockObject;
    private Prefab _prefab;
    private BlockObjectNavMeshEdgeSpecification[] _cachedSpecifications;

    public bool UIEnabledEnabled => uiEnabled;
    public string PrefabName => _prefab.PrefabName;
    public float MovementSpeedMultiplier => movementSpeedMultiplier;
    public PassengerStationDistrictObject PassengerStationDistrictObject => _passengerStationDistrictObject;
    public DistrictBuilding DistrictBuilding => _districtBuilding;
    public bool ConnectsTwoWay => connectsTwoWay;
    public List<Passenger> PassengerQueue { get; } = new();
    public List<Passenger> ReservedPassengerQueue { get; } = new();

    public List<Passenger> UnreservedPassengerQueue => PassengerQueue.Where(passenger => !ReservedPassengerQueue.Contains(passenger)).ToList();
    public int MaxPassengers => maxPassengers;

    public Vector3 Location
    {
      get
      {
        var gridCenter = _blockObject.GetPositionedBlock(teleportationEdge).Coordinates - new Vector3(-0.5f, -0.5f, 0.5f);
        var gridCenterGrounded = new Vector3(gridCenter.x, gridCenter.y, _blockObject.Coordinates.z);
        var worldCenterGrounded = CoordinateSystem.GridToWorld(gridCenterGrounded);

        return worldCenterGrounded;
      }
    }

    [Inject]
    public void InjectDependencies(
      PassengerStationLinkRepository passengerStationLinkRepository,
      IDayNightCycle dayNightCycle,
      EventBus eventBus)
    {
      _passengerStationLinkRepository = passengerStationLinkRepository;
      _dayNightCycle = dayNightCycle;
      _eventBus = eventBus;
    }

    private void Awake()
    {
      _passengerStationDistrictObject = GetComponentFast<PassengerStationDistrictObject>();
      _blockObjectNavMeshSettings = GetComponentFast<BlockObjectNavMeshSettings>();
      _blockObjectNavMesh = GetComponentFast<BlockObjectNavMesh>();
      _districtBuilding = GetComponentFast<DistrictBuilding>();
      GetComponentFast<TrainDestination>();
      _blockObject = GetComponentFast<BlockObject>();
      _prefab = GetComponentFast<Prefab>();
      var navMeshEdgeSpecifications = (BlockObjectNavMeshEdgeSpecification[])ChooChooCore.GetInaccessibleField(_blockObjectNavMeshSettings, "_addedEdges");
      _cachedSpecifications = navMeshEdgeSpecifications.ToArray();
      enabled = false;
    }

    public void OnEnterFinishedState()
    {
      enabled = true;
      _eventBus.Register(this);
    }

    public void OnExitFinishedState()
    {
      enabled = false;
      _passengerStationLinkRepository.RemoveInvalidLinks();
      _eventBus.Post(new OnConnectedPassengerStationsUpdated());
      _eventBus.Unregister(this);
      foreach (var passenger in PassengerQueue.ToList()) 
        passenger.ArrivedAtDestination();
    }

    public void Connect(PassengerStation endPoint)
    {
      if (DistrictBuilding.District == null && endPoint.DistrictBuilding.District == null)
      {
        // Plugin.Log.LogWarning("BOTH null");
        // Plugin.Log.LogWarning(Location + "   " + endPoint.Location);
        return;
      }
      
      if (DistrictBuilding.District == null || endPoint.DistrictBuilding.District == null)
      {
        AddPassengerLink(endPoint);
        // Plugin.Log.LogWarning("EITHER null");
        // Plugin.Log.LogWarning(Location + "   " + endPoint.Location);
        return;
      }

      if (PassengerStationDistrictObject.GoesAcrossDistrict && endPoint.PassengerStationDistrictObject.GoesAcrossDistrict &&
          DistrictBuilding.District != endPoint.DistrictBuilding.District)
      {
        AddPassengerLink(endPoint);
        // Plugin.Log.LogWarning("Both goes acress and different district");
        // Plugin.Log.LogWarning(Location + "   " + endPoint.Location);
        return;
      }
      
      if (!PassengerStationDistrictObject.GoesAcrossDistrict && !endPoint.PassengerStationDistrictObject.GoesAcrossDistrict &&
        DistrictBuilding.District == endPoint.DistrictBuilding.District)
      {
        AddPassengerLink(endPoint);
        // Plugin.Log.LogWarning("Both NOT goes acress and same district");
        // Plugin.Log.LogWarning(Location + "   " + endPoint.Location);
        return;
      }
      
      // Plugin.Log.LogError("Couldnt connect");
      // Plugin.Log.LogWarning(Location + "   " + endPoint.Location);
    }

    private void AddPassengerLink(PassengerStation endPoint)
    {
      float waitingTimeInHours = CalculateWaitingTimeInHours(endPoint);
      // Plugin.Log.LogError(waitingTimeInHours + "");
      _passengerStationLinkRepository.AddNew(new PassengerStationLink(this, endPoint, waitingTimeInHours));
      // if (connectsTwoWay)
      //   _passengerStationLinkRepository.AddNew(new PassengerStationLink(endPoint, this, waitingTimeInHours));
    }

    [OnEvent]
    public void OnPathLinksUpdated(OnConnectedPassengerStationsUpdated onPathLinksUpdated) => UpdateNavMesh();

    private float CalculateWaitingTimeInHours(PassengerStation endPoint) => _dayNightCycle.SecondsToHours(Vector3.Distance(Location, endPoint.Location) / (2.7f * movementSpeedMultiplier));

    private void UpdateNavMesh()
    {
      List<BlockObjectNavMeshEdgeSpecification> list = _cachedSpecifications.ToList();
      foreach (PassengerStationLink pathLink in _passengerStationLinkRepository.PathLinks(this))
      {
        Vector3Int vector = (pathLink.EndLinkPoint.Location - Location).ToBlockServicePosition();
        Vector3Int end = _blockObject.Orientation.Untransform(vector);
        // Vector3Int end2 = _blockObject.Orientation.Transform(vector);
        // var end3 = _blockObject.Orientation.Transform(pathLink.EndLinkPoint.Location - Location);
        // var end4 = _blockObject.Orientation.Transform(pathLink.EndLinkPoint.Location - Location).ToBlockServicePosition();
        // Plugin.Log.LogWarning(pathLink.EndLinkPoint.Location + "   " + Location);
        // Plugin.Log.LogError(vector.ToString());
        // Plugin.Log.LogError(_blockObject.Orientation.ToString());
        // Plugin.Log.LogError(end.ToString());
        // Plugin.Log.LogError(end2.ToString());
        // Plugin.Log.LogError(end3.ToString()); // BAD
        // Plugin.Log.LogError(end4.ToString());
        list.Add(CreateNewBlockObjectNavMeshEdgeSpecification(teleportationEdge, end, connectsTwoWay));
      }
      _blockObjectNavMesh.NavMeshObject?.RemoveFromNavMesh();
      _blockObjectNavMesh.NavMeshObject?.RemoveFromPreviewNavMesh();
      ChooChooCore.SetInaccessibleField(_blockObjectNavMeshSettings, "_addedEdges", list.ToArray());
      _blockObjectNavMesh.RecalculateNavMeshObject();
      _blockObjectNavMesh.NavMeshObject.AddToNavMesh();
    }

    private BlockObjectNavMeshEdgeSpecification CreateNewBlockObjectNavMeshEdgeSpecification(
      Vector3Int start,
      Vector3Int end,
      bool isTwoWay)
    {
      BlockObjectNavMeshEdgeSpecification instance = new BlockObjectNavMeshEdgeSpecification();
      ChooChooCore.SetInaccessibleField(instance, "_start", start);
      ChooChooCore.SetInaccessibleField(instance, "_end", end);
      ChooChooCore.SetInaccessibleField(instance, "_isTwoWay", isTwoWay);
      return instance;
    }
  }
}
