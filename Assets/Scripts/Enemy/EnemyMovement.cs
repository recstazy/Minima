using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MovementComponent
{
    #region Fields

    [SerializeField]
    private Transform currentTarget;

    [SerializeField]
    private AgressionTrigger agressionTrigger;

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
        agressionTrigger.OnTargetTriggered += TargetTriggered;
    }

    private void OnDestroy()
    {
        agressionTrigger.OnTargetTriggered -= TargetTriggered;
    }

    override protected void Update()
    {
        if (canMove)
        {
            MoveToTarget();
        }
        
        base.Update();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == currentTarget)
        {
            canMove = false;
            StopMoving();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform == currentTarget)
        {
            canMove = true;
        }
    }

    private void TargetTriggered(GameObject target)
    {
        currentTarget = target.transform;
        canMove = true;
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
