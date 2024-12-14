// using System;
// using System.Collections.Generic;
// using System.Linq;
// using ChooChoo.TrackSystem;
// using Timberborn.BlockSystem;
// using Timberborn.Common;
// using Timberborn.Coordinates;
// using Timberborn.CoreUI;
// using UnityEditor;
// using UnityEditor.UIElements;
// using UnityEngine;
// using UnityEngine.UIElements;
//
// namespace ChooChoo.Editor
// {
//     [CustomEditor(typeof(TrackPieceSpec))]
//     internal class CornerInspector : UnityEditor.Editor
//     {
//         private TrackPieceSpec _trackPieceSpec;
//         private int? _lastHoverCoords;
//         private VisualElement _root;
//         private readonly Dictionary<int, List<VisualElement>> _blockSpecificationElements = new();
//         private DropdownField _matterBelowDropdown;
//         private EnumFlagsField _occupationDropdown;
//         private DropdownField _stackableDropdown;
//
//         public event EventHandler BlockObjectSizeChanged;
//
//         public int Size { get; set; }
//
//         public int MinLayer => 0;
//
//         public bool HideUnoccupied { get; set; }
//
//         public void OnEnable()
//         {
//             _trackPieceSpec = (TrackPieceSpec)target;
//             Size = 1;
//         }
//
//         public override VisualElement CreateInspectorGUI()
//         {
//             _root = new VisualElement();
//             Resources.Load<VisualTreeAsset>("UI/Views/Editor/BlockSystem/BlockObject").CloneTree(_root);
//             CreateBlockSpecifications();
//             _root.Q<Button>("SetMatterBelowButton").RegisterCallback((EventCallback<ClickEvent>)(_ => SetMatterBelowForVisibleLayers()));
//             _root.Q<Button>("SetOccupationButton").RegisterCallback((EventCallback<ClickEvent>)(_ => SetOccupationForVisibleLayers()));
//             _root.Q<Button>("SetStackableButton").RegisterCallback((EventCallback<ClickEvent>)(_ => SetStackableForVisibleLayers()));
//             _matterBelowDropdown = _root.Q<DropdownField>("MatterBelowDropdown");
//             _matterBelowDropdown.choices = Enum.GetNames(typeof(MatterBelow)).ToList();
//             _matterBelowDropdown.index = 0;
//             _occupationDropdown = _root.Q<EnumFlagsField>("OccupationDropdown");
//             _occupationDropdown.Init(~BlockOccupations.None);
//             _stackableDropdown = _root.Q<DropdownField>("StackableDropdown");
//             _stackableDropdown.choices = Enum.GetNames(typeof(BlockStackable)).ToList();
//             _stackableDropdown.index = 0;
//             return _root;
//         }
//
//         public void RefreshBlocksVisibility()
//         {
//             foreach (var specificationElement in _blockSpecificationElements)
//             {
//                 int num1;
//                 List<VisualElement> visualElementList1;
//                 specificationElement.Deconstruct(out num1, out visualElementList1);
//                 var num2 = num1;
//                 var visualElementList2 = visualElementList1;
//                 var visible = num2 >= MinLayer && num2 <= Size;
//                 foreach (var visualElement in visualElementList2)
//                     visualElement.ToggleDisplayStyle(visible);
//             }
//         }
//
//         private SerializedProperty SizeProperty => serializedObject.FindProperty("_trackRouteCount");
//
//         private void CreateBlockSpecifications()
//         {
//             var field = _root.Q<PropertyField>("Size");
//             field.BindProperty(SizeProperty);
//             field.RegisterValueChangeCallback(OnSizeChanged);
//             _root.schedule.Execute(UpdateBlockSpecifications);
//         }
//
//         private void OnSizeChanged(SerializedPropertyChangeEvent evt)
//         {
//             Size = evt.changedProperty.intValue;
//             var num = evt.changedProperty.intValue;
//             var propertyRelative = serializedObject.FindProperty("_trackRouteSpecs");
//             if (propertyRelative.arraySize != num)
//             {
//                 propertyRelative.arraySize = num;
//                 serializedObject.ApplyModifiedProperties();
//             }
//
//             UpdateBlockSpecifications();
//             var objectSizeChanged = BlockObjectSizeChanged;
//             if (objectSizeChanged == null)
//                 return;
//             objectSizeChanged(this, EventArgs.Empty);
//         }
//
//         private void UpdateBlockSpecifications()
//         {
//             var vector3IntValue = SizeProperty.intValue;
//             var propertyRelative = serializedObject.FindProperty("_trackRouteSpecs");
//             var visualElement = _root.Q<VisualElement>("BlocksContainer");
//             visualElement.Clear();
//             _blockSpecificationElements.Clear();
//             _lastHoverCoords = new int?();
//             for (int i = 0; i < vector3IntValue; i++)
//             {
//                 var arrayElementAtIndex = propertyRelative.GetArrayElementAtIndex(i);
//                 var specificationElement = CreateBlockSpecificationElement(i, arrayElementAtIndex);
//                 visualElement.Add(specificationElement);
//             }
//         }
//
//         private VisualElement CreateBlockSpecificationElement(
//             int coords,
//             SerializedProperty blockProperty)
//         {
//             var root = new VisualElement
//             {
//                 style =
//                 {
//                     borderBottomWidth = 2, borderBottomColor = Color.black,
//                     borderTopWidth = 2, borderTopColor = Color.black,
//                     borderLeftWidth = 2, borderLeftColor = Color.black,
//                     borderRightWidth = 2, borderRightColor = Color.black,
//                     
//                     paddingBottom = 3,
//                     paddingTop = 3,
//                     paddingLeft = 3,
//                     paddingRight = 3,
//                 }
//             };
//             root.Add(new Label { text = coords.ToString() });
//             var entrance = new PropertyField();
//             entrance.BindProperty(blockProperty.FindPropertyRelative("_entrance"));
//             root.Add(entrance);
//             var exit = new PropertyField();
//             exit.BindProperty(blockProperty.FindPropertyRelative("_exit"));
//             root.Add(exit);
//             var routeCorners = new PropertyField();
//             routeCorners.BindProperty(blockProperty.FindPropertyRelative("_routeCorners"));
//             root.Add(routeCorners);
//             // root.Q<PropertyField>("MatterBelow").BindProperty(blockProperty.FindPropertyRelative("_entrance"));
//             // root.Q<PropertyField>("Occupations").BindProperty(blockProperty.FindPropertyRelative("_exit"));
//             // root.Q<PropertyField>("Stackable").BindProperty(blockProperty.FindPropertyRelative("_routeCorners"));
//             // var propertyRelative = blockProperty.FindPropertyRelative("_useNewOccupation");
//             // if (!propertyRelative.boolValue)
//             // {
//             //     propertyRelative.boolValue = true;
//             //     propertyRelative.serializedObject.ApplyModifiedProperties();
//             // }
//
//             root.RegisterCallback((EventCallback<MouseEnterEvent>)(_ =>
//             {
//                 _lastHoverCoords = new int?(coords);
//                 SceneView.RepaintAll();
//             }));
//             _blockSpecificationElements.GetOrAdd(coords, () => new List<VisualElement>()).Add(root);
//             return root;
//         }
//
//         private static int IndexFromCoordinates(Vector3Int coordinates, Vector3Int size)
//         {
//             return (coordinates.z * size.y + coordinates.y) * size.x + coordinates.x;
//         }
//
//         private void SetMatterBelowForVisibleLayers()
//         {
//             OverrideVisibleLayers(visualElement => visualElement.Q<PropertyField>("MatterBelow").Q<PopupField<string>>().index = _matterBelowDropdown.index);
//         }
//
//         private void SetOccupationForVisibleLayers()
//         {
//             var occupation = (BlockOccupations)_occupationDropdown.value;
//             OverrideVisibleLayers(visualElement => visualElement.Q<PropertyField>("Occupations").Q<EnumFlagsField>().value = occupation);
//         }
//
//         private void SetStackableForVisibleLayers()
//         {
//             OverrideVisibleLayers(visualElement => visualElement.Q<PropertyField>("Stackable").Q<PopupField<string>>().index = _stackableDropdown.index);
//         }
//
//         private void OverrideVisibleLayers(Action<VisualElement> overrideAction)
//         {
//             foreach (var specificationElement in _blockSpecificationElements)
//             {
//                 int num1;
//                 List<VisualElement> visualElementList1;
//                 specificationElement.Deconstruct(out num1, out visualElementList1);
//                 var num2 = num1;
//                 var visualElementList2 = visualElementList1;
//                 if (num2 >= MinLayer && num2 <= Size)
//                 {
//                     foreach (var visualElement in visualElementList2)
//                         overrideAction(visualElement);
//                 }
//             }
//         }
//
//         private void OnSceneGUI()
//         {
//             // CornerRenderer.Draw(_trackPieceSpec, (float)MinLayer, (float)MaxLayer, HideUnoccupied);
//             // if (!_lastHoverCoords.HasValue)
//             //     return;
//             // Handles.color = Color.green;
//             // Vector3 size = new Vector3(0.5f, 0.5f, 0.5f);
//             // Vector3Int vector3Int = _trackPieceSpec.Coordinates + _trackPieceSpec.Orientation.Transform(_lastHoverCoords.Value);
//             // Handles.DrawWireCube(new Vector3((float)vector3Int.x, (float)(vector3Int.z - _trackPieceSpec.BaseZ), (float)vector3Int.y) + size, size);
//         }
//     }
// }