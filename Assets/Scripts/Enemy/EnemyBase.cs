using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : Character
{
    #region Fields

    [SerializeField]
    private List<DamageTarget> targets = new List<DamageTarget>();

    #endregion

    #region Properties

    #endregion

    protected override void Awake()
    {
        base.Awake();

        var targetable = GetComponentsInChildren<IEnemyTargetable>(true);
        foreach (var t in targetable)
        {
            t.UpdateTargets(targets);
        }
    }
}
