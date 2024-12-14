// using System;
// using System.Collections.Immutable;
// using Timberborn.BlockSystem;
// using Timberborn.Coordinates;
// using UnityEditor;
// using UnityEditor.SceneManagement;
// using UnityEngine;
// using UnityEngine.Rendering;
//
// namespace ChooChoo.Editor
// {
//     public static class CornerRenderer
//     {
//         private static readonly float CubeSize = 0.4f;
//         private static readonly float BottomOffset = 0.2f;
//         private static readonly Vector3 CubeOffset = new Vector3(0.5f, 0.5f, 0.5f);
//         private static readonly Vector3 StackableSquareOffset = new Vector3(0.5f, 1f, 0.5f);
//         private static readonly Vector3 StackableSquareSize = new Vector3(0.8f, 0.0f, 0.8f);
//         private static readonly Vector3 OccupationVector = new Vector3(0.0f, -0.75f, 0.0f);
//
//         public static void Draw(
//             BlockObject blockObject,
//             float minLayer,
//             float maxLayer,
//             bool hideUnoccupied)
//         {
//             PositionedBlocks positionedBlocks = blockObject.PositionedBlocks;
//             ImmutableArray<Block> immutableArray = positionedBlocks != null ? positionedBlocks.GetAllBlocks() : blockObject.Blocks.GetAllBlocks();
//             Vector3 baseZVector = blockObject.Positioned ? Vector3.zero : new Vector3(0.0f, (float)blockObject.BaseZ, 0.0f);
//             double num1 = (double)minLayer;
//             Vector3Int coordinates = blockObject.Coordinates;
//             double z1 = (double)coordinates.z;
//             minLayer = (float)(num1 + z1);
//             double num2 = (double)maxLayer;
//             coordinates = blockObject.Coordinates;
//             double z2 = (double)coordinates.z;
//             maxLayer = (float)(num2 + z2);
//             CompareFunction zTest = Handles.zTest;
//             Handles.zTest = CompareFunction.LessEqual;
//             foreach (Block block in immutableArray)
//             {
//                 if (IsRenderable(block, minLayer, maxLayer, hideUnoccupied))
//                     RenderBlock(block, baseZVector);
//             }
//
//             if ((bool)(UnityEngine.Object)PrefabStageUtility.GetCurrentPrefabStage() && blockObject.Entrance.HasEntrance)
//                 DrawEntrance(blockObject);
//             Handles.zTest = zTest;
//         }
//
//         private static bool IsRenderable(
//             Block block,
//             float minLayer,
//             float maxLayer,
//             bool hideUnoccupied)
//         {
//             return (double)block.Coordinates.z >= (double)minLayer && (double)block.Coordinates.z <= (double)maxLayer && (block.IsOccupied || !hideUnoccupied);
//         }
//
//         private static void RenderBlock(Block block, Vector3 baseZVector)
//         {
//             Vector3 vector3 = CoordinateSystem.GridToWorld(block.Coordinates) - baseZVector;
//             Vector3 position = vector3 + CubeOffset;
//             DrawFloor(position, block.Occupation.HasFlag((Enum)BlockOccupations.Floor));
//             DrawBottom(position, block.Occupation.HasFlag((Enum)BlockOccupations.Bottom));
//             DrawCorners(position, block.Occupation.HasFlag((Enum)BlockOccupations.Corners));
//             DrawTop(position, block.Occupation.HasFlag((Enum)BlockOccupations.Top));
//             if (block.Stackable.IsStackable())
//             {
//                 Vector3 center = vector3 + StackableSquareOffset;
//                 Handles.color = Color.green;
//                 Handles.DrawWireCube(center, StackableSquareSize);
//             }
//
//             if (!block.OccupyAllBelow)
//                 return;
//             Handles.color = Color.red;
//             Handles.DrawLine(vector3 + CubeOffset, vector3 + CubeOffset + OccupationVector);
//         }
//
//         private static void DrawFloor(Vector3 position, bool isOccupied)
//         {
//             Handles.color = isOccupied ? Color.yellow : Color.white;
//             Handles.DrawPolyLine(position + new Vector3(-CubeSize, -CubeSize, -CubeSize), position + new Vector3(CubeSize, -CubeSize, -CubeSize), position + new Vector3(CubeSize, -CubeSize, CubeSize), position + new Vector3(-CubeSize, -CubeSize, CubeSize), position + new Vector3(-CubeSize, -CubeSize, -CubeSize));
//         }
//
//         private static void DrawBottom(Vector3 position, bool isOccupied)
//         {
//             Handles.color = isOccupied ? Color.yellow : Color.white;
//             Handles.DrawLine(position + new Vector3(-CubeSize, -CubeSize, -CubeSize), position + new Vector3(-CubeSize, -CubeSize + BottomOffset, -CubeSize));
//             Handles.DrawLine(position + new Vector3(-CubeSize, -CubeSize, CubeSize), position + new Vector3(-CubeSize, -CubeSize + BottomOffset, CubeSize));
//             Handles.DrawLine(position + new Vector3(CubeSize, -CubeSize, -CubeSize), position + new Vector3(CubeSize, -CubeSize + BottomOffset, -CubeSize));
//             Handles.DrawLine(position + new Vector3(CubeSize, -CubeSize, CubeSize), position + new Vector3(CubeSize, -CubeSize + BottomOffset, CubeSize));
//         }
//
//         private static void DrawCorners(Vector3 position, bool isOccupied)
//         {
//             Handles.color = isOccupied ? Color.yellow : Color.white;
//             Handles.DrawLine(position + new Vector3(-CubeSize, -CubeSize + BottomOffset, -CubeSize), position + new Vector3(-CubeSize, CubeSize, -CubeSize));
//             Handles.DrawLine(position + new Vector3(-CubeSize, -CubeSize + BottomOffset, CubeSize), position + new Vector3(-CubeSize, CubeSize, CubeSize));
//             Handles.DrawLine(position + new Vector3(CubeSize, -CubeSize + BottomOffset, -CubeSize), position + new Vector3(CubeSize, CubeSize, -CubeSize));
//             Handles.DrawLine(position + new Vector3(CubeSize, -CubeSize + BottomOffset, CubeSize), position + new Vector3(CubeSize, CubeSize, CubeSize));
//         }
//
//         private static void DrawTop(Vector3 position, bool isOccupied)
//         {
//             Handles.color = isOccupied ? Color.yellow : Color.white;
//             Handles.DrawPolyLine(position + new Vector3(-CubeSize, CubeSize, -CubeSize), position + new Vector3(CubeSize, CubeSize, -CubeSize), position + new Vector3(CubeSize, CubeSize, CubeSize), position + new Vector3(-CubeSize, CubeSize, CubeSize), position + new Vector3(-CubeSize, CubeSize, -CubeSize));
//         }
//
//         private static void DrawEntrance(BlockObject blockObject)
//         {
//             Vector3 worldCentered = CoordinateSystem.GridToWorldCentered(blockObject.Entrance.Coordinates - new Vector3Int(0, 0, blockObject.BaseZ));
//             Handles.color = new Color(1f, 0.5f, 0.0f);
//             Handles.ArrowHandleCap(0, worldCentered + Vector3.forward * 0.4f, Quaternion.LookRotation(Vector3.back), 0.7f, EventType.Repaint);
//             Handles.DrawWireCube(worldCentered, new Vector3(0.95f, 0.0f, 0.95f));
//         }
//     }
// }