using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : Character
{
    #region Fields

    [SerializeField]
    private List<DamageTarget> targets = new List<DamageTarget>();

    [SerializeField]
    private bool autoTargetPlayer = true;

    #endregion

    #region Properties

    #endregion

    protected override void Awake()
    {
        base.Awake();

        if (autoTargetPlayer)
        {
            SetPlayerAsTarget();
        }
        else
        {
            UpdateTargets();
        }
    }

    public void AddTarget(DamageTarget target)
    {
        targets.Add(target);
        UpdateTargets();
    }

    protected virtual void UpdateTargets()
    {
        var targetable = GetComponentsInChildren<IEnemyTargetable>(true);
        foreach (var t in targetable)
        {
            t.UpdateTargets(targets);
        }
    }

    private void SetPlayerAsTarget()
    {
        AddTarget(DamageTarget.Player);
    }
}
