using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.StateMachine
{
    public class Node
    {
        #region Fields

        private Rect rect;
        private string title;
        private GUIStyle style;

        #endregion

        #region Properties

        public Rect Rect { get => rect; }
        public string Title { get => title; }
        public GUIStyle Style { get => style; }

        #endregion

        public Node(Vector2 position, float width, float height, GUIStyle nodeStyle)
        {
            rect = new Rect(position.x, position.y, width, height);
            style = nodeStyle;
        }

        public void Drag(Vector2 delta)
        {
            rect.position += delta;
        }

        public void Draw()
        {
            GUI.Box(Rect, Title, Style);
        }

        public bool ProcessEvents(Event e)
        {
            return false;
        }
    }
}