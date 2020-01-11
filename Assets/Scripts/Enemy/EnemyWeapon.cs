using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour, IEnemyTargetable
{
    #region Fields

    [SerializeField]
    private WeaponComponent weapon;

    #endregion

    #region Properties

    #endregion

    public void UpdateTargets(List<DamageTarget> targets)
    {
        ITargetable targetable = weapon as ITargetable;
        targetable.SetTarget(targets[0]);
    }

    public void Shoot()
    {
        weapon.UseWeapon();
    }
}
