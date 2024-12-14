// using System.IO;
// using System.Threading.Tasks;
// using ChooChoo.TrackSystem;
// using UnityEditor;
// using UnityEngine;
//
// namespace ChooChoo.Editor
// {
//     [CustomEditor(typeof(TrackPiece), true)]
//     class DecalMeshHelperEditor : UnityEditor.Editor {
//         public override void OnInspectorGUI() {
//             if (GUILayout.Button("Test"))
//             {
//                 var directoryName =  Path.GetDirectoryName(AssetDatabase.GetAssetPath(target));
//
//                 var asset = JsonUtility.ToJson(new 
//                 {
//                     PropertyName = ""
//                 });
//
//                 var path = Path.Combine(directoryName, string.Concat("TrackPieceSpecification.", target.name, ".json"));
//                 
//                 Task.Run(() => File.WriteAllTextAsync(path, asset)).Wait();
//             }
//         }
//     }
// }