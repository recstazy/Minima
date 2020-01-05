using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IDamageble
{
    #region Fields

    [SerializeField]
    private List<string> targetTags = new List<string>();

    [SerializeField]
    private HealthSystem healthSystem;

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

    public void ApplyDamage(float amount)
    {
        healthSystem.ApplyDamage(amount);
    }
}
