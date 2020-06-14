﻿using System;
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

        public NodeType NodeType { get; protected set; }
        public Rect Rect { get => rect; }
        public Vector2 DefaultSize { get; } = new Vector2(200f, 50f);
        public string Title { get; private set; }
        public ColumnContent Content { get => content; }
        public bool UseParentRectCenter { get; set; } = false;
        public Minima.StateMachine.Node SMNode { get; private set; }

        public Connection[] Connections { get; private set; } = new Connection[0];
        public int InputStates { get; private set; }
        public int OutputStates { get; private set; }
        public int InputConditions { get; private set; }
        public int OutputConditions { get; private set; }

        public Node[] ConnectedNodes
        {
            get => Connections
                    .Select(c => c.Output)
                    .Concat(Connections.Select(c => c.Input))
                    .ToArray();
        }

        public Node[] Outputs
        {
            get => Connections
                .Where(c => c.Output == this)
                .Select(c => c.Input).ToArray();
        }

        #endregion

        public Node(Vector2 position, NodeType nodeType)
        {
            SMNode = new Minima.StateMachine.Node(nodeType);
            rect = new Rect(position.x, position.y, DefaultSize.x, DefaultSize.y);
            CreateStyle();
        }

        public Node(Minima.StateMachine.Node node)
        {
            SMNode = node;
            rect = new Rect(node.Position.x, node.Position.y, DefaultSize.x, DefaultSize.y);
            CreateStyle();
        }

        public void Drag(Vector2 delta)
        {
            rect.position += delta;
        }

        public virtual void Draw()
        {
            UpdateRectToFitContent();
            GUI.Box(Rect, "", currentStyle);
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
            foreach (var c in Connections.Where(c => c.Output == this))
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
            SMNode.AddConnectionTo(connection.Input.SMNode);
            ChangeConncectionsCount(connection, true);
        }

        public void RemoveConnection(Connection connection)
        {
            Connections = Connections.Remove(connection);
            SMNode.RemoveConnectionTo(connection.Input.SMNode);
            ChangeConncectionsCount(connection, false);
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
                        SMNode.Position = rect.position;
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
            if (content != null)
            {
                return Content.ProcessEvent(e, eventArgs);
            }

            return false;
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

        private void ChangeConncectionsCount(Connection connection, bool increment)
        {
            var term = increment ? 1 : -1;

            if (connection.Input == this)
            {
                if (connection.Output.NodeType == NodeType.State)
                {
                    InputStates += term;
                }
                else if (connection.Output.NodeType == NodeType.Condition)
                {
                    InputConditions += term;
                }
            }
            else
            {
                if (connection.Input.NodeType == NodeType.State)
                {
                    OutputStates += term;
                }
                else if (connection.Input.NodeType == NodeType.Condition)
                {
                    OutputConditions += term;
                }
            }
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
            Title = SMNode.Title;
        }

        private void UpdateRectToFitContent()
        {
            if (content != null)
            {
                if (content.Rect.height < DefaultSize.y)
                {
                    rect.height = DefaultSize.y;
                    rect.width = content.Rect.width * contentSizeMultiplier;
                }
                else if (content.Rect.width < DefaultSize.x)
                {
                    rect.width = DefaultSize.x;
                    rect.height = content.Rect.height * contentSizeMultiplier;
                }
                else
                {
                    rect.size = content.Rect.size * contentSizeMultiplier;
                }
            }
            else
            {
                rect.size = DefaultSize;
            }
        }
    }
}