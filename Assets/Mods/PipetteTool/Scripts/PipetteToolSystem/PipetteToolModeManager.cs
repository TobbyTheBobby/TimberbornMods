using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using PipetteTool.SettingsSystem;
using Timberborn.Localization;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace PipetteTool.PipetteToolSystem
{
    public class PipetteToolModeManager : ILoadableSingleton
    {
        private readonly PipetteToolSettingsOwner _pipetteToolSettingsOwner;
        private readonly ILoc _loc;

        public PipetteToolModeManager(IEnumerable<IPipetteToolMode> pipetteToolMods, PipetteToolSettingsOwner pipetteToolSettingsOwner, ILoc loc)
        {
            PipetteToolModes = pipetteToolMods.ToList().OrderBy(mode => mode.SortOrder);
            _pipetteToolSettingsOwner = pipetteToolSettingsOwner;
            _loc = loc;
        }

        private IEnumerable<IPipetteToolMode> PipetteToolModes { get; }

        private IPipetteToolMode ActivePipetteToolMode { get; set; }

        public void Load()
        {
            _pipetteToolSettingsOwner.PipetteToolModeModSetting.ValueChanged += (_, value) => SwitchToMode(value);
            SwitchToMode(_pipetteToolSettingsOwner.PipetteToolModeModSetting.Value);
        }

        private void SwitchToMode(string modeLocKey)
        {
            Debug.Log($"Switching Pipette Tool Mode to: {_loc.T(modeLocKey)}");
            if (ActivePipetteToolMode != null)
            {
                ActivePipetteToolMode.ExitMode();
                ActivePipetteToolMode = null;
            }

            foreach (var pipetteToolMode in PipetteToolModes)
            {
                if (pipetteToolMode.LabelLocKey == modeLocKey)
                {
                    pipetteToolMode.EnterMode();
                    ActivePipetteToolMode = pipetteToolMode;
                }
            }

            if (ActivePipetteToolMode == null)
                throw new NoPipetteToolModeSelectedError();
        }
    }
}