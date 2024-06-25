using System.Collections.Generic;
using System.Collections.Immutable;
using MorePaths.Core;
using MorePaths.Settings;
using MorePaths.Specifications;
using TimberApi.DependencyContainerSystem;
using TimberApi.UiBuilderSystem;
using Timberborn.CoreUI;
using Timberborn.Localization;
using Timberborn.SingletonSystem;
using TobbyTools.ImageRepository;
using UnityEngine;
using UnityEngine.UIElements;

namespace MorePaths
{
    public class MorePathsSettingsUI : ILoadableSingleton
    {
        private readonly ImageRepositoryService _imageRepositoryService;
        private readonly MorePathsCore _morePathsCore;
        private readonly UIBuilder _builder;
        private readonly ILoc _loc;

        private ImmutableArray<PathSpecification> _pathsSpecifications;

        private readonly Dictionary<string, Texture2D> _textures = new();

        MorePathsSettingsUI(ImageRepositoryService imageRepositoryService, MorePathsCore morePathsCore, UIBuilder uiBuilder, ILoc loc)
        {
            _imageRepositoryService = imageRepositoryService;
            _morePathsCore = morePathsCore;
            _builder = uiBuilder;
            _loc = loc;
        }

        public void Load()
        {
            _pathsSpecifications = _morePathsCore.PathsSpecifications;
            
            foreach (var pathsSpecification in _pathsSpecifications)
            {
                if (pathsSpecification.Name == "DefaultPath")
                    continue;

                _textures.Add(pathsSpecification.Name, _imageRepositoryService.GetByName(pathsSpecification.PathTexture, pathsSpecification.Name));
            }
        }
        
        public void InitializeSelectorSettings(ref VisualElement root)
        {
            var container = _builder.CreateComponentBuilder().CreateVisualElement()
                .SetWidth(new Length(100, LengthUnit.Percent))
                .SetJustifyContent(Justify.Center)
                .SetAlignContent(Align.Center)
                .SetAlignItems(Align.Center)
                .BuildAndInitialize();
            
            var customPathsHeader = _builder.CreateComponentBuilder()
                .CreateVisualElement()
                .AddPreset(factory =>
                {
                    var test = factory.Labels().DefaultHeader();
                    test.TextLocKey = "Tobbert.MorePaths.SettingsHeader";
                    test.style.fontSize = new Length(16, LengthUnit.Pixel);
                    test.style.unityFontStyleAndWeight = FontStyle.Bold;
                    return test;
                })
                .BuildAndInitialize();
            
            var listView = _builder.CreateComponentBuilder()
                .CreateScrollView()
                .SetName("SelectorList").AddChildren(CreateRowItem())
                .SetWidth(new Length(60, LengthUnit.Percent))
                .SetHeight(250)
                .SetAlignContent(Align.Center)
                .SetJustifyContent(Justify.Center)
                .BuildAndInitialize();

            container.Add(customPathsHeader);
            container.Add(listView);
            
            var toggle = root.Q<Toggle>("AutoSavingOn");
            toggle.parent.Add(container);
        }

        private IEnumerable<VisualElement> CreateRowItem()
        {
            foreach (var pathSpecification in _pathsSpecifications)
            {
                var image = new Image
                {
                    style =
                    {
                        width = new StyleLength(40),
                        height = new StyleLength(40)
                    }
                };

                var row = _builder.CreateComponentBuilder()
                    .CreateVisualElement()
                    .SetFlexDirection(FlexDirection.Row)
                    .SetAlignItems(Align.Center)
                    .SetPadding(5)
                    .AddComponent(image)
                    .AddComponent(new VisualElement())
                    .SetJustifyContent(Justify.SpaceBetween)
                    .SetAlignItems(Align.Center)
                    .AddPreset(factory => factory.Labels().DefaultText())
                    .AddPreset(factory => factory.Toggles().Checkbox())
                    .BuildAndInitialize();

                row.style.height = new Length(50, LengthUnit.Pixel);
            
                var name = pathSpecification.Name;

                if (name.Equals("DefaultPath"))
                {
                    row.Q<Image>().ToggleDisplayStyle(false);
                    row.Q<Label>().text = name;
                }
                else
                {
                    row.Q<Image>().image = _textures[name];
                    row.Q<Label>().text = _loc.T(pathSpecification.DisplayNameLocKey);
                }
                row.Q<Toggle>().SetValueWithoutNotify(DependencyContainer.GetInstance<MorePathsSettings>().GetSetting(name).Enabled);
                row.Q<Toggle>().RegisterValueChangedCallback(e => OnSettingChanged(name, e.newValue));
            
                yield return row;
            }
        }

        private void OnSettingChanged(string settingName, bool value)
        {
            DependencyContainer.GetInstance<MorePathsSettings>().ChangeSetting(settingName, value);
        }
    }
}