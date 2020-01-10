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
                if (IsTriangleSpawnable(c))
                {
                    var newPoint = StaticHelpers
                        .GetTriangleCenter(c.position, c.PreviousCorner.position, c.NextCorner.position, true);

                    if (IsPointInRoom(newPoint))
                    {
                        point = newPoint;
                        break;
                    }
                }
            }

            return point;
        }

        private bool IsTriangleSpawnable(WallCorner corner)
        {
            float ab = (corner.NextCorner.position - corner.position).magnitude;
            float bc = (corner.PreviousCorner.position - corner.NextCorner.position).magnitude;
            float ac = (corner.PreviousCorner.position - corner.position).magnitude;

            float p = 0.5f * (ab + bc + ac);
            float radius = Mathf.Sqrt(((p - ab) * (p - bc) * (p - ac)) / p); // triangle inner circle radius

            if (radius > spawnableRadius)
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
            return StaticHelpers.CheckVisibility(point.ToVector3(), ThisRoom.transform.position, true);
        }
    }
}
