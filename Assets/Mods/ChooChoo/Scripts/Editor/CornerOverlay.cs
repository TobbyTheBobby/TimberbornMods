// using System;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEditor;
// using UnityEditor.Overlays;
// using UnityEngine;
// using UnityEngine.UIElements;
//
// namespace ChooChoo.Editor
// {
//     [Overlay(typeof(SceneView), "TrackPieceSpec", true)]
//     internal class TrackPieceSpecOverlay : Overlay
//     {
//         private VisualElement _root;
//         private CornerInspector _cornerInspector;
//         private Label _layerRangeLabel;
//         private MinMaxSlider _layerRangeSlider;
//         private Toggle _hideUnoccupied;
//
//         public override void OnCreated()
//         {
//             Selection.selectionChanged += new Action(OnSelectionChanged);
//         }
//
//         public override void OnWillBeDestroyed()
//         {
//             if ((bool)(UnityEngine.Object)_cornerInspector)
//                 _cornerInspector.BlockObjectSizeChanged -= new EventHandler(BlockObjectSizeChanged);
//             Selection.selectionChanged -= new Action(OnSelectionChanged);
//             _root = (VisualElement)null;
//             _cornerInspector = (CornerInspector)null;
//             _layerRangeLabel = (Label)null;
//             _layerRangeSlider = (MinMaxSlider)null;
//             _hideUnoccupied = (Toggle)null;
//         }
//
//         public override VisualElement CreatePanelContent()
//         {
//             FindBlockObjectInspector();
//             CreateUI();
//             UpdateUI();
//             return _root;
//         }
//
//         private void OnSelectionChanged()
//         {
//             FindBlockObjectInspector();
//             UpdateUI();
//         }
//
//         private void FindBlockObjectInspector()
//         {
//             if ((bool)(UnityEngine.Object)_cornerInspector)
//                 _cornerInspector.BlockObjectSizeChanged -= new EventHandler(BlockObjectSizeChanged);
//             _cornerInspector = ((IEnumerable<CornerInspector>)Resources.FindObjectsOfTypeAll<CornerInspector>()).FirstOrDefault<CornerInspector>();
//             if (!(bool)(UnityEngine.Object)_cornerInspector)
//                 return;
//             _cornerInspector.BlockObjectSizeChanged += new EventHandler(BlockObjectSizeChanged);
//         }
//
//         private void BlockObjectSizeChanged(object sender, EventArgs e) => UpdateUI();
//
//         private void CreateUI()
//         {
//             _root = new VisualElement();
//             Resources.Load<VisualTreeAsset>("UI/Views/Editor/BlockSystem/BlockObjectOverlay").CloneTree(_root);
//             _layerRangeLabel = _root.Q<Label>("LayerRangeLabel", (string)null);
//             _layerRangeSlider = _root.Q<MinMaxSlider>("LayerRangeSlider", (string)null);
//             _layerRangeSlider.RegisterValueChangedCallback<Vector2>(new EventCallback<ChangeEvent<Vector2>>(OnLayerRangeChanged));
//             _hideUnoccupied = _root.Q<Toggle>("HideUnoccupiedToggle", (string)null);
//             _hideUnoccupied.RegisterValueChangedCallback<bool>(new EventCallback<ChangeEvent<bool>>(OnHideUnoccupiedChanged));
//         }
//
//         private void UpdateUI()
//         {
//             displayed = (bool)(UnityEngine.Object)_cornerInspector && _root != null;
//             if (!displayed)
//                 return;
//             _hideUnoccupied.value = _cornerInspector.HideUnoccupied;
//             _layerRangeSlider.lowLimit = 0.0f;
//             _layerRangeSlider.highLimit = (float)_cornerInspector.MaxLayer;
//             UpdateSliderAndLabel();
//         }
//
//         private void UpdateSliderAndLabel()
//         {
//             if (!(bool)(UnityEngine.Object)_cornerInspector)
//                 return;
//             int minLayer = _cornerInspector.MinLayer;
//             int maxLayer = _cornerInspector.MaxLayer;
//             _layerRangeSlider.SetValueWithoutNotify(new Vector2((float)minLayer, (float)maxLayer));
//             _layerRangeLabel.text = string.Format("Show layers {0} to {1}", (object)minLayer, (object)maxLayer);
//         }
//
//         private void OnLayerRangeChanged(ChangeEvent<Vector2> evt)
//         {
//             if (!(bool)(UnityEngine.Object)_cornerInspector)
//                 return;
//             _cornerInspector.MinLayer = Mathf.RoundToInt(evt.newValue.x);
//             _cornerInspector.MaxLayer = Mathf.RoundToInt(evt.newValue.y);
//             _cornerInspector.RefreshBlocksVisibility();
//             UpdateSliderAndLabel();
//         }
//
//         private void OnHideUnoccupiedChanged(ChangeEvent<bool> evt)
//         {
//             if (!(bool)(UnityEngine.Object)_cornerInspector)
//                 return;
//             _cornerInspector.HideUnoccupied = evt.newValue;
//         }
//     }
// }