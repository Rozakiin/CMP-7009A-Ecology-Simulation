using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public int gridX;// X position in Node Array
    public int gridY;// Y position in Node Array

    public bool isWalkable;// Is node obstructed
    public int penalty; //penaty for walking the node
    public Vector3 position;// World position of node

    public Node parentNode;// For A* algo, store previous node came from to trace path

    public int gCost;// Cost to move to next node
    public int hCost;// Distance to end from this node

    public int FCost { get { return gCost + hCost; } }//Total Cost of the Node

    protected int heapIndex;

    public Node(bool _isWalkable, Vector3 _position, int _gridX, int _gridY, int _penalty)// Constructor for Node
    {
        isWalkable = _isWalkable;
        position = _position;
        gridX = _gridX;
        gridY = _gridY;
        penalty = _penalty;
    }

    public int HeapIndex 
    {
        get 
        {
            return heapIndex;
        }
        set 
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);
        if (compare == 0) //tiebreaker
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;//higher priority is lower cost
    }
}
