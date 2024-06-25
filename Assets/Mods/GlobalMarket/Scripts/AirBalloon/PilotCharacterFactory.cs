// using System.Collections.Generic;
// using System.Reflection;
// using Timberborn.Beavers;
// using Timberborn.SingletonSystem;
// using UnityEngine;
//
// public class PilotCharacterFactory : ILoadableSingleton
// {
//     private BeaverFactory _beaverFactory;
//
//     private GameObject _pilotPrefab;
//
//     private readonly List<string> _bodyPartsToDisable = new()
//     {
//         "__Barrel",
//         "__Box",
//         "__Log",
//         "__Bag",
//         "__Backpack",
//         "__None"
//     };
//
//     PilotCharacterFactory(BeaverFactory beaverFactory)
//     {
//         _beaverFactory = beaverFactory;
//     }
//
//     public void Load()
//     {
//         InitializePilotCharacter();
//     }
//
//     public GameObject CreatePilot(Transform parent)
//     {
//         return Object.Instantiate(_pilotPrefab, parent).gameObject;
//     }
//     
//     private void InitializePilotCharacter()
//     {
//         Beaver beaver = typeof(BeaverFactory).GetField("_adultPrefab", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_beaverFactory) as Beaver;
//         Transform beaverModel = beaver.transform.GetChild(0).GetChild(0);
//         DisableBodyParts(beaverModel);
//         _pilotPrefab = beaverModel.gameObject;
//     }
//
//     private void DisableBodyParts(Transform beaver)
//     {
//         foreach (var bodyPartName in _bodyPartsToDisable)
//         {
//             DisableBodyPart(beaver, bodyPartName);
//         }
//     }
//
//     private void DisableBodyPart(Transform parent, string bodyPartName)
//     {
//         foreach (Transform child in parent)
//         {
//             if (child.name == bodyPartName)
//                 child.gameObject.SetActive(false);
//                     
//             DisableBodyPart(child, bodyPartName);
//         }
//     }
// }
