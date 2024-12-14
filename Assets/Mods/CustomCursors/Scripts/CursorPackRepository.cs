using System.Collections.Generic;
using System.IO;
using System.Linq;
using Timberborn.InputSystem;
using Timberborn.Modding;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace CustomCursors
{
    public class CursorPackRepository : ILoadableSingleton
    {
        private readonly ModRepository _modRepository;

        private readonly List<CursorPack> _cursorPacks = new();

        public CursorPackRepository(ModRepository modRepository)
        {
            _modRepository = modRepository;
        }

        public List<CursorPack> CursorPacks => _cursorPacks;

        public void Load()
        {
            foreach (var enabledMod in _modRepository.EnabledMods)
            {
                var path = LookForCursorsDirectory(enabledMod.ModDirectory.Path);
                if (path == null)
                    continue;
                foreach (var cursorPackPath in Directory.GetDirectories(path))
                {
                    var packName = Path.GetFileName(cursorPackPath);
                    Debug.Log($"Loading Cursor pack: {packName}");
                    var files = Directory.GetFiles(cursorPackPath);
                    var selectionCursor = LoadImageFile(files, "SelectionCursor");
                    var selectionCustomCursor = CreateCustomCursor(selectionCursor);
                    var draggingCursor = LoadImageFile(files, "DraggingCursor");
                    var draggingCustomCursor = CreateCustomCursor(draggingCursor);
                    _cursorPacks.Add(new CursorPack(packName, selectionCustomCursor, draggingCustomCursor));
                }
            }
        }

        private static string LookForCursorsDirectory(string parentDirectory)
        {
            foreach (var childDirectory in Directory.GetDirectories(parentDirectory))
            {
                if (Path.GetFileName(childDirectory) == "Cursors")
                    return childDirectory;

                var result = LookForCursorsDirectory(childDirectory);
                if (result != null)
                    return result;
            }

            return null;
        }

        private static Texture2D LoadImageFile(IEnumerable<string> files, string fileName)
        {
            var cursorFile = files.FirstOrDefault(file => Path.GetFileNameWithoutExtension(file) == fileName);
            return cursorFile != null ? LoadImage(cursorFile) : null;
        }

        private static Texture2D LoadImage(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath);
            var cursorTexture2D = new Texture2D(150, 150);
            cursorTexture2D.LoadImage(bytes);
            return cursorTexture2D;
        }

        public static CustomCursor CreateCustomCursor(Texture2D cursorTexture, bool forceCreation = false)
        {
            return CreateCustomCursor(cursorTexture, cursorTexture, forceCreation);
        }

        public static CustomCursor CreateCustomCursor(Texture2D smallCursorTexture, Texture2D largeCursorTexture, bool forceCreation = false)
        {
            if (forceCreation == false && (smallCursorTexture == null || largeCursorTexture == null))
            {
                Debug.LogError($"Cannot create custom cursor as either is null: (smallCursorTexture: {smallCursorTexture == null}, largeCursorTexture:{largeCursorTexture == null})");
                return null;
            }

            var selectionCustomCursor = ScriptableObject.CreateInstance<CustomCursor>();
            selectionCustomCursor._smallCursor = smallCursorTexture;
            selectionCustomCursor._largeCursor = largeCursorTexture;
            return selectionCustomCursor;
        }
    }
}