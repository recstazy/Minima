using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.LevelGeneration
{
    public class CornerPoints : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private GameObject cornerPrefab;

        [SerializeField]
        private List<WallCorner> corners = new List<WallCorner>();

        [SerializeField]
        private bool randomizeCorners = true;

        [SerializeField]
        private float maxOffset = 3f;

        [SerializeField]
        private float maxOffsetFromCenter = 3f;

        #endregion

        #region Properties

        public List<WallCorner> Corners { get => corners; }
        
        #endregion

        public void Initialize()
        {
            foreach (var c in corners)
            {
                c.BindPrevious();
            }

            if (randomizeCorners)
            {
                OffsetCorners();
            }
        }

        public WallCorner CreateNewCorner(Vector2 position)
        {
            var newCorner = Instantiate(cornerPrefab, position, Quaternion.identity, this.transform);
            WallCorner corner = newCorner.GetComponent<WallCorner>();
            corners.Add(corner);
            return corner;
        }

        private void OffsetCorners()
        {
            foreach (var c in corners)
            {
                DragCorner(c);
            }
        }

        private void DragCorner(WallCorner corner)
        {
            Vector2 centerDirection = -corner.localPosition;
            Vector2 rawOffset = GetAvailableOffsetDirection(centerDirection) * Random.Range(0f, maxOffsetFromCenter);

            Vector2 offset = (centerDirection + rawOffset).normalized * Random.Range(0f, maxOffset);
            Vector2 newPosition = corner.position + new Vector3(offset.x, offset.y, corner.position.z);
            corner.position = newPosition;
        }

        public Vector2 GetAvailableOffsetDirection(Vector2 centerDirection)
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
