namespace ChooChoo
{
    public class WagonModelSpecification
    {
        public WagonModelSpecification(
            string faction,
            string nameLocKey,
            string modelLocation,
            string dependentModel,
            float length)
        {
            Faction = faction;
            NameLocKey = nameLocKey;
            ModelLocation = modelLocation;
            DependentModel = dependentModel;
            Length = length;
        }

        public string Faction { get; }
        public string NameLocKey { get; }
        public string ModelLocation { get; }
        public string DependentModel { get; }
        public float Length { get; }
    }
}