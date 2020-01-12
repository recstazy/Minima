using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MovementComponent
{
    #region Fields

    [SerializeField]
    private Transform currentTarget;

    private Transform thisTransform;
    private bool canMove = false;
    #endregion

    #region Properties

    public Transform CurrentTarget { get => currentTarget; }

    #endregion

    override protected void Start()
    {
        base.Start();

        thisTransform = transform;
    }

    override protected void Update()
    {
        if (canMove)
        {
            MoveToTarget();
        }
        
        base.Update();
    }

    public void MoveToTarget(Transform target)
    {
        currentTarget = target;
        SetCanMove(true);
        MoveToTarget();
    }

    protected virtual void SetCanMove(bool newCanMove)
    {
        canMove = newCanMove;

        if (!canMove)
        {
            StopMoving();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == currentTarget)
        {
            SetCanMove(false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform == currentTarget)
        {
            SetCanMove(true);
        }
    }

    private void MoveToTarget()
    {
        if (currentTarget != null)
        {
            var direction = currentTarget.position - thisTransform.position;
            MoveOnDirection(direction);
        }
    }
}
