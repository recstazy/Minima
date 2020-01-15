using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovementAvoidObstacles : TargetMovement
{
    #region Fields

    [SerializeField]
    private TriggerDelegate forwardTrigger;

    #endregion

    #region Properties

    #endregion
    protected override void Start()
    {
        base.Start();
        forwardTrigger.OnTargetTriggered += ForwardTriggered;
    }

    private void OnDestroy()
    {
        forwardTrigger.OnTargetTriggered -= ForwardTriggered;
    }

    public override void MoveToTarget()
    {
        base.MoveToTarget();
    }

    private void ForwardTriggered(GameObject target)
    {

    }
}
