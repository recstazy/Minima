using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public struct DijkstraNode
    {
        #region Properties

        public NavPoint Point { get; set; }
        public float Weight { get; set; }
        public NavPoint Source { get; set; }
        public bool IsValid { get; set; }

        #endregion

        public DijkstraNode(NavPoint point, float weight)
        {
            Point = point;
            Weight = weight;
            Source = new NavPoint();
            IsValid = true;
        }

        public void SetWeightAndSource(float weight, NavPoint source)
        {
            SetWeight(weight);
            SetSource(source);
        }

        public void SetSource(NavPoint source)
        {
            Source = source;
        }

        public void SetWeight(float value)
        {
            Weight = value;
        }
    }
}
