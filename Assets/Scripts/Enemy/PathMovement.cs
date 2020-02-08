using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Minima.Navigation;

public enum MovementType { Path, Direct }

public class PathMovement : TargetMovement
{
    #region Fields

    [SerializeField]
    private NavAgent navAgent;

    [SerializeField]
    private float acceptableRadius = 0.25f;

    [SerializeField]
    private float updateTime = 0.2f;

    private NavPath path;
    private Vector2 currentTargetPoint;
    private Coroutine movingCoroutine;
    protected MovementType movementType = MovementType.Path;

    #endregion

    #region Properties

    #endregion

    public void MoveToTarget(Transform target, MovementType movementType, System.Action reachedCallback = null)
    {
        this.movementType = movementType;
        StopMoving();

        if (reachedCallback != null)
        {
            OnTargetReached += reachedCallback;
        }
        
        MoveToTarget(target);
    }

    public override void MoveToTarget(Transform target)
    {
        currentTarget = target;
        path = navAgent.GetPath(target.position);
        MoveOnPath(path);
    }

    public override void MoveToTarget()
    {
        if (currentTarget != null && currentTargetPoint != Vector2.zero)
        {
            var direction = currentTargetPoint - thisTransform.position.ToVector2();
            MoveOnDirection(direction);
        }
    }

    private void MoveOnPath(NavPath path)
    {
        movingCoroutine = StartCoroutine(MoveOnPathCycle(path));
    }

    private IEnumerator MoveOnPathCycle(NavPath path)
    {
        int index = 0;

        while (index != path.NavPoints.Length)
        {
            currentTargetPoint = path.Points[index];

            while (!Helpers.IsPointInRadius(thisTransform.position, currentTargetPoint, acceptableRadius))
            {
                yield return new WaitForSeconds(updateTime);
            }

            index++;
        }

        StopMoving();
        CallTargetTeached();
        movingCoroutine = null;
    }

    public override void StopMoving()
    {
        base.StopMoving();
        currentTargetPoint = Vector2.zero;

        if (movingCoroutine != null)
        {
            StopCoroutine(movingCoroutine);
            movingCoroutine = null;
        }
    }
}
