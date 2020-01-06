using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class StaticHelpers
{
    public static bool RandomBool()
    {
        int randomInt = UnityEngine.Random.Range(0, 100);

        if (randomInt >= 50)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    
    private static System.Random random = new System.Random();
    /// <summary>
    /// Copiet this from StackOverflow
    /// </summary>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
