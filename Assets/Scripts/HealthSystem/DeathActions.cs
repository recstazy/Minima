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
        healthSystem.OnDeath += PerformDeathActions;
    }

    private void OnDestroy()
    {
        healthSystem.OnDeath -= PerformDeathActions;
    }

    protected virtual void PerformDeathActions()
    {
        Destroy(healthSystem.Owner.gameObject);
    }
}
