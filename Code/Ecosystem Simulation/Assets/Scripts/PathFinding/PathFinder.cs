using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    PathFinderController grid;
    public Transform startPosition;
    public Transform targetPosition;

    // Awake is called when program starts
    private void Awake()
    {
        grid = GetComponent<PathFinderController>();//Get a reference to Simulation manager
    }

    // Update is called once per frame
    private void Update()
    {
        FindPath(startPosition.position, targetPosition.position);//Find a path to the goal
    }

    void FindPath(Vector3 _startPosition, Vector3 _targetPosition)
    {
        Node startNode = grid.NodeFromWorldPoint(_startPosition);//Gets the node closest to the starting position
        Node targetNode = grid.NodeFromWorldPoint(_targetPosition);//Gets the node closest to the target position

        // List is slow searching, to be optimised
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
                GetFinalPath(startNode, targetNode);//Calculate the final path
            }

            foreach (Node neighbourNode in grid.GetNeighboringNodes(currentNode))//Loop through each neighbor of the current node
            {
                if (!neighbourNode.isWalkable || closedList.Contains(neighbourNode))//If the neighbor is a wall or has already been checked
                {
                    continue;//Skip it
                }

                int moveCost = currentNode.gCost + GetManhattenDistance(currentNode, neighbourNode);//Get the F cost of that neighbor
                if (moveCost < neighbourNode.gCost || !openList.Contains(neighbourNode))//If the f cost is greater than the g cost or it is not in the open list
                {
                    neighbourNode.gCost = moveCost;//Set the g cost to the f cost
                    neighbourNode.hCost = GetManhattenDistance(neighbourNode, targetNode);//Set the h cost
                    neighbourNode.parentNode = currentNode;//Set the parent of the node for retracing steps

                    if(!openList.Contains(neighbourNode))//If the neighbor is not in the openList
                    {
                        openList.Add(neighbourNode);//Add it to the list
                    }
                }
            }
        }
    }

    // Retrace path through the nodes
    void GetFinalPath(Node _startingNode, Node _endNode)
    {
        List<Node> finalPath = new List<Node>();//List to hold the path sequentially 
        Node currentNode = _endNode;//Node to store the current node being checked

        while(currentNode != _startingNode)//While loop to work through each node going through the parents to the beginning of the path
        {
            finalPath.Add(currentNode);//Add that node to the final path
            currentNode = currentNode.parentNode;//Move onto its parent node
        }

        finalPath.Reverse();//Reverse the path to get the correct order

        grid.finalPath = finalPath;//Set the final path
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
