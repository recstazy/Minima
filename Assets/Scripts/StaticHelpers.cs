using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticHelpers
{
    public static bool RandomBool()
    {
        int randomInt = Random.Range(0, 100);

        if (randomInt >= 50)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
