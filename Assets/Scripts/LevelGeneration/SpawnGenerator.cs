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
        private float pointOffset = 3f;

        private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
        private EnemiesParent enemiesParent;

        #endregion

        #region Properties

        public RoomDraft ThisRoom { get; private set; }

        #endregion

        public void Initialize(RoomDraft thisRoom)
        {
            ThisRoom = thisRoom;
            enemiesParent = FindObjectOfType<EnemiesParent>();
        }

        public void GeneratePoints()
        {
            if (ThisRoom.IsStartRoom)
            {
                Destroy(this.gameObject);
                return;
            }

            for (int i = 0; i < spawnPointsCount; i++)
            {
                var point = CreatePoint(GetPointPosition());
                point.Initialize(enemiesParent);
                point.Spawn();
            }
        }

        private SpawnPoint CreatePoint(Vector2 position)
        {
            var point = Instantiate(spawnPointPrefab, position, Quaternion.identity, this.transform);
            var spawnPoint = point.GetComponent<SpawnPoint>();
            spawnPoints.Add(spawnPoint);

            return spawnPoint;
        }

        private Vector2 GetPointPosition()
        {
            var corner = ThisRoom.GetRandomCorner();
            Vector3 centerDirection = (ThisRoom.transform.position - corner.position).normalized;

            return corner.position + (centerDirection * pointOffset);
        }
    }
}
