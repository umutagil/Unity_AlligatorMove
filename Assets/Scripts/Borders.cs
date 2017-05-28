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
