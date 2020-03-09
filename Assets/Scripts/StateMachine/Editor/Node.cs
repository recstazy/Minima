using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Minima.StateMachine.Editor
{
    public class Node : IGraphObject
    {
        public event Action<Node> OnRemoveNode;
        public event Action<Node> OnConnectClicked;

        #region Fields

        protected Rect rect;
        protected float contentSizeMultiplier = 1.1f;
        protected bool isSelected;
        private bool isDragged;
        private ColumnContent content;

        private GUIStyle currentStyle;
        private GUIStyle defaultStyle;
        private GUIStyle selectedStyle;

        #endregion

        #region Properties

        public Rect Rect { get => rect; }
        public Vector2 DefaultSize { get; } = new Vector2(200f, 50f);
        public string Title { get; private set; }
        public ColumnContent Content { get => content; }
        public bool UseParentRectCenter { get; set; } = false;


        public Connection[] Connections { get; private set; } = new Connection[0];

        public Node[] ConnectedNodes
        {
            get
            {
                return Connections
                    .Select(c => c.InNode)
                    .Concat(Connections.Select(c => c.OutNode))
                    .ToArray();
            }
        }

        #endregion

        public Node(Vector2 position)
        {
            rect = new Rect(position.x, position.y, DefaultSize.x, DefaultSize.y);
            CreateStyle();
        }

        public void Drag(Vector2 delta)
        {
            rect.position += delta;
        }

        public virtual void Draw()
        {
            UpdateRectToFitContent();
            GUI.Box(Rect, Title, currentStyle);
            DrawContent();
            DrawConnections();
        }

        private void DrawContent()
        {
            if (content != null)
            {
                content.Draw();
            }
        }

        private void DrawConnections()
        {
            // Draw only connections where this node is InNode
            foreach (var c in Connections.Where(c => c.InNode == this))
            {
                c.Draw();
            }
        }

        public void AddContent(NodeContent content)
        {
            if (this.content == null)
            {
                this.content = new ColumnContent(this);
            }

            if (content != null)
            {
                Content.AddContent(content);
            }
        }

        public void AddConnection(Connection connection)
        {
            Connections = Connections.ConcatOne(connection);
        }

        public void RemoveConnection(Connection connection)
        {
            var index = Array.IndexOf(Connections, connection);

            if (index >= 0)
            {
                Connections = Connections.RemoveAt(index);
            }
        }

        public Vector2 GetClosestPointOnRect(Vector2 origin)
        {
            return Helpers.GetClosestPointOnRect(origin, Rect);
        }

        public bool ProcessEvents(Event e, SMEditorEventArgs eventArgs)
        {
            bool used = ProcessContentEvents(e, eventArgs);

            if (used)
            {
                e.Use();
                return true;
            }

            switch (e.type)
            {
                case EventType.MouseDown:
                    {
                        ProcessMouseDown(e, eventArgs);
                        break;
                    }
                case EventType.MouseUp:
                    {
                        isDragged = false;
                        break;
                    }
                case EventType.MouseDrag:
                    {
                        if (e.button == 0 && isDragged)
                        {
                            Drag(e.delta);
                            e.Use();
                            return true;
                        }
                        break;
                    }
                case EventType.KeyUp:
                    {
                        ProcessKeyUp(e, eventArgs);
                        break;
                    }
            }

            return false;
        }

        private bool ProcessContentEvents(Event e, SMEditorEventArgs eventArgs)
        {
            return Content.ProcessEvent(e, eventArgs);
        }

        private void ProcessMouseDown(Event e, SMEditorEventArgs eventArgs)
        {
            if (e.button == 0)
            {
                if (Rect.Contains(e.mousePosition))
                {
                    isDragged = true;
                    SetSelected(true);

                    if (eventArgs.IsPerformingConnection)
                    {
                        ConnectNodeClicked();
                        e.Use();
                    }
                    else
                    {
                        if (e.modifiers == EventModifiers.Control)
                        {
                            ConnectNodeClicked();
                            e.Use();
                            GUI.changed = true;
                        }
                    }
                }
                else
                {
                    SetSelected(false);
                }
            }
            else if (e.button == 1)
            {
                if (!Rect.Contains(e.mousePosition))
                {
                    SetSelected(false);
                }
                else
                {
                    if (!isSelected)
                    {
                        SetSelected(true);
                    }

                    ProcessContextMenu();
                    e.Use();
                }
            }
        }

        private void ProcessKeyUp(Event e, SMEditorEventArgs eventArgs)
        {
            if (isSelected && !eventArgs.IsPerformingConnection)
            {
                if (e.keyCode == KeyCode.Delete)
                {
                    RemoveSelf();
                }
            }
        }

        private void SetSelected(bool isSelected)
        {
            this.isSelected = isSelected;

            if (isSelected)
            {
                currentStyle = selectedStyle;
            }
            else
            {
                currentStyle = defaultStyle;
            }

            GUI.changed = true;
        }

        private void ProcessContextMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Remove node"), false, RemoveSelf);
            genericMenu.AddItem(new GUIContent("Connect"), false, ConnectNodeClicked);
            genericMenu.ShowAsContext();
        }

        private void RemoveSelf()
        {
            foreach (var c in Connections)
            {
                c.RemoveSelf();
            }

            OnRemoveNode?.Invoke(this);
            GUI.changed = true;
        }

        private void ConnectNodeClicked()
        {
            OnConnectClicked?.Invoke(this);
        }

        private void CreateStyle()
        {
            defaultStyle = (GUIStyle)"flow node 0";
            selectedStyle = (GUIStyle)"flow node 0 on";
            currentStyle = defaultStyle;
        }

        private void UpdateRectToFitContent()
        {
            if (content != null)
            {
                rect.size = content.Rect.size * contentSizeMultiplier;
            }
            else
            {
                rect.size = DefaultSize;
            }
        }

        public void SetRectPosition(Vector2 position)
        {
        }
    }
}