using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MovementComponent
{
    public event System.Action OnTargetReached;

    #region Fields

    [SerializeField]
    protected Transform currentTarget;

    [SerializeField]
    protected bool updateEveryFrame = true;

    protected Vector2 currentTargetPoint;
    
    #endregion

    #region Properties

    public Transform CurrentTarget { get => currentTarget; }

    #endregion

    override protected void Update()
    {
        if (updateEveryFrame && currentTarget != null)
        {
            currentTargetPoint = currentTarget.position;
        }
        
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
        if (currentTarget != null && currentTargetPoint != Vector2.zero)
        {
            var direction = currentTargetPoint - thisTransform.position.ToVector2();
            MoveOnDirection(direction);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentTarget != null)
        {
            if (collision.transform == currentTarget)
            {
                StopMoving();
                CallTargetTeached();
            }

            if (collision.gameObject.tag == "Obstacle")
            {
                ReachedObstacle();
            }
        }
    }

    protected virtual void ReachedObstacle()
    {
    }

    protected virtual void CallTargetTeached()
    {
        OnTargetReached?.Invoke();
    }
}
