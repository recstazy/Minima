using System.Collections;
using System.Linq;
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

    public static Vector2 RandomPointInTriangle(Vector2 a, Vector2 b, Vector2 c, bool showDebug = false)
    {
        if (showDebug)
        {
            Debug.DrawLine(a, b, Color.red, 30f);
            Debug.DrawLine(b, c, Color.magenta, 30f);
            Debug.DrawLine(a, c, Color.yellow, 30f);
        }

        Vector2 ab = b - a;
        Vector2 p = ab * Random.Range(0f, 1f); // point on ab
        Vector2 cp = p - (c - a); // line to p from c
        Vector2 result = c + cp * Random.Range(0f, 1f); // point on cp

        return result;
    }

    public static Vector2 GetTriangleCenter(Vector2 a, Vector2 b, Vector2 c, bool showDebug = false)
    {
        float centerX = (a.x + b.x + c.x) / 3;
        float centerY = (a.y + b.y + c.y) / 3;

        Vector2 center = new Vector2(centerX, centerY);

        if (showDebug)
        {
            // draw triangle
            Debug.DrawLine(a, b, Color.red, 30f);
            Debug.DrawLine(b, c, Color.red, 30f);
            Debug.DrawLine(a, c, Color.red, 30f);

            //draw center
            Debug.DrawLine(a, center.ToVector3(), Color.yellow, 30f);
            Debug.DrawLine(b, center.ToVector3(), Color.yellow, 30f);
            Debug.DrawLine(c, center.ToVector3(), Color.yellow, 30f);
        }

        return center;
    }

    public static float GetInnerTriangleRadius(Vector2 a, Vector2 b, Vector2 c)
    {
        float ab = (b - a).magnitude;
        float bc = (c - b).magnitude;
        float ac = (c - a).magnitude;

        float p = 0.5f * (ab + bc + ac);
        float radius = Mathf.Sqrt(((p - ab) * (p - bc) * (p - ac)) / p); // triangle inner circle radius

        return radius;
    }

    public static Vector2 RandomPointInRadius(Vector2 origin, float radius, bool showDebug = false)
    {
        float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);

        Vector2 offset = new Vector2(x, y) * radius;

        if (showDebug)
        {
            Debug.DrawLine(origin, origin + offset, Color.red, 30f);
        }

        return origin + offset;
    }

    public static bool CheckVisibility(Vector2 from, Vector2 to, bool showDebug = false)
    {
        Vector2 distanceVector = (to - from);
        Ray ray = new Ray(from, distanceVector.normalized);

        List<RaycastHit2D> hits = new List<RaycastHit2D>();

        var hitsCount = Physics2D.Raycast(from, distanceVector.normalized, new ContactFilter2D(), hits, distanceVector.magnitude);

        if (showDebug)
        {
            Debug.DrawLine(ray.origin, ray.direction * distanceVector.magnitude  + ray.origin, Color.green, 30f);
            Debug.Log("CheckVisibility: count = " + hitsCount);

            foreach (var h in hits)
            {
                Debug.Log(h.collider.name);
            }
        }

        return hitsCount <= 0;
    }

    public static bool IsPointInCircle(Vector2 center, float radius, Vector2 point)
    {
        float distance = Vector2.Distance(point, center);

        if (distance <= radius)
        {
            return true;
        }

        return false;
    }

    public static bool IsInRange(this float value, float left, float right, bool includeBounds)
    {
        if (includeBounds)
        {
            if (value >= left && value <= right)
            {
                return true;
            }
        }
        else
        {
            if (value > left && value < right)
            {
                return true;
            }
        }

        return false;
    }

    public static Vector2 ToVector2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    public static Vector3 ToVector3(this Vector2 vector)
    {
        return new Vector3(vector.x, vector.y, 0f);
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
