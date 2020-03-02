using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.LevelGeneration
{
    public class SpawnGenerator : MonoBehaviour
    {
        private enum GetPositionMethod { RoomCorner, Raycast }

        #region Fields

        [SerializeField]
        private List<SpawnParams> spawnParams;

        [SerializeField]
        private GameObject spawnPointPrefab;

        [SerializeField]
        private float spawnableRadius = 2f;

        private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
        private EnemiesParent enemiesParent;
        private Rect spawnableArea;

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
            ExecuteNextFrame(() => GeneratePointsImmediately());
        }

        private void GeneratePointsImmediately()
        {
            foreach (var p in spawnParams)
            {
                for (int i = 0; i < p.Count; i++)
                {
                    var point = CreatePoint(GetPointPosition(GetPositionMethod.Raycast));

                    if (point != null)
                    {
                        point.Initialize(enemiesParent);
                        point.AddToSpawnParams(new SpawnParams(p.Prefab));
                        point.Spawn();
                    }
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

        private Vector2 GetPointPosition(GetPositionMethod method)
        {
            switch (method)
            {
                case GetPositionMethod.RoomCorner:
                    {
                        return GetPositionByRoomCorner();
                    }
                case GetPositionMethod.Raycast:
                    {
                        return GetPositionByRaycast();
                    }
                default: throw new System.NotImplementedException("No such case: " + method);
            }
        }

        private Vector2 GetPositionByRaycast()
        {
            Vector2 origin = ThisRoom.transform.position;
            Vector2 direction = Helpers.RandomVectorNormalized();
            Vector2 spawnPosition = origin;

            var hits = new List<RaycastHit2D>();
            var filter = new ContactFilter2D();
            filter.useLayerMask = true;
            filter.layerMask = LayerMask.GetMask("Obstacles", "Exits");
            filter.useTriggers = true;

            int hitsCount = Physics2D.Raycast(origin, direction, filter, hits);

            if (hitsCount > 0)
            {
                var hit = hits[0];
                spawnPosition += (hit.point - origin) / 2;
            }

            return spawnPosition;
        }

        private Vector2 GetPositionByRoomCorner()
        {
            var corners = ThisRoom.Corners;
            corners.Shuffle();
            Vector2 point = Vector2.zero;

            foreach (var c in corners)
            {
                float radius = 0f;
                if (IsTriangleSpawnable(c, out radius))
                {
                    var newPoint = Helpers
                        .GetTriangleCenter(c.position, c.PreviousCorner.position, c.NextCorner.position);

                    if (IsPointInRoom(newPoint))
                    {
                        point = Helpers.RandomPointInRadius(newPoint, radius);
                        break;
                    }
                }
            }

            return point;
        }

        private bool IsTriangleSpawnable(WallCorner corner, out float innerRadius)
        {
            innerRadius = Helpers.GetInnerTriangleRadius(
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
            return Helpers.CheckVisibility(point.ToVector3(), ThisRoom.transform.position);
        }

        protected void ExecuteNextFrame(System.Action method)
        {
            StartCoroutine(WaitNextFrameAndExecute(method));
        }

        private IEnumerator WaitNextFrameAndExecute(System.Action method)
        {
            yield return new WaitForEndOfFrame();
            method?.Invoke();
        }
    }
}
