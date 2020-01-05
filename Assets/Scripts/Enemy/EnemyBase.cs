using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private List<string> targetTags = new List<string>();

    #endregion

    #region Properties

    #endregion

    private void Awake()
    {
        var targetable = GetComponentsInChildren<IEnemyTargetable>(true);
        foreach (var t in targetable)
        {
            t.UpdateTargets(targetTags);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
