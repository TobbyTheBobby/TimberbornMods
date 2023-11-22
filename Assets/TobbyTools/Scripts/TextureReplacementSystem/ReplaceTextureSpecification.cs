namespace TobbyTools.TextureReplacementTool
{
    public class ReplaceTextureSpecification 
    {
        public ReplaceTextureSpecification(
            string buildingName,
            string materialName,
            string replacementTextureName)
        {
            BuildingName = buildingName;
            MaterialName = materialName;
            ReplacementTextureName = replacementTextureName;
        }

        public string BuildingName { get; }
        public string MaterialName { get; }
        public string ReplacementTextureName { get; }
    }
}