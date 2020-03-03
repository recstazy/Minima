using System;
using UnityEditor;
using UnityEngine;

public class Connection
{
    public Node InNode;
    public Node OutNode;
    public Action<Connection> OnClickRemoveConnection;

    public Connection(Node inNode, Node outNode, Action<Connection> OnClickRemoveConnection)
    {
        this.InNode = inNode;
        this.OutNode = outNode;
        this.OnClickRemoveConnection = OnClickRemoveConnection;
    }

    public void Draw()
    {
        var inPoint = InNode.GetClosestConnectionPoint(OutNode.rect.center).rect.center;
        var outPoint = OutNode.GetClosestConnectionPoint(InNode.rect.center).rect.center;

        Handles.DrawBezier(
                inPoint,
                outPoint,
                inPoint,
                outPoint,
                Color.white,
                null,
                2f
            );

        if (Handles.Button((InNode.rect.center + OutNode.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
        {
            OnClickRemoveConnection?.Invoke(this);
        }

        DrawArrowCap(inPoint, outPoint, 10f);
    }

    private void DrawArrowCap(Vector2 origin, Vector2 target, float size)
    {
        Vector2 bisect = (target - origin).normalized * size;
        Vector2 triangleBotomCenter = target - bisect;
        Vector2 a = Vector2.Perpendicular(bisect / 2);

        Handles.DrawPolyLine(target, triangleBotomCenter + a, triangleBotomCenter - a, target);
    }
}