namespace BeaverHats
{
    public class WorkplaceClothingSpecification 
    {
        public WorkplaceClothingSpecification(
            bool enabled,
            string workplace,
            int wearChance
            )
        {
            Enabled = enabled;
            Workplace = workplace;
            WearChance = wearChance;
        }

        public bool Enabled { get; }
        public string Workplace { get; }
        public int WearChance { get; }
    }
}