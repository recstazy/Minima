using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IDamagable
{
    #region Fields

    [SerializeField]
    private HealthSystem healthSystem;

    #endregion

    #region Properties

    public HealthSystem HealthSystem { get => healthSystem; }

    #endregion

    protected virtual void Awake()
    {
        healthSystem.Owner = this;
    }

    public void ApplyDamage(float amount, Character from = null)
    {
        healthSystem.ApplyDamage(amount, from);
    }
}
