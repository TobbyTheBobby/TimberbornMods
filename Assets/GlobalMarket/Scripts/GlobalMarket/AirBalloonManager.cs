// using Bindito.Core;
// using HarmonyLib;
// using Timberborn.AssetSystem;
// using Timberborn.BlockSystem;
// using Timberborn.Characters;
// using Timberborn.ConstructibleSystem;
// using Timberborn.Coordinates;
// using Timberborn.EntitySystem;
// using Timberborn.FactionSystemGame;
// using Timberborn.Localization;
// using Timberborn.Persistence;
// using UnityEngine;
//
// namespace GlobalMarket
// {
//     public class AirBalloonManager : MonoBehaviour, IFinishedStateListener, IPersistentEntity
//     {
//         private static readonly ComponentKey AirBalloonManagerKey = new(nameof (AirBalloonManager));
//         
//         private static readonly PropertyKey<GameObject> AirBalloonKey = new(nameof (_airBalloon));
//         
//         private const string AirBalloonNameLocKey = "Tobbert.AirBalloon.Name";
//
//         private ILoc _loc;
//         
//         private EntityService _entityService;
//
//         private IResourceAssetLoader _resourceAssetLoader;
//
//         private FactionService _factionService;
//
//         private GameObject _airBalloon;
//
//         private GlobalMarketServant _globalMarketServant;
//
//         public bool AirBalloonEnabled { get; private set; } = true;
//
//         [Inject]
//         public void InjectDependencies(ILoc loc, EntityService entityService, IResourceAssetLoader resourceAssetLoader, FactionService factionService)
//         {
//             _loc = loc;
//             _entityService = entityService;
//             _resourceAssetLoader = resourceAssetLoader;
//             _factionService = factionService;
//         }
//         
//         public void Save(IEntitySaver entitySaver)
//         {
//             if (_airBalloon != null)
//                 entitySaver.GetComponent(AirBalloonManagerKey).Set(AirBalloonKey, _airBalloon);
//         }
//         
//         public void Load(IEntityLoader entityLoader)
//         {
//             if (!entityLoader.HasComponent(AirBalloonManagerKey))
//                 return;
//             
//             var component = entityLoader.GetComponent(AirBalloonManagerKey);
//             
//             if (component.Has(AirBalloonKey))
//                 _airBalloon = component.Get(AirBalloonKey);
//         }
//         
//         public void OnEnterFinishedState()
//         {
//             if (_airBalloon == null)
//             {
//                 InitializeAirBalloon();
//             }
//         }
//
//         public void OnExitFinishedState()
//         {
//             _entityService.Delete(_airBalloon);
//         }
//         
//         public void EnableAirBalloon()
//         {
//             AirBalloonEnabled = true;
//             _airBalloon.SetActive(true);
//         }
//         
//         public void DisableAirBalloon()
//         {
//             AirBalloonEnabled = false;
//             _airBalloon.SetActive(false);
//         }
//
//         private void InitializeAirBalloon()
//         {
//             var airBalloonPrefab = _resourceAssetLoader.Load<GameObject>("tobbert.globalmarket/tobbert_globalmarket/AirBalloon." + _factionService.Current.Id);
//             
//             _airBalloon = _entityService.Instantiate(airBalloonPrefab.gameObject);
//             
//             Destroy(_airBalloon.GetComponent(AccessTools.TypeByName("StrandedStatus")));
//
//             SetAirBalloonPosition();
//             
//             SetAirBalloonName();
//
//             var globMarketServant = _airBalloon.GetComponent<GlobalMarketServant>();
//             var globalMarket = gameObject;
//             globMarketServant.LinkedGlobalMarket = globalMarket;
//             globMarketServant.LinkedGlobalMarketPosition = globalMarket.transform.position + GetSpawnOffset();
//         }
//         
//         private void SetAirBalloonPosition()
//         {
//             var position = _airBalloon.transform.position;
//             position += GetSpawnOffset();
//             position += transform.position;
//             _airBalloon.transform.position = position;
//         }
//
//         private Vector3 GetSpawnOffset()
//         {
//             var orientation = GetComponent<BlockObject>().Orientation;
//             switch (orientation)
//             {
//                 default:
//                     return new Vector3(1.9f, 0.45f, 1.8f);
//                 case Orientation.Cw90:
//                     return new Vector3(1.9f, 0.45f, -1.8f);
//                 case Orientation.Cw180:
//                     return new Vector3(-1.9f, 0.45f, -1.8f);
//                 case Orientation.Cw270:
//                     return new Vector3(-1.9f, 0.45f, 1.8f);
//             }
//         }
//
//         private void SetAirBalloonName()
//         {
//             Character component = _airBalloon.GetComponent<Character>();
//             component.FirstName = _loc.T(AirBalloonNameLocKey);
//         }
//     }
// }
