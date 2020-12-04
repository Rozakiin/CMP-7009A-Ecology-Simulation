using Unity.Mathematics;
using Unity.Entities;

public struct GridNode
{
    public bool isWalkable;
    public int movementPenalty;

    public float3 worldPosition;
    public int x;
    public int y;

    public GridNode(bool _isWalkable, float3 _worldPosition, int _x, int _y, int _movementPenalty)
    {
        isWalkable = _isWalkable;
        worldPosition = _worldPosition;
        x = _x;
        y = _y;
        movementPenalty = _movementPenalty;
    }
}
