using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.LevelGeneration
{
    public class ExitPoints : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private List<WallCorner> exits = new List<WallCorner>();

        [SerializeField]
        private bool randomizeExits = true;

        [SerializeField]
        private float maxOffset = 5f;

        #endregion

        #region Properties

        public List<WallCorner> Exits { get => exits; }

        #endregion

        public void Initialize()
        {
            if (randomizeExits)
            {
                OffsetExits();
            }
        }

        private void OffsetExits()
        {
            foreach (var e in exits)
            {
                DragExit(e);
            }
        }

        private void DragExit(WallCorner exit)
        {
            var offset = GetOffset(exit);
            Vector3 newPosition = exit.transform.position + new Vector3(offset.x, offset.y, exit.transform.position.z);
            exit.transform.position = newPosition;
        }

        private Vector2 GetOffset(WallCorner exit)
        {
            Vector2 direction;

            if (exit.position.x != 0f)
            {
                direction = Vector2.up;
            }
            else
            {
                direction = Vector2.right;
            }

            bool isNegative = StaticHelpers.RandomBool();

            if (isNegative)
            {
                direction *= -1;
            }

            float magnitude = Random.Range(0.1f, maxOffset);

            return direction * magnitude;
        }

        
    }
}
