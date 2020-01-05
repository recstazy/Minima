using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathActions : DeathActions
{
    #region Fields

    [SerializeField]
    private ParticleSystem deathVFX;

    #endregion

    protected override void PerformDeathActions()
    {
        deathVFX.transform.parent = null;
        deathVFX.Play();

        base.PerformDeathActions();
    }
}
