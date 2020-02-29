using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MovementComponent
{
    public System.Action<bool> OnMovementStopped;

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

    public void BindMovementStop(System.Action<bool> stopCallback)
    {
        OnMovementStopped = stopCallback;
    }

    public void DisposeCallbacks()
    {
        OnMovementStopped = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentTarget != null)
        {
            if (collision.transform == currentTarget)
            {
                StopMoving();
                CallTargetReached();
            }

            if (collision.gameObject.tag == "Obstacle")
            {
                ReachedObstacle();
            }
        }
    }

    protected virtual void ReachedObstacle()
    {
        CallOnFail();
    }

    protected virtual void CallOnFail()
    {
        OnMovementStopped?.Invoke(false);
        DisposeCallbacks();
    }

    protected virtual void CallTargetReached()
    {
        OnMovementStopped?.Invoke(true);
        DisposeCallbacks();
    }
}
