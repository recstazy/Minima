using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MovementComponent
{
    #region Fields

    [SerializeField]
    private Transform currentTarget;

    private Transform thisTransform;
    
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
        MoveToTarget();
        base.Update();
    }

    public void MoveToTarget(Transform target)
    {
        currentTarget = target;
        MoveToTarget();
    }

    public void MoveToTarget()
    {
        if (currentTarget != null)
        {
            var direction = currentTarget.position - thisTransform.position;
            MoveOnDirection(direction);
        }
    }

    public override void StopMoving()
    {
        SetCanMove(false);
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
