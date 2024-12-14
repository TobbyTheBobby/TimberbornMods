// using System.Collections.Generic;
// using System.IO;
// using Timberborn.AssetSystem;
// using Timberborn.InputSystem;
// using UnityEngine;
//
// namespace CustomCursors
// {
//     public class CustomCursorAssetsProvider : IAssetProvider
//     {
//         private readonly CursorPackRepository _cursorPackRepository;
//
//         public CustomCursorAssetsProvider(CursorPackRepository cursorPackRepository)
//         {
//             _cursorPackRepository = cursorPackRepository;
//         }
//
//         public bool IsBuiltIn => false;
//         
//         public bool TryLoad<T>(string path, out OrderedAsset<T> orderedAsset) where T : Object
//         {
//             if (typeof(T) != typeof(CustomCursor))
//             {
//                 orderedAsset = new OrderedAsset<T>();
//                 return false;
//             }
//             
//             Debug.LogError($"path {path}");
//             var fileName = Path.GetFileName(path);
//             Debug.LogError($"fileName {fileName}");
//             foreach (var cursorPack in _cursorPackRepository.CursorPacks)
//             {
//                 if (fileName == cursorPack.PackName.ToLower())
//                 {
//                     if (fileName.Contains(CustomCursorsService.SelectionCursorExtension) && cursorPack.TryGetSelectionCursor(out var selectionCursor))
//                     {
//                         orderedAsset = new OrderedAsset<T>(0, selectionCursor as T);
//                         return true;
//                     }
//                     
//                     if (fileName.Contains(CustomCursorsService.DraggingCursorExtension) && cursorPack.TryGetDraggingCursor(out var draggingCursor))
//                     {
//                         orderedAsset = new OrderedAsset<T>(0, draggingCursor as T);
//                         return true;
//                     }
//                 }
//             }
//             
//             orderedAsset = new OrderedAsset<T>();
//             return false;
//         }
//
//         public IEnumerable<OrderedAsset<T>> LoadAll<T>(string path) where T : Object
//         {
//             if (typeof(T) != typeof(CustomCursor)) 
//                 yield break;
//
//             var i = 0;
//             var fileName = Path.GetFileName(path);
//             foreach (var cursorPack in _cursorPackRepository.CursorPacks)
//             {
//                 if (fileName == cursorPack.PackName)
//                 {
//                     if (cursorPack.TryGetSelectionCursor(out var selectionCursor))
//                     {
//                         yield return new OrderedAsset<T>(i, selectionCursor as T);
//                         i++;
//                     }
//                 }
//             }
//         }
//
//         public void Reset()
//         {
//             
//         }
//     }
// }