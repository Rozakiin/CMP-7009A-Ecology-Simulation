using System;
using Unity.Entities;

namespace Components
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct SizeData : IComponentData
    {
        public float size;
        public float SizeMultiplier;
        public float AgeSizeMultiplier;
        public float YoungSizeMultiplier;
        public float AdultSizeMultiplier;
        public float OldSizeMultiplier;
        public float Size
        {
            get { return size * SizeMultiplier * AgeSizeMultiplier; }
        }
    }
}
