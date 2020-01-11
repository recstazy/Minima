using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathActions : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private HealthSystem healthSystem;

    #endregion

    #region Properties
    
    #endregion

    void Start()
    {
        healthSystem.OnDeath += TargetDead;
    }

    private void OnDestroy()
    {
        healthSystem.OnDeath -= TargetDead;
    }

    protected virtual void TargetDead(Character target, Character victim)
    {
        PerformDeathActions();
    }

    protected virtual void PerformDeathActions()
    {
        Destroy(healthSystem.Owner.gameObject);
    }
}
