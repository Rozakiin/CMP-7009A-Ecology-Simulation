using System;
using Unity.Entities;

namespace Components
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct SizeData : IComponentData
    {
        public float size;
        public float sizeMultiplier;
        public float ageSizeMultiplier;
        public float youngSizeMultiplier;
        public float adultSizeMultiplier;
        public float oldSizeMultiplier;
        public float Size
        {
            get { return size * sizeMultiplier * ageSizeMultiplier; }
        }
    }
}
