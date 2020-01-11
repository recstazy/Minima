using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    public static IDamagable ApplyDamage (this GameObject gObject, float amount, Character from = null)
    {
        var iDamagable = gObject.GetComponent<IDamagable>();

        if (iDamagable != null)
        {
            iDamagable.ApplyDamage(amount, from);
            return iDamagable;
        }
        else
        {
            return null;
        }
    }
}
