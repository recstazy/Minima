using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControlled : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private WeaponComponent weapon;

    [SerializeField]
    private TargetMovement movementComp;

    #endregion

    #region Fields

    public WeaponComponent Weapon { get => weapon; }
    public MovementComponent MovementComp { get => movementComp; }

    #endregion

    public virtual void MoveTo(Transform target)
    {
        if (movementComp != null)
        {
            movementComp.MoveToTarget(target);
        }
    }

    public void StopMovement()
    {
        movementComp.StopMoving();
    }

    public void ContinueMovement()
    {
        movementComp.SetCanMove(true);
    }

    public virtual void Shoot(Character target = null)
    {
        if (Weapon != null)
        {
            Weapon.UseWeapon();
        }
    }
}
