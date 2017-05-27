using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Borders
{

    public Rect rect;

    public Borders(List<Vector3> cornerPosList)
    {
        float maxX = Mathf.NegativeInfinity;
        float maxZ = Mathf.NegativeInfinity;
        float minX = Mathf.Infinity;
        float minZ = Mathf.Infinity;

        for(int i = 0; i < cornerPosList.Count; i++)
        {
            if (cornerPosList[i].x > maxX)
                maxX = cornerPosList[i].x;
            if (cornerPosList[i].x < minX)
                minX = cornerPosList[i].x;
            if (cornerPosList[i].z > maxZ)
                maxZ = cornerPosList[i].z;
            if (cornerPosList[i].z < minZ)
                minZ = cornerPosList[i].z;
        }

        rect = new Rect(new Vector2(minX, minZ), new Vector2(maxX - minX, maxZ - minZ));
    }

    public bool isPointInside(Vector3 point)
    {
        return rect.Contains(new Vector2(point.x, point.z));                   
    }

}
