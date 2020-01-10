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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damagable = collision.GetComponent<IDamageble>();

        if (damagable != null)
        {
            damagable.HealthSystem.OnDeath += TargetDead;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var damagable = collision.GetComponent<IDamageble>();

        if (damagable != null)
        {
            damagable.HealthSystem.OnDeath -= TargetDead;
        }
    }

    private void TargetDead()
    {
        Owner.HealthSystem.Restore(hpPerEnemy);
    }
}
