using System.Collections.Generic;
using System.IO;
using System.Linq;
using TimberApi.UiBuilderSystem;
using Timberborn.SettingsSystem;
using Timberborn.SingletonSystem;
using Timberborn.ToolSystem;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace CustomCursors
{
    public class CustomCursorsService : ILoadableSingleton

    {
        private readonly UIBuilder _builder;
        private readonly ISettings _settings;
        private readonly EventBus _eventBus;

        CustomCursorsService(UIBuilder uiBuilder, ISettings settings, EventBus eventBus)
        {
            _builder = uiBuilder;
            _settings = settings;
            _eventBus = eventBus;
        }

        private Texture2D _selectorCursor = new(150, 150);
        private Texture2D _grabberCursor = new(150, 150);
        private const CursorMode CursorMode = UnityEngine.CursorMode.Auto;
        private readonly Vector2 _hotSpot = Vector2.zero;

        private readonly Dictionary<string, List<Texture2D>> _cursors = new();
        private readonly List<string> _cursorPacks = new();

        private static readonly string CurrentCursorPackKey = nameof (CurrentCursorPack);

        public string CurrentCursorPack
        {
            get => _settings.GetString(CurrentCursorPackKey, "DefaultCursor");
            set => _settings.SetString(CurrentCursorPackKey, value);
        }

        public void Load()
        {
            var path = Path.Combine(Plugin.MyPath, "Cursors");
            foreach (var cursorPackPath in Directory.GetDirectories(path))
            {
                var files = Directory.GetFiles(cursorPackPath).Where(s => !s.Contains(".meta")).ToList();
                for (int i = 0; i < files.Count; i++)
                {
                    var filePath = files[i];
                    var cursorPackName = cursorPackPath.Split("\\").Last();
                    if (i == 0)
                        _cursorPacks.Add(cursorPackName);
                    
                    var bytes = File.ReadAllBytes(filePath);
                    var cursorTexture2D = new Texture2D(150, 150);
                    cursorTexture2D.LoadImage(bytes);
                    if (!_cursors.ContainsKey(cursorPackName))
                    {
                        _cursors.Add(cursorPackName, new List<Texture2D>());
                    }
                    _cursors[cursorPackName].Add(cursorTexture2D);
                    _selectorCursor = cursorTexture2D;
                }
            }
            
            UpdateCursor(CurrentCursorPack);
            
            Cursor.SetCursor(_selectorCursor, _hotSpot, CursorMode);
            _eventBus.Register(this);
        }
        
        [OnEvent]
        public void OnToolExited(ToolExitedEvent toolExitedEvent) => UpdateCursor(CurrentCursorPack);
        
        public void StartGrabbing() => Cursor.SetCursor(_grabberCursor, _hotSpot, CursorMode);
        
        public void StopGrabbing() => Cursor.SetCursor(_selectorCursor, _hotSpot, CursorMode);

        private void OnSelectorChanged(IEnumerable<object> obj)
        {
            UpdateCursor((string)obj.First());
        }

        public void InitializeSelectorSettings(ref VisualElement root)
        {
            var listView = _builder.CreateComponentBuilder()
                .CreateListView()
                .SetName("SelectorList")
                .SetItemSource(_cursorPacks)
                .SetMakeItem(() => new Image
                {
                    style =
                    {
                        width = new StyleLength(112),
                        height = new StyleLength(112)
                    }
                })
                .SetBindItem((element, i) => element.Q<Image>().image = _cursors[_cursorPacks[i]][0])
                .SetSelectionChange(OnSelectorChanged)
                .SetWidth(112)
                .SetHeight(200)
                .SetAlignContent(Align.Center)
                .SetJustifyContent(Justify.Center)
                .BuildAndInitialize();

            var toggle = root.Q<Toggle>("AutoSavingOn");

            var container = _builder.CreateComponentBuilder().CreateVisualElement()
                .SetWidth(new Length(100, LengthUnit.Percent))
                .SetJustifyContent(Justify.Center)
                .SetAlignContent(Align.Center)
                .SetAlignItems(Align.Center)
                .BuildAndInitialize();
            
            var myHeader = _builder.CreateComponentBuilder()
                .CreateLabel()
                .SetName("CustomCursorsHeader")
                .SetLocKey("Tobbert.CustomCursors.SettingsHeader")
                .SetColor(new StyleColor(Color.white))
                .SetFontSize(new Length(16, LengthUnit.Pixel))
                .SetFontStyle(FontStyle.Bold)
                .SetWidth(new Length(112, LengthUnit.Pixel))
                .BuildAndInitialize();
            
            container.Add(myHeader);
            container.Add(listView);

            toggle.parent.Add(container);
        }

        private void UpdateCursor(string newCursorPack)
        {
            CurrentCursorPack = newCursorPack;
            var texture2Ds = _cursors[newCursorPack];
            if (texture2Ds.Count == 1)
            {
                _selectorCursor = texture2Ds[0];
                _grabberCursor = texture2Ds[0];
            }
            else
            {
                for (int i = 0; i < texture2Ds.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            _selectorCursor = texture2Ds[i];
                            break;
                        case 1:
                            _grabberCursor = texture2Ds[i];
                            break;
                    }
                }
            }

            Cursor.SetCursor(_selectorCursor, _hotSpot, CursorMode);
        }
    }
}
