using System;
using Timberborn.Coordinates;
using UnityEngine;

namespace ChooChoo
{
   public class TrackArrayProvider
   {
      public TrackRoute[] GetConnections(string prefabName)
      {
         prefabName = FixPrefabName(prefabName);
         // Plugin.Log.LogWarning("Providing TrackRoutes: " + prefabName);
         switch (prefabName)
         {
            case "TrackOneWay":
               return new TrackRoute[]
               {
                  new(
                     new TrackConnection(new Vector3Int(0, 0, 0), Direction2D.Up),
                     new TrackConnection(new Vector3Int(0, 0, 0), Direction2D.Down), 
                     new[] { Vector3.zero }),
                  new(
                     new TrackConnection(new Vector3Int(0, 0, 0), Direction2D.Down),
                     new TrackConnection(new Vector3Int(0, 0, 0), Direction2D.Up), 
                     new[] { Vector3.zero })
               };
            case "TrackCrossing":
               return new TrackRoute[]
               {
                  new(
                     new TrackConnection(new Vector3Int(0, 0, 0), Direction2D.Up),
                     new TrackConnection(new Vector3Int(0, 0, 0), Direction2D.Down), 
                     new[] { Vector3.zero }),
                  new(
                     new TrackConnection(new Vector3Int(0, 0, 0), Direction2D.Down),
                     new TrackConnection(new Vector3Int(0, 0, 0), Direction2D.Up), 
                     new[] { Vector3.zero })
               };
            case "TrackHill1x2":
               return new TrackRoute[]
               {
                  new(
                     new TrackConnection(new Vector3Int(0, 0, 0), Direction2D.Up),
                     new TrackConnection(new Vector3Int(0, 1, 1), Direction2D.Down), 
                     new Vector3[]
                     {
                        new(0, 0, -1),
                        new(0, 1, 1)
                     }),
                  new(
                     new TrackConnection(new Vector3Int(0, 1, 1), Direction2D.Down),
                     new TrackConnection(new Vector3Int(0, 0, 0), Direction2D.Up), 
                     new Vector3[]
                     {
                        new(0, 1, 1),
                        new(0, 0, -1),
                     })
               };
            case "TrackCorner1x1":
               return new TrackRoute[]
               {
                  new(
                     new TrackConnection(new Vector3Int(0, 0, 0), Direction2D.Up), 
                     new TrackConnection(new Vector3Int(0, 0, 0), Direction2D.Right), 
                     new[] { Vector3.zero }),
                  new(
                     new TrackConnection(new Vector3Int(0, 0, 0), Direction2D.Right),
                     new TrackConnection(new Vector3Int(0, 0, 0), Direction2D.Up), 
                     new[] { Vector3.zero })
               };
            case "TrackCorner2x2":
               return new TrackRoute[]
               {
                  new(
                     new(new Vector3Int(1, 0, 0), Direction2D.Up), 
                     new(new Vector3Int(0, 1, 0), Direction2D.Right), new Vector3[]
                     {
                        new(0.5f, 0, -1f),
                        new(0.4f, 0, -0.35f),
                        new(0.1f, 0, 0.1f),
                        new(-0.35f, 0, 0.4f),
                        new(-1f, 0, 0.5f),
                     }),
                  new(
                     new(new Vector3Int(0, 1, 0), Direction2D.Right),
                     new(new Vector3Int(1, 0, 0), Direction2D.Up),
                     
                     new Vector3[]
                     {
                        new(-1f, 0, 0.5f),
                        new(-0.35f, 0, 0.4f),
                        new(0.1f, 0, 0.1f),
                        new(0.4f, 0, -0.35f),
                        new(0.5f, 0, -1f),
                     })
               };
            case "TrackTIntersection1x1":
                return new TrackRoute[]
               {
                  new(
                     new(new Vector3Int(0, 0, 0), Direction2D.Up),
                     new(new Vector3Int(0, 0, 0), Direction2D.Right), 
                     new[] { Vector3.zero }),
                  new(
                     new(new Vector3Int(0, 0, 0), Direction2D.Right),
                     new(new Vector3Int(0, 0, 0), Direction2D.Up), 
                     new[] { Vector3.zero }),
                  new(
                     new(new Vector3Int(0, 0, 0), Direction2D.Up),
                     new(new Vector3Int(0, 0, 0), Direction2D.Down), 
                     new[] { Vector3.zero }),
                  new(
                     new(new Vector3Int(0, 0, 0), Direction2D.Down),
                     new(new Vector3Int(0, 0, 0), Direction2D.Up), 
                     new[] { Vector3.zero }),
                  new(
                     new(new Vector3Int(0, 0, 0), Direction2D.Down),
                     new(new Vector3Int(0, 0, 0), Direction2D.Right), 
                     new[] { Vector3.zero }),
                  new(
                     new(new Vector3Int(0, 0, 0), Direction2D.Right),
                     new(new Vector3Int(0, 0, 0), Direction2D.Down), 
                     new[] { Vector3.zero })
               };
            case "TrackTIntersection2x3":
               return new TrackRoute[]
               {
                  new(
                     new(new Vector3Int(1, 0, 0), Direction2D.Up), 
                     new(new Vector3Int(0, 1, 0), Direction2D.Right), 
                     new Vector3[]
                     {
                        new(0.5f, 0, -1.5f),
                        new(0.4f, 0, -0.85f),
                        new(0.1f, 0, -0.4f),
                        new(-0.35f, 0, -0.1f),
                        new(-1f, 0, 0f),
                     }),
                  new(
                     new(new Vector3Int(0, 1, 0), Direction2D.Right),
                     new(new Vector3Int(1, 0, 0), Direction2D.Up),
                     new Vector3[]
                     {
                        new(-1f, 0, 0f),
                        new(-0.35f, 0, -0.1f),
                        new(0.1f, 0, -0.4f),
                        new(0.4f, 0, -0.85f),
                        new(0.5f, 0, -1.5f),
                     }),
                  new(
                     new(new Vector3Int(1, 0, 0), Direction2D.Up),
                     new(new Vector3Int(1, 2, 0), Direction2D.Down), 
                     new Vector3[] { new(0.5f, 0, 0), }),
                  new(
                     new(new Vector3Int(1, 2, 0), Direction2D.Down),
                     new(new Vector3Int(1, 0, 0), Direction2D.Up), 
                     new Vector3[] { new(0.5f, 0, 0), }),
                  new(
                     new(new Vector3Int(1, 2, 0), Direction2D.Down),
                     new(new Vector3Int(0, 1, 0), Direction2D.Right), 
                     new Vector3[]
                     {
                        new(0.5f, 0, 1.5f),
                        new(0.4f, 0, 0.85f),
                        new(0.1f, 0, 0.4f),
                        new(-0.35f, 0, 0.1f),
                        new(-1f, 0, 0f),
                     }),
                  new(
                     new(new Vector3Int(0, 1, 0), Direction2D.Right),
                     new(new Vector3Int(1, 2, 0), Direction2D.Down), 
                     new Vector3[]
                     {
                        new(-1f, 0, 0f),
                        new(-0.35f, 0, 0.1f),
                        new(0.1f, 0, 0.4f),
                        new(0.4f, 0, 0.85f),
                        new(0.5f, 0, 1.5f),
                     }),
               };
            case "TrackXIntersection1x1":
               return new TrackRoute[]
               {
                  // Up
                  new(
                     new(new Vector3Int(0, 0, 0), Direction2D.Up),
                     new(new Vector3Int(0, 0, 0), Direction2D.Right), 
                     new[] { Vector3.zero }),
                  new(
                     new(new Vector3Int(0, 0, 0), Direction2D.Up),
                     new(new Vector3Int(0, 0, 0), Direction2D.Left), 
                     new[] { Vector3.zero }),
                  new(
                     new(new Vector3Int(0, 0, 0), Direction2D.Up),
                     new(new Vector3Int(0, 0, 0), Direction2D.Down), 
                     new[] { Vector3.zero }),
                  // Down
                  new(
                     new(new Vector3Int(0, 0, 0), Direction2D.Down),
                     new(new Vector3Int(0, 0, 0), Direction2D.Right), 
                     new[] { Vector3.zero }),
                  new(
                     new(new Vector3Int(0, 0, 0), Direction2D.Down),
                     new(new Vector3Int(0, 0, 0), Direction2D.Left), 
                     new[] { Vector3.zero }),
                  new(
                     new(new Vector3Int(0, 0, 0), Direction2D.Down),
                     new(new Vector3Int(0, 0, 0), Direction2D.Up), 
                     new[] { Vector3.zero }),
                  // Right
                  new(
                     new(new Vector3Int(0, 0, 0), Direction2D.Right),
                     new(new Vector3Int(0, 0, 0), Direction2D.Up), 
                     new[] { Vector3.zero }),
                  new(
                     new(new Vector3Int(0, 0, 0), Direction2D.Right),
                     new(new Vector3Int(0, 0, 0), Direction2D.Left), 
                     new[] { Vector3.zero }),
                  new(
                     new(new Vector3Int(0, 0, 0), Direction2D.Right),
                     new(new Vector3Int(0, 0, 0), Direction2D.Down), 
                     new[] { Vector3.zero }),
                  // Left
                  new(
                     new(new Vector3Int(0, 0, 0), Direction2D.Left),
                     new(new Vector3Int(0, 0, 0), Direction2D.Up), 
                     new[] { Vector3.zero }),
                  new(
                     new(new Vector3Int(0, 0, 0), Direction2D.Left),
                     new(new Vector3Int(0, 0, 0), Direction2D.Right), 
                     new[] { Vector3.zero }),
                  new(
                     new(new Vector3Int(0, 0, 0), Direction2D.Left),
                     new(new Vector3Int(0, 0, 0), Direction2D.Down), 
                     new[] { Vector3.zero }),
               };
            case "TrackXIntersection3x3":
               return new TrackRoute[]
               {
                  // Up
                  new(
                     new(new Vector3Int(1, 0, 0), Direction2D.Up), 
                     new(new Vector3Int(0, 1, 0), Direction2D.Right), 
                     new Vector3[]
                     {
                        new(0, 0, -1.5f),
                        new(-0.1f, 0, -0.85f),
                        new(-0.4f, 0, -0.4f),
                        new(-0.85f, 0, -0.1f),
                        new(-1.5f, 0, 0f),
                     }),
                  new(
                     new(new Vector3Int(1, 0, 0), Direction2D.Up),
                     new(new Vector3Int(1, 2, 0), Direction2D.Down), 
                     new Vector3[] { new(0, 0, 0), }),
                  new(
                     new(new Vector3Int(1, 0, 0), Direction2D.Up), 
                     new(new Vector3Int(2, 1, 0), Direction2D.Left), 
                     new Vector3[]
                     {
                        new(0, 0, -1.5f),
                        new(0.1f, 0, -0.85f),
                        new(0.4f, 0, -0.4f),
                        new(0.85f, 0, -0.1f),
                        new(1.5f, 0, 0f),
                     }),
                  // Down
                  new(
                     new(new Vector3Int(1, 2, 0), Direction2D.Down), 
                     new(new Vector3Int(0, 1, 0), Direction2D.Right), 
                     new Vector3[]
                     {
                        new(0, 0, 1.5f),
                        new(-0.1f, 0, 0.85f),
                        new(-0.4f, 0, 0.4f),
                        new(-0.85f, 0, 0.1f),
                        new(-1.5f, 0, 0f),
                     }),
                  new(
                     new(new Vector3Int(1, 2, 0), Direction2D.Down),
                     new(new Vector3Int(1, 0, 0), Direction2D.Up), 
                     new Vector3[] { new(0, 0, 0), }),
                  new(
                     new(new Vector3Int(1, 2, 0), Direction2D.Down), 
                     new(new Vector3Int(2, 1, 0), Direction2D.Left), 
                     new Vector3[]
                     {
                        new(0, 0, 1.5f),
                        new(0.1f, 0, 0.85f),
                        new(0.4f, 0, 0.4f),
                        new(0.85f, 0, 0.1f),
                        new(1.5f, 0, 0f),
                     }),
                  // Right
                  new(
                     new(new Vector3Int(0, 1, 0), Direction2D.Right), 
                     new(new Vector3Int(1, 0, 0), Direction2D.Up), 
                     new Vector3[]
                     {
                        new(-1.5f, 0, 0f),
                        new(-0.85f, 0, -0.1f),
                        new(-0.4f, 0, -0.4f),
                        new(-0.1f, 0, -0.85f),
                        new(0, 0, -1.5f),
                     }),
                  new(
                     new(new Vector3Int(0, 1, 0), Direction2D.Right),
                     new(new Vector3Int(2, 1, 0), Direction2D.Left), 
                     new Vector3[] { new(0, 0, 0), }),
                  new(
                     new(new Vector3Int(0, 1, 0), Direction2D.Right), 
                     new(new Vector3Int(1, 2, 0), Direction2D.Down), 
                     new Vector3[]
                     {
                        new(-1.5f, 0, 0f),
                        new(-0.85f, 0, 0.1f),
                        new(-0.4f, 0, 0.4f),
                        new(-0.1f, 0, 0.85f),
                        new(0, 0, 1.5f),
                     }),
                  // Left
                  new(
                     new(new Vector3Int(2, 1, 0), Direction2D.Left), 
                     new(new Vector3Int(1, 0, 0), Direction2D.Up), 
                     new Vector3[]
                     {
                        new(1.5f, 0, 0f),
                        new(0.85f, 0, -0.1f),
                        new(0.4f, 0, -0.4f),
                        new(0.1f, 0, -0.85f),
                        new(0, 0, -1.5f),
                     }),
                  new(
                     new(new Vector3Int(2, 1, 0), Direction2D.Left), 
                     new(new Vector3Int(0, 1, 0), Direction2D.Right),
                     new Vector3[] { new(0, 0, 0), }),
                  new(
                     new(new Vector3Int(2, 1, 0), Direction2D.Left), 
                     new(new Vector3Int(1, 2, 0), Direction2D.Down), 
                     new Vector3[]
                     {
                        new(1.5f, 0, 0f),
                        new(0.85f, 0, 0.1f),
                        new(0.4f, 0, 0.4f),
                        new(0.1f, 0, 0.85f),
                        new(0, 0, 1.5f),
                     }),
               };
            case "TrackRIntersectionLeft2x3":
               return new TrackRoute[]
               {
                  new(
                     new(new Vector3Int(1, 0, 0), Direction2D.Up), 
                     new(new Vector3Int(0, 2, 0), Direction2D.Down), 
                     new Vector3[]
                     {
                        new(0.5f, 0, -1.5f),
                        new(0.4f, 0, -0.8f),
                        new(-0.1f,0,0),
                        new(-0.3f, 0, 0.2f),
                        new(-0.4f, 0, 0.5f),
                        new(-0.5f, 0, 0.8f),
                     }),
                  new(
                     new(new Vector3Int(0, 2, 0), Direction2D.Down),
                     new(new Vector3Int(1, 0, 0), Direction2D.Up),
                     new Vector3[]
                     {
                        new(-0.4f, 0, 0.5f),
                        new(-0.3f, 0, 0.2f),
                        new(-0.1f,0,0),
                        new(0.4f, 0, -0.8f),
                        new(0.5f, 0, -1.5f),
                     }),
                  new(
                     new(new Vector3Int(0, 0, 0), Direction2D.Up),
                     new(new Vector3Int(0, 2, 0), Direction2D.Down), 
                     new Vector3[] { new(-0.5f, 0, 0), }),
                  new(
                     new(new Vector3Int(0, 2, 0), Direction2D.Down),
                     new(new Vector3Int(0, 0, 0), Direction2D.Up), 
                     new Vector3[] { new(-0.5f, 0, 0), }),
               };
            case "TrackRIntersectionRight2x3":
               return new TrackRoute[]
               {
                  new(
                     new(new Vector3Int(0, 0, 0), Direction2D.Up), 
                     new(new Vector3Int(1, 2, 0), Direction2D.Down), 
                     new Vector3[]
                     {
                        new(-0.5f, 0, -1.5f),
                        new(-0.4f, 0, -0.8f),
                        new(0.1f,0,0),
                        new(0.3f, 0, 0.2f),
                        new(0.4f, 0, 0.5f),
                        new(0.5f, 0, 0.8f),
                     }),
                  new(
                     new(new Vector3Int(1, 2, 0), Direction2D.Down),
                     new(new Vector3Int(0, 0, 0), Direction2D.Up),
                     new Vector3[]
                     {
                        new(0.4f, 0, 0.5f),
                        new(0.3f, 0, 0.2f),
                        new(0.1f,0,0),
                        new(-0.4f, 0, -0.8f),
                        new(-0.5f, 0, -1.5f),
                     }),
                  new(
                     new(new Vector3Int(1, 0, 0), Direction2D.Up),
                     new(new Vector3Int(1, 2, 0), Direction2D.Down), 
                     new Vector3[] { new(0.5f, 0, 0), }),
                  new(
                     new(new Vector3Int(1, 2, 0), Direction2D.Down),
                     new(new Vector3Int(1, 0, 0), Direction2D.Up), 
                     new Vector3[] { new(0.5f, 0, 0), }),
               };
            case "TrainYard":
               return new TrackRoute[]
               {
                  new(
                     new TrackConnection(new Vector3Int(0, 2, 0), Direction2D.Down), 
                     new TrackConnection(new Vector3Int(0, 0, 0), Direction2D.Up), 
                     new Vector3[] { new(-0.5f, 0, -1.1f) }),
                  new(
                     new TrackConnection(new Vector3Int(0, 0, 0), Direction2D.Up),
                     new TrackConnection(new Vector3Int(0, 2, 0), Direction2D.Down), 
                     new Vector3[] { new(-0.5f, 0, 1.1f) })
               };
            case "TrainStation":
               return new TrackRoute[]
               {
                  new(
                     new TrackConnection(new Vector3Int(2, 0, 0), Direction2D.Left),
                     new TrackConnection(new Vector3Int(0, 0, 0), Direction2D.Right),
                     new Vector3[]
                     {
                        new(1.2f, 0, -1f),
                        new(-1.2f, 0, -1f)
                     }),
                  new(new TrackConnection(new Vector3Int(0, 0, 0), Direction2D.Right),
                     new TrackConnection(new Vector3Int(2, 0, 0), Direction2D.Left),
                     new Vector3[]
                     {
                        new(-1.2f, 0, -1f),
                        new(1.2f, 0, -1f)
                     })
               };
            case "GoodsStation":
               return new TrackRoute[]
               {
                  new(
                     new TrackConnection(new Vector3Int(1, 3, 0), Direction2D.Down), 
                     new TrackConnection(new Vector3Int(1, 0, 0), Direction2D.Up), 
                     new Vector3[]
                     {
                        new(0.5f, 0, 1.2f),
                        new(0.5f, 0, -1.9f)
                     }),
                  new(
                     new TrackConnection(new Vector3Int(1, 0, 0), Direction2D.Up),
                     new TrackConnection(new Vector3Int(1, 3, 0), Direction2D.Down), 
                     new Vector3[]
                     {
                        new(0.5f, 0, -1.2f),
                        new(0.5f, 0, 1.9f)
                     }),
               };
            case "PassengerStation":
               return new TrackRoute[]
               {
                  new(
                     new TrackConnection(new Vector3Int(1, 3, 0), Direction2D.Down), 
                     new TrackConnection(new Vector3Int(1, 0, 0), Direction2D.Up), 
                     new Vector3[]
                     {
                        new(0.5f, 0, 1.2f),
                        new(0.5f, 0, -1.9f)
                     }),
                  new(
                     new TrackConnection(new Vector3Int(1, 0, 0), Direction2D.Up),
                     new TrackConnection(new Vector3Int(1, 3, 0), Direction2D.Down), 
                     new Vector3[]
                     {
                        new(0.5f, 0, -1.2f),
                        new(0.5f, 0, 1.9f)
                     }),
               };
            case "WaitingStation":
               return new TrackRoute[]
               {
                  new(
                     new(new Vector3Int(0, 0, 0), Direction2D.Up),
                     new(new Vector3Int(0, 3, 0), Direction2D.Down),
                     new Vector3[]
                     {
                        new(0, 0, -2f),
                        new(0, 0, 1.7f),
                     })
               };
            default:
               throw new ArgumentOutOfRangeException($"Unexpected Track object: {prefabName}");
         }
      }

      private string FixPrefabName(string prefabName)
      {
         return prefabName
            .Replace("(Clone)", "")
            .Replace(".Folktails", "")
            .Replace(".IronTeeth", "")
            .Replace("Preview", "");
      }
   }
}
