using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementShootDependent : EnemyMovement
{
    #region Fields

    [SerializeField]
    private WeaponComponent weapon;

    protected bool isShooting = false;

    #endregion

    #region Properties

    #endregion

    private void Awake()
    {
        weapon.OnWeaponUsed += WeaponUsed;
    }

    protected override void SetCanMove(bool newCanMove)
    {
        if (!isShooting)
        {
            base.SetCanMove(newCanMove);
        }
    }

    protected virtual void WeaponUsed()
    {
    }
}
