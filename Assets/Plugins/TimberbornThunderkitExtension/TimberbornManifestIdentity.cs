using ThunderKit.Core.Attributes;
using ThunderKit.Core.Manifests;
using ThunderKit.Core.Manifests.Datum;
using UnityEditor;

namespace TimberbornThunderkitExtension
{
    [HideFromScriptWindow]
    public class TimberbornManifestIdentity : ManifestIdentity
    {
        public string Id;
        public string MinimumGameVersion;
        public Manifest[] RequiredMods;
    }
    
    [CustomEditor(typeof(TimberbornManifestIdentity))]
    public class DerivedClassEditor : Editor
    {
        private SerializedProperty _derivedField1;
        private SerializedProperty _derivedField2;
        private SerializedProperty _derivedField3;
        private SerializedProperty _derivedField4;
        private SerializedProperty _derivedField5;
        private SerializedProperty _derivedField6;

        private void OnEnable()
        {
            _derivedField1 = serializedObject.FindProperty("Name");
            _derivedField2 = serializedObject.FindProperty("Version");
            _derivedField3 = serializedObject.FindProperty("Id");
            _derivedField4 = serializedObject.FindProperty("MinimumGameVersion");
            _derivedField5 = serializedObject.FindProperty("Description");
            _derivedField6 = serializedObject.FindProperty("RequiredMods");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_derivedField1);
            EditorGUILayout.PropertyField(_derivedField2);
            EditorGUILayout.PropertyField(_derivedField3);
            EditorGUILayout.PropertyField(_derivedField4);
            EditorGUILayout.PropertyField(_derivedField5);
            EditorGUILayout.PropertyField(_derivedField6);

            serializedObject.ApplyModifiedProperties();
        }
    }
}