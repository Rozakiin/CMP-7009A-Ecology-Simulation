using Unity.Mathematics;

namespace MonoBehaviourTools.Grid
{
    public struct GridNode
    {
        public bool IsWalkable;
        public int MovementPenalty;

        public float3 WorldPosition;
        public int X;
        public int Y;

        public GridNode(bool isWalkable, float3 worldPosition, int x, int y, int movementPenalty)
        {
            IsWalkable = isWalkable;
            WorldPosition = worldPosition;
            X = x;
            Y = y;
            MovementPenalty = movementPenalty;
        }
    }
}
