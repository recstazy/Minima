using System.Collections;
using System.Collections.Generic;
using System;

public static class ListExtentions
{
    public static T LastItem<T>(this IList<T> list)
    {
        if (list.Count > 0)
        {
            return list[list.Count - 1];
        }
        else
        {
            return default;
        }
    }

    public static T FirstItem<T>(this IList<T> list)
    {
        if (list.Count > 0)
        {
            return list[0];
        }
        else
        {
            return default;
        }
    }

    public static void AddUniq<T>(this List<T> list, T item)
    {
        if (!list.Contains(item))
        {
            list.Add(item);
        }
    }

    private static Random random = new Random();
    /// <summary>
    /// Copied this from StackOverflow
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
