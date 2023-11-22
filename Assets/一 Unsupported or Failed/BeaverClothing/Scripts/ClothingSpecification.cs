using System.Collections.Generic;

namespace BeaverHats
{
    public class ClothingSpecification 
    {
        public ClothingSpecification(
            bool enabled,
            string prefabPath,
            string characterType,
            string bodyPartName,
            List<string> workPlaces,
            int wearChance,
            float positionX,
            float positionY,
            float positionZ,
            float rotationX,
            float rotationY,
            float rotationZ,
            float scaleX,
            float scaleY,
            float scaleZ
            )
        {
            Enabled = enabled;
            PrefabPath = prefabPath;
            CharacterType = characterType;
            BodyPartName = bodyPartName;
            WorkPlaces = workPlaces;
            WearChance = wearChance;

            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;

            RotationX = rotationX;
            RotationY = rotationY;
            RotationZ = rotationZ;

            ScaleX = scaleX;
            ScaleY = scaleY;
            
            ScaleZ = scaleZ;
        }

        public bool Enabled { get; }
        public string PrefabPath { get; }
        public string CharacterType { get; }
        public string BodyPartName { get; }
        public List<string> WorkPlaces { get; }
        public int WearChance { get; }
        public float PositionX { get; }
        public float PositionY { get; }
        public float PositionZ { get; }
        public float RotationX { get; }
        public float RotationY { get; }
        public float RotationZ { get; }
        public float ScaleX { get; }
        public float ScaleY { get; }
        public float ScaleZ { get; }
    }
}