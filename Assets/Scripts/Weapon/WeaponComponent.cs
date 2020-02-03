using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType
{
    Player = 8,
    Enemy = 9,
}

public class WeaponComponent : MonoBehaviour
{
    public event System.Action OnWeaponUsed;

    #region Fields

    [SerializeField]
    private Character owner;

    #endregion

    #region Properties

    public Character Owner { get => owner; set => owner = value; }

    #endregion

    public virtual void UseWeapon()
    {
        CallWeaponUsed();
    }

    protected void CallWeaponUsed()
    {
        OnWeaponUsed?.Invoke();
    }
}
