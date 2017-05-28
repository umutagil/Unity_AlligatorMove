using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Borders
{
    public List<Vector3> cornerPosList = new List<Vector3>();
    public List<Line> lineIndices = new List<Line>();

    public Borders(List<Vector3> cornerPosList)
    {
        this.cornerPosList = cornerPosList;
        int cornerCount = cornerPosList.Count;
        for (int i = 0; i < cornerCount; i++)
        {
            Line newLine = new Line(i, (i + 1) % cornerCount);
            lineIndices.Add(newLine);
        }
    }

    //checks all the border lines if the new destination is out
    public bool IsDestinationOut(Vector3 currentPos, Vector3 nextDestPos)
    {
        Vector2 currentPosXZ = new Vector2(currentPos.x, currentPos.z);
        Vector2 nextDestPosXZ = new Vector2(nextDestPos.x, nextDestPos.z);

        for (int i = 0; i < lineIndices.Count; i++)
        {
            int p1Index = lineIndices[i].pos1;
            int p2Index = lineIndices[i].pos2;
            Vector3 p1 = cornerPosList[p1Index];
            Vector3 p2 = cornerPosList[p2Index];
            Vector2 p1XZ = new Vector2(p1.x, p1.z);
            Vector2 p2XZ = new Vector2(p2.x, p2.z);

            bool intersects = GeometryExtensions.FastLineSegmentIntersection(p1XZ, p2XZ, currentPosXZ, nextDestPosXZ);
            if (intersects)
                return true;
        }

        return false;
    }

}

public class Line
{
    public int pos1, pos2;

    public Line(int p1, int p2)
    {
        pos1 = p1;
        pos2 = p2;
    }
}
