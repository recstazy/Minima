using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private WeaponComponent weapon;

    #endregion

    #region Properties

    #endregion

    public void Shoot()
    {
        weapon.UseWeapon();
    }
}
