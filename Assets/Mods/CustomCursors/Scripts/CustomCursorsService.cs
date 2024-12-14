using System.Linq;
using Timberborn.CameraSystem;
using Timberborn.InputSystem;
using Timberborn.SingletonSystem;

namespace CustomCursors
{
    public class CustomCursorsService : ILoadableSingleton
    {

        private readonly CustomCursorsSettings _customCursorsSettings;
        private readonly CursorPackRepository _cursorPackRepository;
        private readonly CursorService _cursorService;

        private CustomCursorsService(CustomCursorsSettings customCursorsSettings, CursorPackRepository cursorPackRepository, CursorService cursorService)
        {
            _customCursorsSettings = customCursorsSettings;
            _cursorPackRepository = cursorPackRepository;
            _cursorService = cursorService;
        }

        private string CurrentCursorPackName => _customCursorsSettings.CustomCursorModSetting.Value;

        private CursorPack _cursorPack;

        public void Load()
        {
            _customCursorsSettings.CustomCursorModSetting.ValueChanged += OnCustomCursorSettingChanged;
            UpdateCursor(CurrentCursorPackName);
        }

        private void OnCustomCursorSettingChanged(object sender, string e)
        {
            UpdateCursor(CurrentCursorPackName);
        }

        private void UpdateCursor(string newCursorPack)
        {
            _cursorPack = _cursorPackRepository.CursorPacks.FirstOrDefault(pack => pack.PackName == newCursorPack);
            if (_cursorPack == null)
                return;
            if (_cursorPack.TryGetSelectionCursor(out var selectionCursor))
                OverwriteCustomCursor(_cursorService.GetCursorForcedLoad(CursorService.DefaultCursorName), selectionCursor);
            if (_cursorPack.TryGetGrabbingCursor(out var grabbingCursor))
                OverwriteCustomCursor(_cursorService.GetCursorForcedLoad(GrabbingCameraTargetPicker.CursorKey), grabbingCursor);
            _cursorService.ResetTemporaryCursor();
        }

        private void OverwriteCustomCursor(CustomCursor oldCursor, CustomCursor newCursor)
        {
            oldCursor._smallCursor = newCursor.SmallCursor;
            oldCursor._largeCursor = newCursor.LargeCursor;
            // oldCursor._hotspot = newCursor.Hotspot;
            // oldCursor._smallCursorOffset = newCursor.SmallCursorOffset;
            // oldCursor._largeCursorOffset = newCursor.LargeCursorOffset;
        }
    }
}