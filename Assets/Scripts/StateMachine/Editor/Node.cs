using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Node
{
    public Rect rect;
    public string title;
    public bool isDragged;
    public bool isSelected;

    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;

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

    public Action<Node> OnRemoveNode;
    public Action<Node> OnConnectClicked;

    public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<Node> connectionClicked, Action<Node> OnClickRemoveNode)
    {
        rect = new Rect(position.x, position.y, width, height);
        style = nodeStyle;
        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        OnRemoveNode = OnClickRemoveNode;
        OnConnectClicked = connectionClicked;
    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;
    }

    public void Draw()
    {
        GUI.Box(rect, title, style);

        foreach (var c in Connections.Where(c => c.InNode == this))
        {
            c.Draw();
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

    public bool ProcessEvents(Event e, NodeEditorEventArgs eventArgs)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                {
                    if (e.button == 0)
                    {
                        if (rect.Contains(e.mousePosition))
                        {
                            isDragged = true;
                            SetSelected(true);

                            if (eventArgs.IsPerformingConnection)
                            {
                                ConnectNodeClicked();
                            }
                        }
                        else
                        {
                            SetSelected(false);
                        }
                    }

                    if (e.button == 1)
                    {
                        if (!rect.Contains(e.mousePosition))
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
                    if (isSelected && !eventArgs.IsPerformingConnection)
                    {
                        if (e.keyCode == KeyCode.C)
                        {
                            ConnectNodeClicked();
                            GUI.changed = true;
                        }
                    }
                    break;
                }
        }

        return false;
    }

    private void SetSelected(bool isSelected)
    {
        this.isSelected = isSelected;

        if (isSelected)
        {
            style = selectedNodeStyle;
        }
        else
        {
            style = defaultNodeStyle;
        }

        GUI.changed = true;
    }

    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        genericMenu.AddItem(new GUIContent("Connect"), false, ConnectNodeClicked);
        genericMenu.ShowAsContext();
    }

    private void OnClickRemoveNode()
    {
        OnRemoveNode?.Invoke(this);
    }

    private void ConnectNodeClicked()
    {
        OnConnectClicked?.Invoke(this);
    }
}