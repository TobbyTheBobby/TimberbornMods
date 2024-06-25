// using Bindito.Core;
// using Timberborn.AssetSystem;
// using Timberborn.Characters;
// using Timberborn.EntityPanelSystem;
// using Timberborn.Localization;
// using Timberborn.SelectionSystem;
// using UnityEngine;
//
// namespace Timberborn.BeaversUI
// {
//   public class AirBalloonEntityBadge : MonoBehaviour, IModifiableEntityBadge
//   {
//     private const string GlobalMarketDisplayNameLocKey = "Tobbert.GlobalMarket.DisplayName";
//     
//     private ILoc _loc;
//     
//     private IResourceAssetLoader _resourceAssetLoader;
//     
//     private SelectionManager _selectionManager;
//     
//     private Character _character;
//     
//     private GlobalMarketServant _globalMarketServant;
//
//     [Inject]
//     public void InjectDependencies(
//       ILoc loc,
//       IResourceAssetLoader resourceAssetLoader,
//       SelectionManager selectionManager)
//     {
//       _loc = loc;
//       _resourceAssetLoader = resourceAssetLoader;
//       _selectionManager = selectionManager;
//     }
//
//     public int EntityBadgePriority => 1;
//
//     public void Awake()
//     {
//       _character = GetComponent<Character>();
//       _globalMarketServant = GetComponent<GlobalMarketServant>();
//     }
//
//     public string GetEntityName() => "<b>" + _character.FirstName + "</b>";
//
//     public void SetEntityName(string entityName) => _character.FirstName = entityName;
//
//     public string GetEntitySubtitle()
//     {
//       return "";
//     }
//
//     public ClickableSubtitle GetEntityClickableSubtitle()
//     {
//       GameObject globalMarket = _globalMarketServant.LinkedGlobalMarket;
//       return ClickableSubtitle.Create(() => _selectionManager.SelectAndFocusOn(globalMarket), _loc.T(GlobalMarketDisplayNameLocKey));
//     }
//
//     public Sprite GetEntityAvatar() => _resourceAssetLoader.Load<Sprite>("tobbert.globalmarket/tobbert_globalmarket/PilotAvatar");
//   }
// }
