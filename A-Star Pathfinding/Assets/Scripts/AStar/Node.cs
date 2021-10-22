using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : IHeapItem<Node>
{
    public bool walkable;

    public Vector3 worldPos;

    public int gCost;
    public int hCost;
    public int gridX;
    public int gridY;
    public int movementPenalty;

    public Node parent;

    int heapIndex;

    public Node(bool isWalkable, Vector3 worldPosition, int _gridX, int _gridY, int _penalty)
    {
        walkable = isWalkable;
        worldPos = worldPosition;
        gridX = _gridX;
        gridY = _gridY;
        movementPenalty = _penalty;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
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
        int compare = fCost.CompareTo(nodeToCompare.fCost);

        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return -compare;
    }
}
