using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public class NavBuilderCostComparer : IComparer<NavMeshBuilder>
    {
        public Vector2 Origin { get; set; }
        public Vector2 Target { get; set; }

        public NavBuilderCostComparer(Vector2 origin, Vector2 target)
        {
            Target = target;
            Origin = origin;
        }

        public int Compare(NavMeshBuilder x, NavMeshBuilder y)
        {
            float costX = 
                (Target - x.transform.position.ToVector2()).sqrMagnitude * 
                (Origin - x.transform.position.ToVector2()).sqrMagnitude;

            float costY = 
                (Target - y.transform.position.ToVector2()).sqrMagnitude * 
                (Origin - y.transform.position.ToVector2()).sqrMagnitude;

            if (costY > costX)
            {
                return 1;
            }
            else if (costY == costX)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }
}
