using System.Collections.Generic;
using Timberborn.Localization;
using Timberborn.WindSystem;

namespace DifficultySettingsChanger
{
    public class WindSettingsGenerator : IGameValueChangerGenerator
    {
        private readonly WindService _windService;
        private readonly ILoc _loc;
        
        WindSettingsGenerator(WindService windService, ILoc loc)
        {
            _windService = windService;
            _loc = loc;
        }
        
        public IEnumerable<GameValueChanger> Generate()
        {
            return new List<GameValueChanger>
            {
                // new SaveableGameValueChanger(
                //     new FieldRef(
                //         () => InaccessibilityUtilities.GetInaccessibleField(_windService, "MinWindTimeInHours"),
                //         value => InaccessibilityUtilities.SetInaccessibleField(_windService, "MinWindTimeInHours", value)
                //         ),
                //     nameof(WindService),
                //     "MinWindTimeInHours",
                //     _loc.T("Property"),
                //     true
                //     ),
                //
                // new SaveableGameValueChanger(
                //     new FieldRef(
                //         () => InaccessibilityUtilities.GetInaccessibleField(_windService, "MaxWindTimeInHours"),
                //         value => InaccessibilityUtilities.SetInaccessibleField(_windService, "MaxWindTimeInHours", value)
                //     ),
                //     nameof(WindService),
                //     "MaxWindTimeInHours",
                //     _loc.T("Property"),
                //     true
                // ),
                //
                // new SaveableGameValueChanger(
                //     new FieldRef(
                //         () => InaccessibilityUtilities.GetInaccessibleField(_windService, "MinWindStrength"),
                //         value => InaccessibilityUtilities.SetInaccessibleField(_windService, "MinWindStrength", value)
                //     ),
                //     nameof(WindService),
                //     "MinWindStrength",
                //     _loc.T("Property"),
                //     true
                // ),
                //
                // new SaveableGameValueChanger(
                //     new FieldRef(
                //         () => InaccessibilityUtilities.GetInaccessibleField(_windService, "MaxWindStrength"),
                //         value => InaccessibilityUtilities.SetInaccessibleField(_windService, "MaxWindStrength", value)
                //     ),
                //     nameof(WindService),
                //     "MaxWindStrength",
                //     _loc.T("Property"),
                //     true
                // ),
            };
        }
    }
}