using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private List<SpawnParams> spawnParams = new List<SpawnParams>();

    Dictionary<EnemyBase, int> spawnParamsCached;

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
    
}
