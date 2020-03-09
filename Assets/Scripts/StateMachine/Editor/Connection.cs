using System;
using UnityEditor;
using UnityEngine;

namespace Minima.StateMachine.Editor
{
    public class Connection
    {
        #region Properties

        public Node InNode { get; private set; }
        public Node OutNode { get; private set; }

        #endregion

        public Connection(Node inNode, Node outNode)
        {
            InNode = inNode;
            OutNode = outNode;
            AddSelf();
        }

        public void Draw()
        {
            var inPoint = InNode.GetClosestPointOnRect(OutNode.Rect.center);
            var outPoint = OutNode.GetClosestPointOnRect(InNode.Rect.center);

            Handles.DrawBezier(inPoint, outPoint, inPoint, outPoint, Color.white, null, 2f);
            DrawArrowCap(inPoint, outPoint, 10f);

            if (Handles.Button((inPoint + outPoint) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
            {
                RemoveSelf();
            }
        }

        public void AddSelf()
        {
            InNode.AddConnection(this);
            OutNode.AddConnection(this);
        }

        public void RemoveSelf()
        {
            InNode.RemoveConnection(this);
            OutNode.RemoveConnection(this);
        }

        private void DrawArrowCap(Vector2 origin, Vector2 target, float size)
        {
            Vector2 bisect = (target - origin).normalized * size;
            Vector2 triangleBotomCenter = target - bisect;
            Vector2 a = Vector2.Perpendicular(bisect / 2);

            Handles.DrawPolyLine(target, triangleBotomCenter + a, triangleBotomCenter - a, target);
        }
    }
}