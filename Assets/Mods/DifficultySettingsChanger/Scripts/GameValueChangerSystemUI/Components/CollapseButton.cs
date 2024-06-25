using System;
using System.Collections.Generic;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace DifficultySettingsChanger.GameValueChangerSystemUI.Components
{
    public class CollapseButton
    {
        private readonly GameValueChangerUiPresetFactory _gameValueChangerUiPresetFactory;
        private readonly VisualElement _subContainer;
        private readonly Func<List<GameValueChanger>> _gameGameValueChangersGetters;

        private bool _isCollapsed = true;

        private CollapseButton(
            GameValueChangerUiPresetFactory gameValueChangerUiPresetFactory, 
            VisualElement subContainer,
            Func<List<GameValueChanger>> gameValueChangersGetter)
        {
            _gameValueChangerUiPresetFactory = gameValueChangerUiPresetFactory;
            _subContainer = subContainer;
            _gameGameValueChangersGetters = gameValueChangersGetter;
        }

        public static void Create(
            GameValueChangerUiPresetFactory gameValueChangerUiPresetFactory,
            Button button,
            Func<List<GameValueChanger>> gameValueChangersGetter,
            VisualElement container)
        {
            var subContainer = new VisualElement();
            subContainer.AddToClassList("mods-box__game-value-changer-sub-container");
            var collapseButton = new CollapseButton(gameValueChangerUiPresetFactory, subContainer, gameValueChangersGetter);
            subContainer.ToggleDisplayStyle(false);
            button.clicked += () => collapseButton.OnClick();
            container.Add(button);
            container.Add(subContainer);
        }

        private void OnClick()
        {
            Plugin.Log.LogInfo($"_isCollapsed {_isCollapsed}");
            if (_isCollapsed)
            {
                UnCollapse();
            }
            else
            {
                Collapse();
            }
            
            _subContainer.ToggleDisplayStyle(!_isCollapsed);
        }

        private void UnCollapse()
        {
            _isCollapsed = false;
            Plugin.Log.LogInfo(_gameGameValueChangersGetters().Count + "");
            foreach (var fieldValueChanger in _gameGameValueChangersGetters())
            {
                _subContainer.Add(_gameValueChangerUiPresetFactory.GetUiPreset(fieldValueChanger));
            }
        }

        private void Collapse()
        {
            _subContainer.Clear();
            _isCollapsed = true;
        }
    }
}