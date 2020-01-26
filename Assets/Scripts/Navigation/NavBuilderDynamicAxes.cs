using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public class NavBuilderDynamicAxes : NavMeshBuilder
    {
        #region Fields

        [SerializeField]
        private int pointsPerObject = 2;

        [SerializeField]
        private float boundCoefficient = 1f;

        [SerializeField]
        private float collapsePointsTreshhold = 0.3f;

        #endregion

        #region Properties

        #endregion

        protected override void InitializeGridAxes()
        {
            float iterationAxes;
            float staticAxes;
            float step;
            List<float> dynamicList;
            List<float> staticList;
            float posOffset;

            foreach (var o in obstacles)
            {
                var position = o.transform.localPosition;
                var extents = o.bounds.extents * boundCoefficient;

                if (extents.x > extents.y)
                {
                    iterationAxes = extents.x;
                    staticAxes = position.y;
                    step = extents.x * 2 / pointsPerObject;
                    dynamicList = xAxes;
                    staticList = yAxes;
                    posOffset = position.x;
                }
                else
                {
                    iterationAxes = extents.y;
                    staticAxes = position.x;
                    step = extents.y * 2 / pointsPerObject;
                    dynamicList = yAxes;
                    staticList = xAxes;
                    posOffset = position.y;
                }

                for (float axes = -iterationAxes; axes <= iterationAxes; axes += step)
                {
                    dynamicList.Add(posOffset + axes);
                }

                staticList.Add(staticAxes);
            }

            xAxes.Sort();
            yAxes.Sort();

            CollapsePoints();
        }

        private void CollapsePoints()
        {
            xAxes = CollapseInList(xAxes);
            yAxes = CollapseInList(yAxes);
        }

        private List<float> CollapseInList(List<float> list)
        {
            List<float> listCopy = list.ToList();

            for (int i = 0; i < listCopy.Count - 1; i++)
            {
                var current = listCopy[i];
                var next = listCopy[i + 1];

                if (next - current <= collapsePointsTreshhold)
                {
                    listCopy[i] = (current + next) / 2;
                    listCopy.Remove(next);
                }
            }

            return listCopy;
        }
    }
}
