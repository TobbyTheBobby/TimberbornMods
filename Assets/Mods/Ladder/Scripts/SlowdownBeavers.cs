// using System.Collections.Generic;
// using Timberborn.Coordinates;
// using Timberborn.EnterableSystem;
// using Timberborn.RangedEffectSystem;
// using Timberborn.TimeSystem;
// using UnityEngine;
//
// namespace Ladder
// {
//     public class SlowdownBeavers : MonoBehaviour
//     {
//         private RangedEffectService _rangedEffectService;
//         private IDayNightCycle _dayNightCycle;
//         private Enterer _enterer;
//         
//         void SlowBeaverDown(GameObject Beaver)
//         {
//             Plugin.Log.LogInfo(Beaver.name);
//             // IReadOnlyList<RangedEffect> affectingEffects = this.GetAffectingEffects();
//             // float deltaTimeInHours1 = this._dayNightCycle.FixedDeltaTimeInHours;
//             // foreach (RangedEffect rangedEffect in (IEnumerable<RangedEffect>) affectingEffects)
//             // {
//             //     // NeedManager needManager = this._needManager;
//             //     // ContinuousEffect continuousEffect = rangedEffect.ToContinuousEffect();
//             //     // ref ContinuousEffect local = ref continuousEffect;
//             //     // double deltaTimeInHours2 = (double) deltaTimeInHours1;
//             //     // needManager.ApplyEffect(in local, (float) deltaTimeInHours2);
//             // }
//         }
//         
//         // private IReadOnlyList<RangedEffect> GetAffectingEffects() => this._enterer.IsInside ? this._enterer.CurrentBuilding.GetComponent<RangedEffectsAffectingEnterable>().Effects : this._rangedEffectService.GetEffectsAffectingCoordinates(CoordinateSystem.WorldToGridInt(this.transform.position).XY());
//
//         
//     }
// }
