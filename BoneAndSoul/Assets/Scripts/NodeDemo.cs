using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/*
    Name:       Christian Schildhauer
    Date:       30.11.2016
    Function:   Set and define nodes
*/

public class NodeDemo {

    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;
    public int gCost;
    public int hCost;
    public NodeDemo parent;


    public NodeDemo (bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }
 
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }   
}
