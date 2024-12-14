using System.Linq;
using Timberborn.CameraSystem;
using Timberborn.InputSystem;
using Timberborn.SingletonSystem;

namespace CustomCursors
{
    public class BaseGameCursorAdder : ILoadableSingleton
    {
        public static string BaseGameCursorPackName = "BaseGame";
        
        private readonly CursorPackRepository _cursorPackRepository;
        private readonly CursorService _cursorService;

        public BaseGameCursorAdder(CursorPackRepository cursorPackRepository, CursorService cursorService)
        {
            _cursorPackRepository = cursorPackRepository;
            _cursorService = cursorService;
        }


        public void Load()
        {
            if (_cursorPackRepository.CursorPacks.Any(pack => pack.PackName == BaseGameCursorPackName))
                return;
            
            var defaultCursor = _cursorService.GetCursorForcedLoad(CursorService.DefaultCursorName);
            var grabbingCursor = _cursorService.GetCursorForcedLoad(GrabbingCameraTargetPicker.CursorKey);
            
            _cursorPackRepository.CursorPacks.Insert(
                0, 
                new CursorPack(
                    BaseGameCursorPackName, 
                    CursorPackRepository.CreateCustomCursor(null, true), 
                    CursorPackRepository.CreateCustomCursor(grabbingCursor.SmallCursor, grabbingCursor.LargeCursor, true)));
        }
    }
}