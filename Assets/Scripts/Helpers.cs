using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Minima.Navigation;

public static class Helpers
{
    public static bool RandomBool()
    {
        return FakeRandomBool(50);
    }

    public static bool FakeRandomBool(int trueChance)
    {
        int randomInt = Random.Range(0, 100);

        if (randomInt >= 100 - Mathf.Clamp(trueChance, 0, 100))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static Vector2 GetClosestPointOnRect(Vector2 origin, Rect rect)
    {
        var a = origin;
        var b = rect.center;
        var slope = (a.y - b.y) / (a.x - b.x);

        float x;
        float y;

        var yOffset = slope * rect.width / 2f;

        if (yOffset.InBounds(-rect.height / 2f, rect.height / 2f))
        {
            if (a.x > b.x)
            {
                x = b.x + rect.width / 2f;
                y = b.y + yOffset;
            }
            else
            {
                x = b.x - rect.width / 2f;
                y = b.y - yOffset;
            }
        }
        else
        {
            var xOffset = (rect.height / 2f) / slope;

            if (a.y > b.y)
            {
                x = b.x + xOffset;
                y = b.y + rect.height / 2f;
            }
            else
            {
                x = b.x - xOffset;
                y = b.y - rect.height / 2f;
            }
        }

        return new Vector2(x, y);
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

    public static Vector2 RandomVectorNormalized()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        return new Vector2(x, y).normalized;
    }

    public static float GetTriangleSurface(Vector2 a, Vector2 b, Vector2 c, bool showDebug = false)
    {
        var ab = Vector2.Distance(a, b);
        float angle = Vector2.Angle(b - a, c - a);

        var hVector = (c - a) * Mathf.Sin(angle);
        float h = hVector.magnitude;

        if (showDebug)
        {
            Debug.DrawLine(a, b, Color.yellow, 15f);
            Debug.DrawLine(a, c, Color.yellow, 15f);
            Debug.DrawLine(b, c, Color.yellow, 15f);
            Debug.DrawLine(c, c - hVector, Color.red, 15f);
        }

        return (ab * h) / 2;

    }

    public static bool IsPointInRadius(Vector2 point, Vector2 origin, float radius)
    {
        if (Vector2.Distance(point, origin) < radius)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Returnes true if ab intersects with cd
    /// </summary>
    public static bool EdgesIntersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        float denominator = (d.y - c.y) * (b.x - a.x) - (d.x - c.x) * (b.y - a.y);

        float u = ((d.x - c.x) * (a.y - c.y) - (d.y - c.y) * (a.x - c.x)) / denominator;
        float v = ((b.x - a.x) * (a.y - c.y) - (b.y - a.y) * (a.x - c.x)) / denominator;

        if (u.InBounds(0f, 1f) && v.InBounds(0f, 1f))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Is int value between left and right (including left, excluding right)
    /// </summary>
    public static bool InBounds(this int value, int left, int right)
    {
        return value >= left && value < right;
    }

    /// <summary>
    /// Is float in bounds, including left and right values
    /// </summary>
    public static bool InBounds(this float value, float left, float right)
    {
        if (value >= left && value <= right)
        {
            return true;
        }

        return false;
    }

    public static bool Equal(this float a, float b, float precision = 0.001f)
    {
        return Mathf.Abs(b - a) <= precision;
    }

    private static List<RaycastHit2D> RaycastVisibility(Vector2 from, Vector2 to)
    {
        Vector2 distanceVector = (to - from);
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        var hitsCount = Physics2D.Raycast(from, distanceVector.normalized, new ContactFilter2D(), hits, distanceVector.magnitude);

        return hits;
    }

    public static bool CheckVisibility(Vector2 from, Vector2 to, params TargetType[] ignoreTypes)
    {
        var hits = RaycastVisibility(from, to);

        foreach (var t in ignoreTypes)
        {
            hits.RemoveAll(h => h.collider.gameObject.layer == (int)t);
        }

        return hits.Count == 0;
    }

    public static bool CheckVisibility(Vector2 from, Vector2 to, out List<RaycastHit2D> outHits)
    {
        var hits = RaycastVisibility(from, to);
        outHits = hits;
        return hits.Count <= 0;
    }

    public static Vector2 ToVector2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    public static Vector3 ToVector3(this Vector2 vector)
    {
        return new Vector3(vector.x, vector.y, 0f);
    }
}
