using System.Collections.Generic;
using Timberborn.BlockSystem;
using Timberborn.Coordinates;
using UnityEngine;

namespace MorePaths.Core
{
    public static class BlockCalculations
    {
        public static void PositionBlocks(
            Blocks blocks,
            IList<Block> positionedBlocks,
            Vector3Int coordinates,
            Orientation orientation)
        {
            var allBlocks = blocks.GetAllBlocks();
            for (var index = 0; index < allBlocks.Length; ++index)
            {
                var positionedBlock = PositionBlock(allBlocks[index], coordinates, orientation);
                positionedBlocks.Add(positionedBlock);
                GetBottomBlocks(positionedBlock, positionedBlocks);
            }
        }

        public static Vector3 Pivot(Vector3Int coordinates, Orientation orientation)
        {
            return (Vector3)coordinates + orientation.ToPivotOffset();
        }

        public static Vector3Int Transform(
            Vector3Int block,
            Vector3Int coordinates,
            Orientation orientation)
        {
            return orientation.Transform(block) + coordinates;
        }

        public static Vector2Int Transform(
            Vector2Int tile,
            Vector2Int coordinates,
            Orientation orientation)
        {
            return orientation.Transform(tile) + coordinates;
        }

        public static Block PositionBlock(
            Block block,
            Vector3Int coordinates,
            Orientation orientation)
        {
            return Block.From(Transform(block.Coordinates, coordinates, orientation), block);
        }

        private static void GetBottomBlocks(Block positionedBlock, ICollection<Block> bottomBlocks)
        {
            if (!positionedBlock.OccupyAllBelow)
                return;
            var z = -1;
            var num = positionedBlock.Coordinates.z - 1;
            while (num >= 0)
            {
                var coordinates = positionedBlock.Coordinates + new Vector3Int(0, 0, z);
                bottomBlocks.Add(Block.FullFrom(coordinates));
                --num;
                --z;
            }
        }
    }
}