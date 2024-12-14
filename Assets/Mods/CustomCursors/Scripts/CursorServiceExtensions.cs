using Timberborn.Common;
using Timberborn.InputSystem;

namespace CustomCursors
{
    public static class CursorServiceExtensions
    {
        public static CustomCursor GetCursorForcedLoad(this CursorService cursorService, string cursorName)
        {
            return cursorService._cursors.GetOrAdd(cursorName, () => cursorService.GetCursor(cursorName));
        }
    }
}