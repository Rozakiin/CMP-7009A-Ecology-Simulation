using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //for array reversal

public class PathFinder : MonoBehaviour
{
    PathRequestManager requestManager;
    PathFinderController grid;

    // Awake is called when program starts
    private void Awake()
    {
        grid = GetComponent<PathFinderController>();//Get a reference to PathFinderController
        requestManager = GetComponent<PathRequestManager>();//Get a reference to PathRequestManager
    }

    //starts the FindPath co-routine
    public void StartFindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        StartCoroutine(FindPath(startPosition, targetPosition));
    }

    IEnumerator FindPath(Vector3 _startPosition, Vector3 _targetPosition)
    {

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(_startPosition);//Gets the node closest to the starting position
        Node targetNode = grid.NodeFromWorldPoint(_targetPosition);//Gets the node closest to the target position

        //Only path find if start and end are walkable
        if (startNode.isWalkable && targetNode.isWalkable)
        {
            Heap<Node> openList = new Heap<Node>(grid.MaxSize);//List of nodes for the open list
            HashSet<Node> closedList = new HashSet<Node>();//Hashset of nodes for the closed list

            openList.Add(startNode);//Add the starting node to the open list to begin the program

            while(openList.Count > 0)//Whilst there is something in the open list
            {
                Node currentNode = openList.RemoveFirst();//Create a node and set it to the first item in the open list
                closedList.Add(currentNode);//And add it to the closed list

                //Found the Path!
                if (currentNode == targetNode)//If the current node is the same as the target node
                {
                    pathSuccess = true; // path was sucessful
                    break;
                }

                foreach (Node neighbourNode in grid.GetNeighboringNodes(currentNode))//Loop through each neighbor of the current node
                {
                    if (!neighbourNode.isWalkable || closedList.Contains(neighbourNode))//If the neighbor is a wall or has already been checked
                    {
                        continue;//Skip it
                    }

                    int moveCost = currentNode.gCost + GetManhattenDistance(currentNode, neighbourNode) + neighbourNode.penalty;//Get the total cost of that neighbor
                    if (moveCost < neighbourNode.gCost || !openList.Contains(neighbourNode))//If the total cost is greater than the g cost or it is not in the open list
                    {
                        neighbourNode.gCost = moveCost;//Set the g cost to the total cost
                        neighbourNode.hCost = GetManhattenDistance(neighbourNode, targetNode);//Set the h cost
                        neighbourNode.parentNode = currentNode;//Set the parent of the node for retracing steps

                        if(!openList.Contains(neighbourNode))//If the neighbor is not in the openList
                        {
                            openList.Add(neighbourNode);//Add it to the list
                        }
                        else
                        {
                            openList.UpdateItem(neighbourNode);// value changed so must be updated 
                        }
                    }
                }
            }
        }
        yield return null;//wait for 1 frame before returning
        if (pathSuccess)
        {
            waypoints = GetFinalPath(startNode, targetNode);//Calculate the final path return path
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess); // Tell requestmanager if path was success with path found
    }

    // Retrace path through the nodes
    Vector3[] GetFinalPath(Node _startingNode, Node _endNode)
    {
        List<Node> finalPath = new List<Node>();//List to hold the path sequentially 
        Node currentNode = _endNode;//Node to store the current node being checked

        while(currentNode != _startingNode)//While loop to work through each node going through the parents to the beginning of the path
        {
            finalPath.Add(currentNode);//Add that node to the final path
            currentNode = currentNode.parentNode;//Move onto its parent node
        }
        if (currentNode == _startingNode)
        {
            finalPath.Add(currentNode);
        }

        Vector3[] waypoints = SimplifyPath(finalPath); //simplify the final path
        Array.Reverse(waypoints);//Reverse the path to get the correct order
        return waypoints;//return the simplified final path
    }

    //Simplifies the path so the path only contains changes to the direction
    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero; // store the direction of last 2 nodes

        //checks for changes to direction in the path, only adds nodes that have differing direction to previous
        for (int i = 1; i < path.Count; i++){
            Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY); // new direction still 0 if no change in direction
            if(directionNew != directionOld)
            {
                waypoints.Add(path[i-1].position);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    // if using NESW only
    int GetManhattenDistance(Node _nodeA, Node _nodeB)
    {
        int x = Mathf.Abs(_nodeA.gridX - _nodeB.gridX);//x1-x2
        int y = Mathf.Abs(_nodeA.gridY - _nodeB.gridY);//y1-y2

        return x + y;//Return the sum
    }

    // if using diagonals
    int GetDistance(Node _nodeA, Node _nodeB)
    {
        int x = Mathf.Abs(_nodeA.gridX - _nodeB.gridX);//x1-x2
        int y = Mathf.Abs(_nodeA.gridY - _nodeB.gridY);//y1-y2
        if (x > y)
        {
            return 14*y + 10*(x-y);//14 used as approximate of sqrt(2)*10 for diagonals
        }
        return 14*x + 10*(y-x);
    }
}
