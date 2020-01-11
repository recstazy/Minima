using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponTriggerable : EnemyWeapon
{
    #region Fields

    [SerializeField]
    private TriggerDelegate shootTrigger;

    #endregion

    #region Properties

    #endregion

    private void Awake()
    {
        shootTrigger.OnTargetTriggered += TargetTriggered;
    }

    private void OnDestroy()
    {
        shootTrigger.OnTargetTriggered -= TargetTriggered;
    }

    private void TargetTriggered(GameObject target)
    {
        Shoot();
    }
}
