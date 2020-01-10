using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IDamageble
{
    #region Fields

    [SerializeField]
    private HealthSystem healthSystem;

    #endregion

    #region Properties

    #endregion

    protected virtual void Awake()
    {
        healthSystem.Owner = this;
    }

    public void ApplyDamage(float amount)
    {
        healthSystem.ApplyDamage(amount);
    }
}
