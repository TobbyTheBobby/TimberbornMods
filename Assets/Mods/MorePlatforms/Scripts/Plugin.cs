using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TimberApi.ConsoleSystem;
using TimberApi.DependencyContainerSystem;
using TimberApi.ModSystem;
using Timberborn.BlockObjectAccesses;
using Timberborn.BlockObjectTools;
using Timberborn.BlockSystem;
using Timberborn.ToolSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace MorePlatforms
{
    public class Plugin : IModEntrypoint
    {
        public const string PluginGuid = "tobbert.moreplatforms";
        
        public static IConsoleWriter Log;
        
        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            Log = consoleWriter;
            
            new Harmony(PluginGuid).PatchAll();
        }
    }
    
    public class FakeParentedNeighborCalculator
    {
        private readonly NeighborCalculator _neighborCalculator;

        private FakeParentedNeighborCalculator(NeighborCalculator neighborCalculator)
        {
            _neighborCalculator = neighborCalculator;
        }
        
        public IEnumerable<ParentedNeighbor2D> FakeGetParentedNeighbors(Vector3Int fakeCoordinate)
        {
            var fakeCoordinates = new List<Vector3Int> { fakeCoordinate };
            return _neighborCalculator.GetParentedNeighborsWithDiagonal(fakeCoordinates).Select(ParentedNeighbor2D.From3D).Distinct();
        }
    }

    [HarmonyPatch(typeof(BlockObjectToolButtonFactory), "Create", new Type[] {typeof(PlaceableBlockObject), typeof(ToolGroup), typeof(VisualElement)})]
    internal class RemoveMiddlePatch
    {
        private static bool Prefix(
            ref PlaceableBlockObject prefab,
            ref ToolGroup toolGroup,
            ref VisualElement buttonParent)
        {
            var objectList = new List<string>()
            {      
                "HorizontalPlatformMiddle1x1.Folktails", "HorizontalPlatformMiddle1x1.IronTeeth",
                "HorizontalPlatformMiddle1x2.Folktails", "HorizontalPlatformMiddle1x2.IronTeeth",
            };

            if ((prefab.name != "HorizontalPlatformMiddle1x1.Folktails" | prefab.name != "HorizontalPlatformMiddle1x1.IronTeeth" | prefab.name != "HorizontalPlatformMiddle1x2.Folktails" | prefab.name != "HorizontalPlatformMiddle1x2.IronTeeth"))
            {
                if (true)
                {
                    return true;
                }
                else
                {
                    return prefab.name is not ("HorizontalPlatformMiddle1x1.Folktails" or "HorizontalPlatformMiddle1x1.IronTeeth" or "HorizontalPlatformMiddle1x2.Folktails" or "HorizontalPlatformMiddle1x2.IronTeeth");
                }
            }
            else
            {
                return prefab.name is not ("HorizontalPlatformMiddle1x1.Folktails" or "HorizontalPlatformMiddle1x1.IronTeeth" or "HorizontalPlatformMiddle1x2.Folktails" or "HorizontalPlatformMiddle1x2.IronTeeth");
            }
        }
    }
    
    [HarmonyPatch(typeof(ParentedNeighborCalculator), "GetParentedNeighbors")]
    internal class ChangeConstructionSitePatch
    {
        private static void Postfix(IEnumerable<ParentedNeighbor2D> __result, ParentedNeighborCalculator __instance)
        {
            var blockObject = __instance.GetComponentFast<BlockObject>();
            
            var objectList1 = new List<string>()
            {
                "HorizontalPlatformMiddle1x1.Folktails(Clone)", "HorizontalPlatformMiddle1x1.IronTeeth(Clone)",
                "HorizontalPlatformEnd1x1.Folktails(Clone)", "HorizontalPlatformEnd1x1.IronTeeth(Clone)",
                "HorizontalPlatformEnd2x1.Folktails(Clone)", "HorizontalPlatformEnd2x1.IronTeeth(Clone)",
                "HorizontalPlatformEnd3x1.Folktails(Clone)", "HorizontalPlatformEnd3x1.IronTeeth(Clone)",  
            };
            
            var objectList2 = new List<string>()
            {      
                "HorizontalPlatformEnd4x1.Folktails(Clone)", "HorizontalPlatformEnd4x1.IronTeeth(Clone)",
                "HorizontalPlatformMiddle1x2.Folktails(Clone)", "HorizontalPlatformMiddle1x2.IronTeeth(Clone)",
            };

            Vector3Int coordinate;
            
            if (objectList1.Contains(blockObject.name))
            {
                coordinate = blockObject.PositionedBlocks.GetAllCoordinates().ToList()[2];
            }else if (objectList2.Contains(blockObject.name))
            {
                coordinate = blockObject.PositionedBlocks.GetAllCoordinates().ToList()[4];
            }
            else
            {
                return;
            }

            __result = DependencyContainer.GetInstance<FakeParentedNeighborCalculator>().FakeGetParentedNeighbors(coordinate);
        }
    }
}