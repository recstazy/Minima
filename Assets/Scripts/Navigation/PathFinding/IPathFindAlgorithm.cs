using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public interface IPathFindAlgorithm
    {
        NavPath FindPath(NavPoint a, NavPoint b);
    }
}
