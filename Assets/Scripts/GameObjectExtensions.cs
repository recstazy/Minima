using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    public static bool ApplyDamage (this GameObject gObject, float amount)
    {
        var iDamagable = gObject.GetComponent<IDamageble>();

        if (iDamagable != null)
        {
            iDamagable.ApplyDamage(amount);
            return true;
        }
        else
        {
            return false;
        }
    }
}
