using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.LevelGeneration
{
    public class SpawnPoint : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private List<SpawnParams> spawnParams = new List<SpawnParams>();

        [SerializeField]
        private EnemiesParent enemiesParent;

        Dictionary<EnemyBase, int> spawnParamsCached;

        List<EnemyBase> spawned = new List<EnemyBase>();

        #endregion

        #region Properties

        public Dictionary<EnemyBase, int> SpawnParams
        {
            get
            {
                if (spawnParamsCached == null)
                {
                    spawnParamsCached = new Dictionary<EnemyBase, int>();

                    foreach (var p in spawnParams)
                    {
                        spawnParamsCached.Add(p.Prefab.GetComponent<EnemyBase>(), p.Count);
                    }
                }

                return spawnParamsCached;
            }
        }

        #endregion

        public void Initialize(EnemiesParent enemiesParent)
        {
            this.enemiesParent = enemiesParent;
        }

        public virtual void Spawn()
        {
            foreach (var p in spawnParams)
            {
                for (int i = 0; i < p.Count; i++)
                {
                    var enemy = InstantiateEnemy(p.Prefab);
                    spawned.Add(enemy);
                }
            }
        }

        protected EnemyBase InstantiateEnemy(GameObject prefab)
        {
            var enemy = Instantiate(prefab, this.transform.position, Quaternion.identity, enemiesParent.transform);
            return enemy.GetComponent<EnemyBase>();
        }
    }
}
