using UnityEngine.UIElements;

namespace DifficultySettingsChanger.GameValueChangerSystemUI
{
    public class GameValueChangersFilter
    {
        private readonly TextField _searchField;

        public string SearchValue => _searchField.value;

        private GameValueChangersFilter(TextField searchField)
        {
            _searchField = searchField;
        }

        public static GameValueChangersFilter Create(TextField searchField)
        {
            var gameValueChangerFilter = new GameValueChangersFilter(searchField);

            return gameValueChangerFilter;
        }

        public void Clear()
        {
            _searchField.Clear();
        }
    }
}