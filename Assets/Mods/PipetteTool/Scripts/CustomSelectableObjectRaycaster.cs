// using System;
// using Timberborn.BaseComponentSystem;
// using Timberborn.CameraSystem;
// using Timberborn.Coordinates;
// using Timberborn.InputSystem;
// using Timberborn.SelectionSystem;
// using Timberborn.TerrainSystem;
// using UnityEngine;
// using Object = UnityEngine.Object;
//
// namespace PipetteTool
// {
//   public class CustomSelectableObjectRaycaster
//   {
//     private readonly TerrainPicker _terrainPicker;
//     private readonly CameraComponent _cameraComponent;
//     private readonly InputService _inputService;
//     private readonly SelectableObjectRetriever _selectableObjectRetriever;
//
//     public CustomSelectableObjectRaycaster(
//       TerrainPicker terrainPicker,
//       CameraComponent cameraComponent,
//       InputService inputService,
//       SelectableObjectRetriever selectableObjectRetriever)
//     {
//       _terrainPicker = terrainPicker;
//       _cameraComponent = cameraComponent;
//       _inputService = inputService;
//       _selectableObjectRetriever = selectableObjectRetriever;
//     }
//
//     public bool TryHitSelectableObject(out BaseComponent hitObject, params Type[] filterTypes)
//     {
//       var rayInWorldSpace = _cameraComponent.ScreenPointToRayInWorldSpace(_inputService.MousePosition);
//
//       foreach (var rayhit in Physics.RaycastAll(rayInWorldSpace))
//       {
//         if (!HitIsCloserThanTerrain(rayInWorldSpace, rayhit))
//           break;
//         var gameObject = rayhit.collider.gameObject;
//         if (!(bool)(Object)gameObject) 
//           continue;
//         if (!IsValidObject(gameObject, filterTypes))
//           continue;
//         hitObject = _selectableObjectRetriever.GetSelectableObject(gameObject);
//         return true;
//       }
//
//       hitObject = null;
//       return false;
//     }
//
//     private bool HitIsCloserThanTerrain(Ray ray, RaycastHit hit)
//     {
//       return !HitTerrain(ray, out var distance) || hit.distance < (double) distance;
//     }
//
//     private bool HitTerrain(Ray ray, out float distance)
//     {
//       var grid = CoordinateSystem.WorldToGrid(ray);
//       var nullable = _terrainPicker.PickTerrainCoordinates(grid);
//       if (nullable.HasValue)
//       {
//         var valueOrDefault = nullable.GetValueOrDefault();
//         distance = Vector3.Distance(grid.origin, valueOrDefault.Intersection);
//         return true;
//       }
//       distance = 0.0f;
//       return false;
//     }
//
//     private bool IsValidObject(GameObject gameObject, Type[] filterTypes)
//     {
//       bool invalidObject = false;
//       foreach (Type typeToBeFiltered in filterTypes)
//       {
//         var component = gameObject.GetComponentInParent(typeToBeFiltered);
//         // Plugin.Log.LogInfo(typeToBeFiltered.Name + (component != null) + "");
//         if (component == null) 
//           continue;
//         invalidObject = true;
//         break;
//       }
//       
//       return !invalidObject;
//     }
//   }
// }
