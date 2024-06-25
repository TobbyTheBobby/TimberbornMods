namespace TobbyTools.CustomTutorialSystem
{
    public class CustomTutorialStepSettings
    {
        private readonly int? _requiredAmount;
        private readonly string[] _prefabNames;
        private readonly string _goodId;

        public CustomTutorialStepSettings(int? requiredAmount = null, string[] prefabNames = null, string goodId = null)
        {
            _requiredAmount = requiredAmount;
            _prefabNames = prefabNames;
            _goodId = goodId;
        }

        public int RequiredAmount => (int)_requiredAmount;
        public string[] PrefabNames => _prefabNames;
        public string GoodId => _goodId;
    }
}