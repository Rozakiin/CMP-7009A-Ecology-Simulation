using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    [InternalBufferCapacity(200)] //gives max capactity for the buffer
    public struct PathPositionData : IBufferElementData
    {
        public float3 Position; //world position of path node
    }
}