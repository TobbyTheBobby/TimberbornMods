// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Timberborn.BlockSystem;
// using Timberborn.Localization;
// using Timberborn.Navigation;
// using Timberborn.PrefabSystem;
// using Timberborn.PreviewSystem;
// using UnityEngine;
//
// namespace GlobalMarket
// {
//   internal class GlobalMarktPreviewsValidator : IPreviewsValidator
//   {
//     private static readonly string ErrorMessageLocKey = "BuildingTools.DistrictsInConflict";
//     private readonly IDistrictService _districtService;
//     private readonly ILoc _loc;
//
//     public GlobalMarktPreviewsValidator(IDistrictService districtService, ILoc loc)
//     {
//       this._districtService = districtService;
//       this._loc = loc;
//     }
//
//     public bool PreviewsAreValid(IReadOnlyList<Preview> previews, out string errorMessage)
//     {
//       Plugin.Log.LogError("validating");
//       if (this.CurrentDistrictHasGlobalMarket((IEnumerable<Preview>) previews))
//       {
//         errorMessage = this._loc.T(GlobalMarktPreviewsValidator.ErrorMessageLocKey);
//         return false;
//       }
//       errorMessage = (string) null;
//       return true;
//     }
//
//     // private bool CurrentDistrictHasGlobalMarket(IEnumerable<Preview> previews) =>
//     //   _districtService.ArePreviewDistrictsInConflict(previews.Select(preview => preview.GetComponent<Prefab>())
//     //     .Where(prefab => prefab.PrefabName == "LargeWarehouse.Folktails")
//     //     .Select(prefab => prefab.GetComponent<BlockObject>().PositionedEntrance.DoorstepCoordinates));
//     
//     private bool CurrentDistrictHasGlobalMarket(IEnumerable<Preview> previews) => this._districtService.ArePreviewDistrictsInConflict(previews.Select(preview => preview.GetComponent<Prefab>()).Where<Prefab>((Func<Prefab, bool>) (districtCenter => (bool) (UnityEngine.Object) districtCenter)).Select<Prefab, Vector3Int>((Func<Prefab, Vector3Int>) (prefab => prefab.GetComponent<BlockObject>().PositionedEntrance.DoorstepCoordinates)));
//   }
// }
