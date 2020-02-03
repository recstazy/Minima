using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MovementComponent
{
    public event System.Action OnTargetReached;

    #region Fields

    [SerializeField]
    protected Transform currentTarget;
    
    #endregion

    #region Properties

    public Transform CurrentTarget { get => currentTarget; }

    #endregion

    override protected void Update()
    {
        MoveToTarget();
        base.Update();
    }

    public virtual void MoveToTarget(Transform target)
    {
        currentTarget = target;
        MoveToTarget();
    }

    public virtual void MoveToTarget()
    {
        if (currentTarget != null)
        {
            var direction = currentTarget.position - thisTransform.position;
            MoveOnDirection(direction);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == currentTarget)
        {
            StopMoving();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform == currentTarget)
        {
            if (canMove)
            {
                MoveToTarget();
            }
        }
    }
}
