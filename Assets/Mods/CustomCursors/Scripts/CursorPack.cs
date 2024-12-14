using Timberborn.InputSystem;

namespace CustomCursors
{
    public class CursorPack
    {
        private readonly CustomCursor _selectionCursor;
        private readonly CustomCursor _draggingCursor;

        public CursorPack(string packName, CustomCursor selectionCursor, CustomCursor draggingCursor)
        {
            PackName = packName;
            _selectionCursor = selectionCursor;
            _draggingCursor = draggingCursor;
        }

        public string PackName { get; }

        public bool TryGetSelectionCursor(out CustomCursor selectionCursor)
        {
            if (_selectionCursor == null)
            {
                selectionCursor = null;
                return false;
            }

            selectionCursor = _selectionCursor;
            return true;
        }
        
        public bool TryGetGrabbingCursor(out CustomCursor draggingCursor)
        {
            if (_draggingCursor == null)
            {
                draggingCursor = null;
                return false;
            }

            draggingCursor = _draggingCursor;
            return true;
        }
    }
}