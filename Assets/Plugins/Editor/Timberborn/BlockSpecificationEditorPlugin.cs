using Timberborn.BlockSystem;
using UnityEditor;
using UnityEngine;

namespace EditorPlugins {
  [CustomPropertyDrawer(typeof(BlockSpecification))]
  public class PropertyDrawer : UnityEditor.PropertyDrawer {

    private static readonly int TotalAmountOfProperties = 4;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      EditorGUI.BeginProperty(position, label, property);
      
      string[] propertyFields = {
        "_matterBelow", "_occupation", "_stackable", "_optionallyUnderground"
      };
    
      for (var i = 0; i < propertyFields.Length; i++)
      {
        var relativeProperty = property.FindPropertyRelative(propertyFields[i]);
        var rect = new Rect(position.x + position.width / TotalAmountOfProperties * i, position.y,
          position.width / TotalAmountOfProperties, position.height);
        EditorGUI.PropertyField(rect, relativeProperty, GUIContent.none);
      }
      
      var oldLabelWidth = EditorGUIUtility.labelWidth;
      EditorGUIUtility.labelWidth = 70.0f;
      
      EditorGUIUtility.labelWidth = oldLabelWidth;

      EditorGUI.EndProperty();
    }
  }
}

namespace BlockSystem.Editor {
  [CustomPropertyDrawer(typeof(BlocksSpecification))]
  public class PropertyDrawer : UnityEditor.PropertyDrawer {

    private static readonly int LabelWidth = 100;
    private static readonly int ColumnNameHeight = 50;
    private static readonly float RowHeight = EditorGUIUtility.singleLineHeight;
    private static readonly string[] ColumnLabels = {
        "Matter below", "Occupation", "Stackable", "Optionally underground"
    };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      EditorGUI.BeginProperty(position, label, property);
      
      var sizeProperty = property.FindPropertyRelative("_size");
      var specificationProperty = property.FindPropertyRelative("_blockSpecifications");
      
      var structureSize = sizeProperty.vector3IntValue;
      
      var expectedStructureSize = structureSize.x * structureSize.y * structureSize.z;
      if (specificationProperty.arraySize != expectedStructureSize) {
        specificationProperty.arraySize = expectedStructureSize;
      }
      
      EditorGUI.PropertyField(position, sizeProperty);
      var sizePropertyHeight = EditorGUI.GetPropertyHeight(sizeProperty);
      for (var i = 0; i < ColumnLabels.Length; i++) {
        GUI.Label(
          new(position.x + LabelWidth + (position.width - LabelWidth) / ColumnLabels.Length * i,
                position.y + sizePropertyHeight,
                (position.width - LabelWidth) / ColumnLabels.Length,
                ColumnNameHeight),
            ColumnLabels[i]);
      }
      
      var coords = new Vector3Int();
      var blockDisplayIndex = 0;
      for (coords.z = 0; coords.z < structureSize.z; ++coords.z) {
        for (coords.x = 0; coords.x < structureSize.x; ++coords.x) {
          for (coords.y = 0; coords.y < structureSize.y; ++coords.y) {
            var blockLabelText = coords.x + ", " + coords.y + ", " + coords.z;
            var blockIndex = IndexFromCoordinates(coords, structureSize);
            var blockLabelRect = new Rect(position.x,
                                          position.y
                                          + blockDisplayIndex * RowHeight
                                          + sizePropertyHeight
                                          + ColumnNameHeight,
                                          LabelWidth, RowHeight);
            var blockRect = new Rect(position.x + LabelWidth,
                                     position.y
                                     + blockDisplayIndex * RowHeight
                                     + sizePropertyHeight
                                     + ColumnNameHeight,
                                     position.width - LabelWidth, RowHeight);
      
            GUI.Label(blockLabelRect, blockLabelText);
            EditorGUI.PropertyField(blockRect, specificationProperty.GetArrayElementAtIndex(blockIndex));
      
            ++blockDisplayIndex;
          }
        }
      }
      
      EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      var sizeProperty = property.FindPropertyRelative("_size");
      var sizePropertyHeight = EditorGUI.GetPropertyHeight(sizeProperty);
      var structureSize = sizeProperty.vector3IntValue;
      var numberOfBlocks = structureSize.x * structureSize.y * structureSize.z;
      return numberOfBlocks * RowHeight + sizePropertyHeight + ColumnNameHeight;
    }

    private int IndexFromCoordinates(Vector3Int coordinates, Vector3Int structureSize) {
      return (coordinates.z * structureSize.y + coordinates.y) * structureSize.x + coordinates.x;
    }

  }
}