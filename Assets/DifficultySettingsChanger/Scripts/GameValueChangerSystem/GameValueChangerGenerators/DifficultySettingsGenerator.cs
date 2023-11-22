// using System.Collections.Generic;
// using Timberborn.GameFactionSystem;
// using Timberborn.Localization;
// using Timberborn.NeedApplication;
// using Timberborn.RecoverableGoodSystem;
// using Timberborn.WeatherSystem;
//
// namespace DifficultySettingsChanger
// {
//     public class DifficultySettingsGenerator : IGameValueChangerGenerator
//     {
//         private readonly EffectProbabilityService _effectProbabilityService;
//         private readonly GoodRecoveryRateService _goodRecoveryRateService;
//         private readonly NeedModificationService _needModificationService;
//         private readonly WeatherDurationService _weatherDurationService;
//         private readonly ILoc _loc;
//
//         public DifficultySettingsGenerator(
//             EffectProbabilityService effectProbabilityService,
//             GoodRecoveryRateService goodRecoveryRateService,
//             NeedModificationService needModificationService,
//             WeatherDurationService weatherDurationService,
//             ILoc loc)
//         {
//             _effectProbabilityService = effectProbabilityService;
//             _goodRecoveryRateService = goodRecoveryRateService;
//             _needModificationService = needModificationService;
//             _weatherDurationService = weatherDurationService;
//             _loc = loc;
//         }
//         
//         public IEnumerable<GameValueChanger> Generate()
//         {
//             return new[]
//             {
//                 new GameValueChanger(
//                     new FieldRef(
//                         () => _needModificationService._waterConsumption, 
//                         value => _needModificationService._waterConsumption = (float)value),
//                     nameof(NeedModificationService),
//                     "_waterConsumption",
//                     _loc.T("Tobbert.DifficultySetting.WaterConsumption.Label"),
//                     false),
//                 new GameValueChanger(
//                     new FieldRef(
//                         () => _needModificationService._foodConsumption, 
//                         value => _needModificationService._foodConsumption = (float)value),
//                     nameof(NeedModificationService),
//                     "_foodConsumption",
//                     _loc.T("Tobbert.DifficultySetting.FoodConsumption.Label"),
//                     false),
//                 
//                 
//                 new GameValueChanger(
//                     new FieldRef(
//                         () => _weatherDurationService._minTemperateWeatherDuration, 
//                         value => _weatherDurationService._minTemperateWeatherDuration = (int)value),
//                     nameof(WeatherDurationService),
//                     "_minTemperateWeatherDuration",
//                     _loc.T("Tobbert.DifficultySetting.MinTemperateWeatherDuration.Label"),
//                     true),
//                 new GameValueChanger(
//                     new FieldRef(
//                         () => _weatherDurationService._maxTemperateWeatherDuration, 
//                         value => _weatherDurationService._maxTemperateWeatherDuration = (int)value),
//                     nameof(WeatherDurationService),
//                     "_maxTemperateWeatherDuration",
//                     _loc.T("Tobbert.DifficultySetting.MaxTemperateWeatherDuration.Label"),
//                     true),
//                 new GameValueChanger(
//                     new FieldRef(
//                         () => _weatherDurationService._minDroughtDuration, 
//                         value => _weatherDurationService._minDroughtDuration = (int)value),
//                     nameof(WeatherDurationService),
//                     "_minDroughtDuration",
//                     _loc.T("Tobbert.DifficultySetting.MinDroughtDuration.Label"),
//                     true),
//                 new GameValueChanger(
//                     new FieldRef(
//                         () => _weatherDurationService._maxDroughtDuration, 
//                         value => _weatherDurationService._maxDroughtDuration = (int)value),
//                     nameof(WeatherDurationService),
//                     "_maxDroughtDuration",
//                     _loc.T("Tobbert.DifficultySetting.MaxDroughtDuration.Label"),
//                     true),
//                 new GameValueChanger(
//                     new FieldRef(
//                         () => _weatherDurationService._handicapMultiplier, 
//                         value => _weatherDurationService._handicapMultiplier = (float)value),
//                     nameof(WeatherDurationService),
//                     "_handicapMultiplier",
//                     _loc.T("Tobbert.DifficultySetting.DroughtDurationHandicapMultiplier.Label"),
//                     true),
//                 new GameValueChanger(
//                     new FieldRef(
//                         () => _weatherDurationService._handicapCycles, 
//                         value => _weatherDurationService._handicapCycles = (int)value),
//                     nameof(WeatherDurationService),
//                     "_handicapCycles",
//                     _loc.T("Tobbert.DifficultySetting.DroughtDurationHandicapCycles.Label"),
//                     true),
//
//                 new GameValueChanger(
//                     new FieldRef(
//                         () => _effectProbabilityService._injuryChanceModifier, 
//                         value => _effectProbabilityService._injuryChanceModifier = (float)value),
//                     nameof(EffectProbabilityService),
//                     "_injuryChanceModifier",
//                     _loc.T("Tobbert.DifficultySetting.InjuryChanceModifier.Label"),
//                     true),
//                 
//                 new GameValueChanger(
//                     new FieldRef(
//                         () => _goodRecoveryRateService.DemolishableRecoveryRate, 
//                         value => _goodRecoveryRateService.DemolishableRecoveryRate = (float)value),
//                     nameof(GoodRecoveryRateService),
//                     "DemolishableRecoveryRate",
//                     _loc.T("Tobbert.DifficultySetting.DemolishableRecoveryRate.Label"),
//                     true),
//             };
//         }
//     }
// }