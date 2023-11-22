using Timberborn.SettingsSystem;

namespace ChooChoo
{
    public class DefaultModelSettings
    {
        private readonly ISettings _settings;
        
        private string DefaultTrainModelKey => nameof(DefaultTrainModel);
        
        private string DefaultWagonModelKey => nameof(DefaultWagonModel);

        public DefaultModelSettings(ISettings settings)
        {
            _settings = settings;
        }
        
        public string DefaultTrainModel
        {
            get => _settings.GetString(DefaultTrainModelKey, "Tobbert.TrainModel.BigWooden");
            set => _settings.SetString(DefaultTrainModelKey, value);
        }
        
        public string DefaultWagonModel
        {
            get => _settings.GetString(DefaultWagonModelKey, "Tobbert.WagonModel.FlatWagon");
            set => _settings.SetString(DefaultWagonModelKey, value);
        }
    }
}
