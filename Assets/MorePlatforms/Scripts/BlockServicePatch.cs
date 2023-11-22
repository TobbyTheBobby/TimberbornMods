using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Timberborn.BlockSystem;
using Timberborn.Common;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace MorePlatforms
{
    [HarmonyPatch]
    public class BlockServicePatch1
    {
        static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("BlockService"), "AnyObjectAt", new []
            {
                typeof(Vector3Int)
            });
        }
        
        static bool Prefix(Vector3Int coordinates, Array3D<WorldBlock> ____blocks, ref bool __result)
        {
            __result = ____blocks.GetCopyAtOrDefault(coordinates).BlockOccupations != 0 || ____blocks.GetCopyAtOrDefault(coordinates).BlockObjects.Any(obj => obj.TryGetComponentFast(out OverhangingBuilding _));

            return false;
        }
    }
    
    [HarmonyPatch]
    public class BlockServicePatch2
    {
        static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("BlockService"), "SetObject", new []
            {
                typeof(BlockObject)
            });
        }
        
        static bool Prefix(BlockService __instance, BlockObject blockObject, EventBus ____eventBus, Array3D<WorldBlock> ____blocks)
        {
            if (blockObject.TryGetComponentFast(out OverhangingBuilding _overhangingBuilding))
            {
                var allBlocks = blockObject.PositionedBlocks.GetAllBlocks();

                var occupiedBlocks = new List<Block>
                {
                    allBlocks[_overhangingBuilding.BlockObjectBlockIndex - 1], 
                    allBlocks[_overhangingBuilding.BlockObjectBlockIndex]
                };
                
                foreach (Block occupiedBlock in occupiedBlocks)
                {
                    if (__instance.Contains(occupiedBlock.Coordinates))
                        ____blocks.GetRefAt(occupiedBlock.Coordinates).SetBlockObject(blockObject, occupiedBlock.Occupation);
                }
                if (blockObject.HasEntrance)
                {
                    PositionedEntrance positionedEntrance = blockObject.PositionedEntrance;
                    if (____blocks.Contains(positionedEntrance.Coordinates))
                        ____blocks.GetRefAt(positionedEntrance.Coordinates).AddEntrance(positionedEntrance.Direction2D);
                }
                ____eventBus.Post(new BlockObjectSetEvent(blockObject));

                return false;
            }
            
            return true;
        }
    }
    
    [HarmonyPatch]
    public class BlockServicePatch3
    {
        static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("BlockService"), "UnsetObject", new []
            {
                typeof(BlockObject)
            });
        }
        
        static bool Prefix(BlockService __instance, BlockObject blockObject, EventBus ____eventBus, Array3D<WorldBlock> ____blocks)
        {
            if (blockObject.TryGetComponentFast(out OverhangingBuilding _overhangingBuilding))
            {
                var allBlocks = blockObject.PositionedBlocks.GetAllBlocks();

                var occupiedBlocks = new List<Block>
                {
                    allBlocks[_overhangingBuilding.BlockObjectBlockIndex - 1], 
                    allBlocks[_overhangingBuilding.BlockObjectBlockIndex]
                };
                
                foreach (Block occupiedBlock in occupiedBlocks)
                {
                    if (__instance.Contains(occupiedBlock.Coordinates))
                        ____blocks.GetRefAt(occupiedBlock.Coordinates).UnsetBlockObject(blockObject, occupiedBlock.Occupation);
                }
                if (blockObject.HasEntrance)
                {
                    PositionedEntrance positionedEntrance = blockObject.PositionedEntrance;
                    if (____blocks.Contains(positionedEntrance.Coordinates))
                        ____blocks.GetRefAt(positionedEntrance.Coordinates).DeleteEntrance(positionedEntrance.Direction2D);
                }
                ____eventBus.Post(new BlockObjectUnsetEvent(blockObject));

                return false;
            }
            
            return true;
        }
    }
}