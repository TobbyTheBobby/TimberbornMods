using System.Collections.Generic;
using System.Collections.Immutable;
using Timberborn.BlockSystem;
using Timberborn.Coordinates;
using UnityEngine;

namespace TobbyTools.Extensions
{
    public static class BlockCalculations
    {
        public static void PositionBlocks(
            Blocks blocks,
            IList<Block> positionedBlocks,
            Vector3Int coordinates,
            Orientation orientation)
        {
            ImmutableArray<Block> allBlocks = blocks.GetAllBlocks();
            for (int index = 0; index < allBlocks.Length; ++index)
            {
                Block positionedBlock = PositionBlock(allBlocks[index], coordinates, orientation);
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
            int z = -1;
            int num = positionedBlock.Coordinates.z - 1;
            while (num >= 0)
            {
                Vector3Int coordinates = positionedBlock.Coordinates + new Vector3Int(0, 0, z);
                bottomBlocks.Add(Block.FullFrom(coordinates));
                --num;
                --z;
            }
        }
    }
}