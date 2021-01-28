using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace MonoBehaviourTools.Grid
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance; //reference to self - singleton
        private EntityManager _entityManager;

        [Header("Debugging")]
        [SerializeField] private bool _displayGridGizmos; //used in debugging, shows walkable and non walkable nodes in scene

        [Header("Grid Data")]
        public float2 GridWorldSize;//A vector2 to store the width and height of the graph in world units.
        public int2 GridSize;//Size of the Grid in Array units.
        public int GridMaxSize => GridSize.x * GridSize.y;

        [Header("Node Properties")]
        [SerializeField] private float _gridNodeRadius;//This stores how big each square on the graph will be
        [SerializeField] private float _distanceBetweenGridNodes;//The distance that the gizmo squares will spawn from each other.
        public float GridNodeDiameter;//Twice the amount of the radius (Set in the start function)
        public GridNode[,] Grid;//The array of nodes that the A Star algorithm uses.

        [Header("LayerMask Data")] 
        [SerializeField] private int _unwalkableProximityPenalty; //Penalty for going near to unwalkable nodes

        private int _penaltyMin = int.MaxValue;
        private int _penaltyMax = int.MinValue;


        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            GridNodeDiameter = _gridNodeRadius * 2;//Double the radius to get diameter
        }

        /*Methods to create the pathfinding grid from the map, assign a movement penalty based on if is walkable and terrain penalty*/
        public bool TrySetupGrid()
        {
            if (!TryCreateGrid()) return false;

            BlurGridMovementPenalty(3);//blur the map with a kernel extent of 3 (5*5)
            return true;
        }
        
        private bool TryCreateGrid()
        {
            GridWorldSize = SimulationManager.WorldSize;
            //Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.
            GridSize.x = (int)math.round(GridWorldSize.x / GridNodeDiameter);
            GridSize.y = (int)math.round(GridWorldSize.y / GridNodeDiameter);
            Grid = new GridNode[GridSize.x, GridSize.y];

            var worldBottomLeft = SimulationManager.WorldBottomLeft;

            for (var x = 0; x < GridSize.x; x++)
            for (var y = 0; y < GridSize.y; y++)
            {
                var worldPoint = worldBottomLeft + new float3(x * GridNodeDiameter + _gridNodeRadius, 0, y * GridNodeDiameter + _gridNodeRadius);

                var tempUp = worldPoint + new float3(0, 100000, 0);
                var tempDown = worldPoint + new float3(0, -100000, 0);
                var tempTileFilter = new CollisionFilter
                {
                    BelongsTo = ~0u, 
                    CollidesWith = 1 >> 0, //filter to only collide with tiles
                    GroupIndex = 0
                }; 

                //raycast from really high point to under map, colliding with only tiles
                var collidedEntity = UtilTools.PhysicsTools.GetEntityFromRaycast(tempUp, tempDown, tempTileFilter);

                if (_entityManager.HasComponent<TerrainTypeData>(collidedEntity))
                {
                    var movementPenalty = _entityManager.GetComponentData<TerrainTypeData>(collidedEntity).TerrainPenalty; //penalty for walking over node
                    var isWalkable = _entityManager.GetComponentData<TerrainTypeData>(collidedEntity).IsWalkable;
                    if (!isWalkable)
                        movementPenalty += _unwalkableProximityPenalty;

                    Grid[x, y] = new GridNode(isWalkable, worldPoint, x, y, movementPenalty); //Create a new node in the array.
                }
                else return false;
            }

            return true;
        }

        /*
         * Blurs a map of the movement penalty for each node using a box blur,
         * this is used to smooth the penalty weights for more natural pathfinding
         */
        private void BlurGridMovementPenalty(int blurSize)
        {
            var kernelSize = blurSize * 2 + 1; //must be odd number
            var kernelExtents = blurSize; // number of squares between centre and edge of kernel

            var penaltiesHorizontal = new int[GridSize.x, GridSize.y]; //temp array to store horizontal pass over the penalty map
            var penaltiesVertical = new int[GridSize.x, GridSize.y]; //temp array to store vertical pass over the penalty map

            //horizontal pass
            //fill the penaltiesHorizontal array with the sum of movement penalties stored in nodeArray covered by the kernel
            for (var y = 0; y < GridSize.y; y++)
            {
                //loop through nodes in kernel and sum them up
                for (var x = -kernelExtents; x <= kernelExtents; x++)
                {
                    var sampleX = Mathf.Clamp(x, 0, kernelExtents); //clamp so take value from first node rather than out of bounds
                    penaltiesHorizontal[0, y] += Grid[sampleX, y].MovementPenalty;//add the node penalty value to penaltiesHorizontal
                }

                //loop over all remaining columns in the row
                for (var x = 1; x < GridSize.x; x++)
                {
                    var indexToRemove = Mathf.Clamp(x - kernelExtents - 1, 0, GridSize.x);//calc index of node that is no longer inside kernel after kernel moved along 1
                    var indexToAdd = Mathf.Clamp(x + kernelExtents, 0, GridSize.x - 1);//calc index of node that is now inside kernel after kernel moved along 1
                    penaltiesHorizontal[x, y] = penaltiesHorizontal[x - 1, y] - Grid[indexToRemove, y].MovementPenalty + Grid[indexToAdd, y].MovementPenalty;//equal to previous - penalty at indexToRemove + penalty at indexToAdd
                }
            }

            //vertical pass
            //fill the penaltiesVertical array with the sum of movement penalties stored in nodeArray covered by the kernel
            for (var x = 0; x < GridSize.x; x++)
            {
                //loop through nodes in kernel and sum them up
                for (var y = -kernelExtents; y <= kernelExtents; y++)
                {
                    var sampleY = Mathf.Clamp(y, 0, kernelExtents); //clamp so take value from first node rather than out of bounds
                    penaltiesVertical[x, 0] += penaltiesHorizontal[x, sampleY];//sample the penalty from the horizontal pass array
                }

                //blur bottom row
                var blurredPenalty = Mathf.RoundToInt((float)penaltiesVertical[x, 0] / (kernelSize * kernelSize));//average the penalty and round to nearest int
                Grid[x, 0].MovementPenalty = blurredPenalty;//set the penalty in the nodeArray to the new blurred penalty

                //loop over all remaining rows in the column
                for (var y = 1; y < GridSize.y; y++)
                {
                    var indexToRemove = Mathf.Clamp(y - kernelExtents - 1, 0, GridSize.y);//calc index of node that is no longer inside kernel after kernel moved along 1
                    var indexToAdd = Mathf.Clamp(y + kernelExtents, 0, GridSize.y - 1);//calc index of node that is now inside kernel after kernel moved along 1
                    penaltiesVertical[x, y] = penaltiesVertical[x, y - 1] - penaltiesHorizontal[x, indexToRemove] + penaltiesHorizontal[x, indexToAdd];//equal to previous - penalty at indexToRemove + penalty at indexToAdd
                    blurredPenalty = Mathf.RoundToInt((float)penaltiesVertical[x, y] / (kernelSize * kernelSize));//average the penalty and round to nearest int
                    Grid[x, y].MovementPenalty = blurredPenalty;//set the penalty in the nodeArray to the new blurred penalty

                    UpdatePenaltyMinMax(blurredPenalty);
                }
            }
        }

        private void UpdatePenaltyMinMax(int blurredPenalty)
        {
            if (blurredPenalty > _penaltyMax)
                _penaltyMax = blurredPenalty;
            if (blurredPenalty < _penaltyMin)
                _penaltyMin = blurredPenalty;
        }

        /*Function that draws the wireframe, and the nodes*/
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y));//Draw a wire cube with the given dimensions from the Unity inspector

            if (Grid != null && _displayGridGizmos)//If the grid is not empty and displayGridGizmos is true
            {
                foreach (var n in Grid)//Loop through every node in the grid
                {
                    Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(_penaltyMin, _penaltyMax, n.MovementPenalty));//pick color on gradient between white and black depending on where penalty lies between min and max
                    if (!n.IsWalkable)//If the current node is not walkable
                    {
                        Gizmos.color = Color.red;//node colour red
                    }
                    Gizmos.DrawCube(n.WorldPosition, Vector3.one * (GridNodeDiameter - _distanceBetweenGridNodes));//Draw the node at the position of the node.
                }
            }
        }
    }
}
