using System.Linq;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace DifficultySettingsChanger.GameValueChangerSystemUI.Components
{
    public class ObjectWrapper
    {
        private readonly GameValueChangerUiPresetFactory _gameValueChangerUiPresetFactory;
        
        private readonly VisualElement _root;
        private readonly IGrouping<string, GameValueChanger> _groupedGameValueChangers;
        private readonly VisualElement _propertiesSubContainer;
        private readonly ScrollView _gameValueChangerContainer;
        
        public VisualElement Root => _root;

        private ObjectWrapper(
            GameValueChangerUiPresetFactory gameValueChangerUiPresetFactory,
            VisualElement root, 
            IGrouping<string, GameValueChanger> groupedGameValueChangers, 
            VisualElement propertiesSubContainer,
            ScrollView gameValueChangerContainer)
        {
            _gameValueChangerUiPresetFactory = gameValueChangerUiPresetFactory;
            _root = root;
            _groupedGameValueChangers = groupedGameValueChangers;
            _propertiesSubContainer = propertiesSubContainer;
            _gameValueChangerContainer = gameValueChangerContainer;
        }

        public static ObjectWrapper Create(
            GameValueChangerUiPresetFactory gameValueChangerUiPresetFactory,
            VisualElement root, 
            Button button, 
            IGrouping<string, GameValueChanger> groupedGameValueChangers, 
            VisualElement propertiesSubContainer, 
            ScrollView gameValueChangerContainer)
        {
            var objectWrapper = new ObjectWrapper(
                gameValueChangerUiPresetFactory, 
                root, 
                groupedGameValueChangers, 
                propertiesSubContainer,
                gameValueChangerContainer);
            button.RegisterCallback<ClickEvent>(objectWrapper.OnButtonClick);
            return objectWrapper;
        }

        private void OnButtonClick(ClickEvent evt)
        {
            _propertiesSubContainer.Clear();
            foreach (var groupedValueChangers in _groupedGameValueChangers.GroupBy(changer => changer.ParentType.Name))
            {
                _propertiesSubContainer.Add(_gameValueChangerUiPresetFactory.GetGroupHeader(groupedValueChangers.Key));
                foreach (var gameValueChanger in groupedValueChangers.OrderBy(changer => changer.FieldName))
                {
                    _propertiesSubContainer.Add(_gameValueChangerUiPresetFactory.GetPropertyWrapper(gameValueChanger, _gameValueChangerContainer));
                }
            }
        }

        public void ApplyFilter(GameValueChangersFilter gameValueChangersFilter)
        {
            if (_groupedGameValueChangers.Key.ToLower().Contains(gameValueChangersFilter.SearchValue.ToLower()))
            {
                _root.ToggleDisplayStyle(true);
                
            }
            else
            {
                _propertiesSubContainer.Clear();
                _root.ToggleDisplayStyle(false);
            }
        }
    }
}