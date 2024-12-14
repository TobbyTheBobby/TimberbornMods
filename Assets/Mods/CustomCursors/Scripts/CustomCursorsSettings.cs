using System.Linq;
using ModSettings.Common;
using ModSettings.Core;
using Timberborn.Modding;
using Timberborn.SettingsSystem;

namespace CustomCursors
{
    public class CustomCursorsSettings : ModSettingsOwner
    {
        private static readonly string SelectedCustomCursorKey = "Tobbert.CustomCursors.SelectedCustomCursor";

        private readonly CursorPackRepository _cursorPackRepository;

        private ModSetting<string> _customCursorModSetting;
        
        public ModSetting<string> CustomCursorModSetting
        {
            get
            {
                return _customCursorModSetting ??= new LimitedStringModSetting(
                    0, 
                    _cursorPackRepository.CursorPacks.Select(pack => new LimitedStringModSettingValue(pack.PackName, pack.PackName)).ToArray(), 
                    ModSettingDescriptor.CreateLocalized(SelectedCustomCursorKey));
            }
        }

        public CustomCursorsSettings(ISettings settings, ModSettingsOwnerRegistry modSettingsOwnerRegistry, ModRepository modRepository, CursorPackRepository cursorPackRepository) : 
            base(settings, modSettingsOwnerRegistry, modRepository)
        {
            _cursorPackRepository = cursorPackRepository;
        }

        public override string HeaderLocKey => "Tobbert.CustomCursors.SettingsHeader";

        protected override string ModId => "Tobbert.CustomCursors";
    }
}