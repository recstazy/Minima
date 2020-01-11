using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingAreaWeapon : AreaWeapon
{
    #region Fields

    [SerializeField]
    private float hpPerEnemy = 5f;

    #endregion

    #region Properties

    #endregion

    protected override List<IDamagable> DamageArea()
    {
        var damaged = base.DamageArea();
        
        foreach (var d in damaged)
        {
            d.HealthSystem.OnDeath += TargetDead;
        }

        return damaged;
    }

    private void TargetDead(Character killer, Character victim)
    {
        if (killer == Owner)
        {
            Owner.HealthSystem.Restore(hpPerEnemy);
            victim.HealthSystem.OnDeath -= TargetDead;
        }
    }
}
