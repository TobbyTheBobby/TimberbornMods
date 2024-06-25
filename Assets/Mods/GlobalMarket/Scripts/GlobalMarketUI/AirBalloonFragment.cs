// using GlobalMarket;
// using Timberborn.BlockSystem;
// using Timberborn.Buildings;
// using Timberborn.CoreUI;
// using Timberborn.EntityPanelSystem;
// using Timberborn.Localization;
// using UnityEngine;
// using UnityEngine.UIElements;
//
// namespace Timberborn.EmptyingUI
// {
//   internal class AirBalloonFragment : IEntityPanelFragment
//   {
//     private readonly ILoc _loc;
//     
//     private readonly VisualElementLoader _visualElementLoader;
//
//     private const string EnableAirBalloonLocKey = "Tobbert.GlobalMarket.EnableAirBalloon";
//
//     private AirBalloonManager _airBalloonManager;
//     
//     private BlockObject _blockObject;
//     
//     private Toggle _airBalloonToggle;
//     
//     private VisualElement _root;
//
//     public AirBalloonFragment(ILoc loc, VisualElementLoader visualElementLoader)
//     {
//       _loc = loc;
//       _visualElementLoader = visualElementLoader;
//     }
//
//     public VisualElement InitializeFragment()
//     {
//       _root = _visualElementLoader.LoadVisualElement("Master/EntityPanel/EmptiableFragment");
//       _airBalloonToggle = _root.Q<Toggle>("Toggle");
//       _airBalloonToggle.text = _loc.T(EnableAirBalloonLocKey);
//       _airBalloonToggle.RegisterValueChangedCallback(value => EnableAirBalloon(value.newValue));
//       _root.ToggleDisplayStyle(false);
//       return _root;
//     }
//
//     public void ShowFragment(GameObject entity)
//     {
//       _airBalloonManager = entity.GetComponent<AirBalloonManager>();
//       _blockObject = entity.GetComponent<BlockObject>();
//       if (!(bool) (Object) _airBalloonManager)
//         return;
//       
//       _airBalloonToggle.SetValueWithoutNotify(_airBalloonManager.AirBalloonEnabled);
//     }
//
//     public void ClearFragment()
//     {
//       _airBalloonManager = null;
//       _blockObject = null;
//       UpdateFragment();
//     }
//
//     public void UpdateFragment() => _root.ToggleDisplayStyle((bool) (Object) _airBalloonManager && _blockObject.Finished);
//
//     private void EnableAirBalloon(bool newState)
//     {
//       if (newState)
//         _airBalloonManager.EnableAirBalloon();
//       else
//         _airBalloonManager.DisableAirBalloon();
//     }
//   }
// }
