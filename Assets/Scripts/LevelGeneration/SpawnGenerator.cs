using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.LevelGeneration
{
    public class SpawnGenerator : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private GameObject spawnPointPrefab;

        [SerializeField]
        private int spawnPointsCount = 3;

        [SerializeField]
        private float spawnableRadius = 2f;

        private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
        private EnemiesParent enemiesParent;

        #endregion

        #region Properties

        public RoomDraft ThisRoom { get; private set; }

        #endregion

        public void Initialize(RoomDraft thisRoom)
        {
            ThisRoom = thisRoom;

            if (ThisRoom.IsStartRoom)
            {
                Destroy(this.gameObject);
                return;
            }

            enemiesParent = FindObjectOfType<EnemiesParent>();
        }

        public void GeneratePoints()
        {
            for (int i = 0; i < spawnPointsCount; i++)
            {
                var point = CreatePoint(GetPointPosition());

                if (point != null)
                {
                    point.Initialize(enemiesParent);
                    point.Spawn();
                }
            }
        }

        private SpawnPoint CreatePoint(Vector2 position)
        {
            if (position != Vector2.zero)
            {
                var point = Instantiate(spawnPointPrefab, position, Quaternion.identity, this.transform);
                var spawnPoint = point.GetComponent<SpawnPoint>();
                spawnPoints.Add(spawnPoint);

                return spawnPoint;
            }

            return null;
        }

        private Vector2 GetPointPosition()
        {
            var corners = ThisRoom.Corners;
            corners.Shuffle();
            Vector2 point = Vector2.zero;

            foreach (var c in corners)
            {
                float radius = 0f;
                if (IsTriangleSpawnable(c, out radius))
                {
                    var newPoint = StaticHelpers
                        .GetTriangleCenter(c.position, c.PreviousCorner.position, c.NextCorner.position);

                    if (IsPointInRoom(newPoint))
                    {
                        point = StaticHelpers.RandomPointInRadius(newPoint, radius);
                        break;
                    }
                }
            }

            return point;
        }

        private bool IsTriangleSpawnable(WallCorner corner, out float innerRadius)
        {
            innerRadius = StaticHelpers.GetInnerTriangleRadius(
                corner.position, corner.NextCorner.position, corner.PreviousCorner.position);

            if (innerRadius > spawnableRadius)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsPointInRoom(Vector2 point)
        {
            return StaticHelpers.CheckVisibility(point.ToVector3(), ThisRoom.transform.position);
        }
    }
}
