namespace ChooChoo.ModelSystem
{
    public class TrainModelSpecification
    {
        public TrainModelSpecification(
            string faction,
            string nameLocKey,
            string modelLocation,
            float length,
            string machinistSeatName,
            float? machinistScale,
            string machinistAnimationName)
        {
            Faction = faction;
            NameLocKey = nameLocKey;
            ModelLocation = modelLocation;
            Length = length;
            MachinistSeatName = machinistSeatName;
            MachinistScale = machinistScale;
            MachinistAnimationName = machinistAnimationName;
        }

        public string Faction { get; }
        public string NameLocKey { get; }
        public string ModelLocation { get; }
        public float Length { get; }
        public string MachinistSeatName { get; }
        public float? MachinistScale { get; }
        public string MachinistAnimationName { get; }
    }
}