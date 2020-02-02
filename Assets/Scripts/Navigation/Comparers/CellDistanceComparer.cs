using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Minima.Navigation;

public class CellDistanceComparer : IComparer<NavCell>
{
    public Vector2 Origin { get; set; }

    public CellDistanceComparer(Vector2 origin)
    {
        Origin = origin;
    }

    public int Compare(NavCell x, NavCell y)
    {
        float magnitudeX = (x.Center - Origin).sqrMagnitude;
        float magnitudeY = (y.Center - Origin).sqrMagnitude;
        
        if (magnitudeX > magnitudeY)
        {
            return 1;
        }
        else if (magnitudeX == magnitudeY)
        {
            return 0;
        }
        else
        {
            return -1;
        }
    }
    
}
