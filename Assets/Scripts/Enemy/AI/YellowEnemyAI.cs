using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowEnemyAI : EnemyAI
{
    #region Fields

    [SerializeField]
    private float stopMovementDelay = 1f;

    private Coroutine stopCoroutine;

    #endregion

    #region Properties

    #endregion

    protected override void ShootTriggerCallback(GameObject target)
    {
        base.ShootTriggerCallback(target);

        if (stopCoroutine == null)
        {
            stopCoroutine = StartCoroutine(StopWhenShooting());
        }
    }

    private IEnumerator StopWhenShooting()
    {
        AIControlled.StopMovement();

        yield return new WaitForSeconds(stopMovementDelay);

        AIControlled.ContinueMovement();
        stopCoroutine = null;
    }
}
