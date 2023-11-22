using System.Linq;
using Timberborn.BlockSystem;
using UnityEngine;

namespace ChooChoo
{
    public static class BlockObjectExtensions
    {
        public static Block GetPositionedBlock(this BlockObject blockObject, Vector3Int coordinates)
        {
            var index = blockObject.Blocks.GetAllBlocks().ToList().FindIndex(block => block.Coordinates == coordinates);

            return blockObject.PositionedBlocks.GetAllBlocks()[index];
        }
    }
}