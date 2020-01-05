using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private InputHandler inputHandler;

    [SerializeField]
    private WeaponComponent weapon;

    #endregion

    #region Properties
    
    #endregion

    void Start()
    {
        inputHandler.OnActionPressed += UseWeapon;
    }

    private void OnDestroy()
    {
        inputHandler.OnActionPressed -= UseWeapon;
    }

    protected virtual void UseWeapon()
    {
        weapon.UseWeapon();
    }
}
