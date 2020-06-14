using System;
using UnityEditor;
using UnityEngine;

namespace Minima.StateMachine.Editor
{
    public class Connection
    {
        #region Properties

        public Node Output { get; private set; }
        public Node Input { get; private set; }

        #endregion

        public Connection(Node output, Node input)
        {
            Output = output;
            Input = input;
            AddSelf();
        }

        public void Draw()
        {
            var startPoint = Output.GetClosestPointOnRect(Input.Rect.center);
            var endPoint = Input.GetClosestPointOnRect(Output.Rect.center);

            Handles.DrawBezier(startPoint, endPoint, startPoint, endPoint, Color.white, null, 3f);
            DrawArrowCap(startPoint, endPoint, 10f);

            if (Handles.Button((startPoint + endPoint) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
            {
                RemoveSelf();
            }
        }

        public void AddSelf()
        {
            Output.AddConnection(this);
            Input.AddConnection(this);
        }

        public void RemoveSelf()
        {
            Output.RemoveConnection(this);
            Input.RemoveConnection(this);
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