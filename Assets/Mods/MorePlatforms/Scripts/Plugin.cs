using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using HarmonyLib;
using JetBrains.Annotations;
using TimberApi.DependencyContainerSystem;
using Timberborn.BlockObjectAccesses;
using Timberborn.BlockObjectTools;
using Timberborn.BlockSystem;
using Timberborn.ModManagerScene;
using Timberborn.ToolSystem;
using UnityEngine;
using UnityEngine.UIElements;

#pragma warning disable CS0618
[assembly: SecurityPermission(action: SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace MorePlatforms
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class Plugin : IModStarter
    {
        public const string PluginGuid = "tobbert.moreplatforms";
        
        public void StartMod(IModEnvironment modEnvironment)
        {
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
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
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
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    internal class ParentedNeighborCalculatorPatch
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