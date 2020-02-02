using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Minima.Navigation;

public class PathMovement : TargetMovement
{
    #region Fields

    [SerializeField]
    private NavAgent navAgent;

    [SerializeField]
    private float acceptableRadius = 0.25f;

    [SerializeField]
    private float updateTime = 0.2f;

    [SerializeField]
    private float updateTargetTime = 1f;

    private NavPath path;
    private Vector2 currentTargetPoint;
    private Coroutine movingCoroutine;
    private Coroutine updateTargetCoroutine;

    #endregion

    #region Properties

    #endregion

    public override void MoveToTarget()
    {
        if (currentTarget != null && currentTargetPoint != Vector2.zero)
        {
            var direction = currentTargetPoint - thisTransform.position.ToVector2();
            MoveOnDirection(direction);
        }
    }

    public override void MoveToTarget(Transform target)
    {
        currentTarget = target;
        path = navAgent.GetPath(target.position);
        MoveOnPath(path);
    }

    private void MoveOnPath(NavPath path)
    {
        movingCoroutine = StartCoroutine(MoveOnPathCycle(path));
        
        if (updateTargetCoroutine == null)
        {
            updateTargetCoroutine = StartCoroutine(UpdateTargetCycle());
        }
    }

    private IEnumerator MoveOnPathCycle(NavPath path)
    {
        int index = 0;

        while (index != path.NavPoints.Count)
        {
            currentTargetPoint = path.Points[index];

            while (!StaticHelpers.IsPointInRadius(thisTransform.position, currentTargetPoint, acceptableRadius))
            {
                yield return new WaitForSeconds(updateTime);
            }

            index++;
        }

        StopMoving();
        movingCoroutine = null;
    }

    private IEnumerator UpdateTargetCycle()
    {
        while (canMove)
        {
            yield return new WaitForSeconds(updateTargetTime);

            if (movingCoroutine != null)
            {
                StopCoroutine(movingCoroutine);
                MoveToTarget(currentTarget);
            }
        }

        updateTargetCoroutine = null;
    }

    public override void StopMoving()
    {
        base.StopMoving();
        currentTargetPoint = Vector2.zero;
    }
}
