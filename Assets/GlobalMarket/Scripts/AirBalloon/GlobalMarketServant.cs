// using Timberborn.Persistence;
// using UnityEngine;
//
// public class GlobalMarketServant : MonoBehaviour, IPersistentEntity
// {
//     private static readonly ComponentKey GlobalMarketServantKey = new(nameof (GlobalMarketServant));
//     
//     private static readonly PropertyKey<GameObject> LinkedGlobalMarketKey = new(nameof (LinkedGlobalMarket));
//     
//     private static readonly PropertyKey<Vector3> LinkedGlobalMarketPositionKey = new(nameof (LinkedGlobalMarketPosition));
//
//     public GameObject LinkedGlobalMarket { get; set; }
//
//     public Vector3 LinkedGlobalMarketPosition { get; set; }
//
//     public void Save(IEntitySaver entitySaver) 
//     {
//         entitySaver.GetComponent(GlobalMarketServantKey).Set(LinkedGlobalMarketKey, LinkedGlobalMarket);
//         entitySaver.GetComponent(GlobalMarketServantKey).Set(LinkedGlobalMarketPositionKey, LinkedGlobalMarketPosition);
//     }
//         
//     public void Load(IEntityLoader entityLoader)
//     {
//         LinkedGlobalMarket = entityLoader.GetComponent(GlobalMarketServantKey).Get(LinkedGlobalMarketKey);
//         LinkedGlobalMarketPosition = entityLoader.GetComponent(GlobalMarketServantKey).Get(LinkedGlobalMarketPositionKey);
//     }
// }
