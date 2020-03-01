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
    private Coroutine movingCoroutine;
    protected MovementType movementType = MovementType.Path;
    protected System.Action reachedCallback;

    #endregion

    #region Properties

    #endregion

    public void MoveToTarget(Transform target, MovementType movementType)
    {
        this.movementType = movementType;
        StopMoving();
        MoveToTarget(target);
    }

    public override void MoveToTarget(Transform target)
    {
        if (movementType == MovementType.Path)
        {
            updateEveryFrame = false;
            currentTarget = target;
            movingCoroutine = StartCoroutine(MoveOnPathCycle());
            navAgent.GetPathAsync(target.position, SetPath);
        }
        else
        {
            updateEveryFrame = true;
            base.MoveToTarget(target);
        }
    }

    public override void StopMoving()
    {
        base.StopMoving();
        currentTargetPoint = Vector2.zero;
        path = default;

        if (movingCoroutine != null)
        {
            StopCoroutine(movingCoroutine);
            movingCoroutine = null;
        }
    }

    private void SetPath(NavPath path)
    {
        this.path = path;
    }

    private IEnumerator MoveOnPathCycle()
    {
        int index = 0;

        yield return new WaitUntil(() => path.IsValid);

        while (index != path.NavPoints.Length)
        {
            if (index < 0 || index >= path.Length)
            {
                Debug.LogError("PathMovement: Index out of path range");
                break;
            }

            currentTargetPoint = path.Points[index];

            while (!Helpers.IsPointInRadius(thisTransform.position, currentTargetPoint, acceptableRadius))
            {
                yield return new WaitForSeconds(updateTime);
            }

            index++;
        }

        StopMoving();

        if (!path.IsValid || index == path.NavPoints.Length)
        {
            CallTargetReached();
        }
        
        movingCoroutine = null;
    }

    protected override void ReachedObstacle()
    {
        CallOnFail();
    }

    private void FindNewPath()
    {
        StopMoving();
        MoveToTarget(currentTarget);
    }
}
