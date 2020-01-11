using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementStopOnShoot : EnemyMovementShootDependent
{
    #region Fields

    public float stopDelay = 1f;

    #endregion

    #region Properties

    #endregion

    protected override void WeaponUsed()
    {
        base.WeaponUsed();
        SetCanMove(false);
        isShooting = true;
        StartCoroutine(Stop());
    }

    private IEnumerator Stop()
    {
        yield return new WaitForSeconds(stopDelay);
        isShooting = false;
        SetCanMove(true);
    }
}
