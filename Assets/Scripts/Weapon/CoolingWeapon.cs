using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolingWeapon : WeaponComponent
{
    #region Fields

    [SerializeField]
    protected float coolDownTime = 0.5f;

    protected bool canFire = true;

    #endregion

    public override void UseWeapon()
    {
        if (canFire)
        {
            Use();
            StartCoroutine(CoolDown());
        }
    }

    protected virtual void Use()
    {

    }

    protected virtual IEnumerator CoolDown()
    {
        canFire = false;
        yield return new WaitForSeconds(coolDownTime);
        canFire = true;
    }
}
