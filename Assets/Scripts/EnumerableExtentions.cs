using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public static class EnumerableExtentions
{
    private static Random random = new Random();

    public static T[] ConcatOne<T>(this T[] array, T item)
    {
        Array.Resize(ref array, array.Length + 1);
        array[array.Length - 1] = item;
        return array;
    }

    public static T[] RemoveAt<T>(this T[] array, int index)
    {
        if (index >= array.Length)
        {
            return array;
        }

        var copy = array.ToList();
        copy.RemoveAt(index);
        return copy.ToArray();
    }

    public static T[] Remove<T>(this T[] array, T item)
    {
        int index = Array.IndexOf(array, item);

        if (index.InBounds(0, array.Length))
        {
            return array.RemoveAt(index);
        }

        return array;
    }

    public static bool Contains<T>(this T[] array, T item)
    {
        int index = Array.IndexOf(array, item);

        if (index.InBounds(0, array.Length))
        {
            return true;
        }

        return false;
    }

    public static T[] AddUniq<T>(this T[] array, T item)
    {
        if (!array.Contains(item))
        {
            return array.ConcatOne(item);
        }

        return array;
    }

    public static T[] ConcatUniq<T>(this T[] array, T[] items)
    {
        var toConcat = items.Except(array);
        return array.Concat(toConcat).ToArray();
    }

    public static void AddUniq<T>(this List<T> list, T item)
    {
        if (!list.Contains(item))
        {
            list.Add(item);
        }
    }

    public static T Random<T>(this IList<T> list)
    {
        return list[random.Next(0, list.Count)];
    }

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
