using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Minima.Navigation;

public class TriangleDistanceComparer : IComparer<NavTriangle>
{
    public Vector2 Origin { get; set; }

    public int Compare(NavTriangle x, NavTriangle y)
    {
        Vector2 xCenter = StaticHelpers.GetTriangleCenter(x.A.Position, x.B.Position, x.C.Position);
        Vector2 yCenter = StaticHelpers.GetTriangleCenter(y.A.Position, y.B.Position, y.C.Position);

        float xMagnitude = Vector2.SqrMagnitude(xCenter - Origin);
        float yMagnitude = Vector2.SqrMagnitude(yCenter - Origin);

        if (xMagnitude > yMagnitude)
        {
            return 1;
        }
        else if (xMagnitude == yMagnitude)
        {
            return 0;
        }
        else
        {
            return -1;
        }
    }

    public TriangleDistanceComparer(Vector2 origin)
    {
        Origin = origin;
    }
}
