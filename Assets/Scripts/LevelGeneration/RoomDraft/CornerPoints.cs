using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.LevelGeneration
{
    public class CornerPoints : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private List<Transform> corners = new List<Transform>();

        [SerializeField]
        private bool randomizeCorners = true;

        [SerializeField]
        private float maxOffset = 3f;

        [SerializeField]
        private float maxOffsetFromCenter = 3f;

        #endregion

        #region Properties

        public List<Transform> Corners { get => corners; }

        #endregion

        public void Initialize()
        {
            if (randomizeCorners)
            {
                OffsetCorners();
            }
        }

        private void OffsetCorners()
        {
            foreach (var c in corners)
            {
                DragCorner(c);
            }
        }

        private void DragCorner(Transform corner)
        {
            Vector2 centerDirection = -corner.localPosition;
            Vector2 rawOffset = GetAvailableOffsetDirection(centerDirection) * Random.Range(0f, maxOffsetFromCenter);
            Vector2 offset = (centerDirection + rawOffset).normalized * Random.Range(0f, maxOffset);
            Vector2 newPosition = corner.position + new Vector3(offset.x, offset.y, corner.position.z);
            corner.position = newPosition;
        }

        private Vector2 GetAvailableOffsetDirection(Vector2 centerDirection)
        {
            bool useX = StaticHelpers.RandomBool();
            Vector2 result;

            if (useX)
            {
                result = new Vector2(centerDirection.x, 0f);
            }
            else
            {
                result = new Vector2(0f, centerDirection.y);
            }

            return result.normalized;
        }

        
    }
}
