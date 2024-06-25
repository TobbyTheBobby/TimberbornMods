namespace PlantingSeeds
{
    public class PlantableSeedSpecification
    {
        public PlantableSeedSpecification(string plantablePrefabName, string goodId, int goodAmount)
        {
            PlantablePrefabName = plantablePrefabName;
            GoodId = goodId;
            GoodAmount = goodAmount;
        }

        public string PlantablePrefabName { get; }

        public string GoodId { get; }

        public int GoodAmount { get; }
    }
}