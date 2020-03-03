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

    public ConnectionPoint[] connectPoints;

    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;

    public Action<Node> OnRemoveNode;
    public Action<Node> OnConnectClicked;

    public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<Node> connectionClicked, Action<Node> OnClickRemoveNode)
    {
        rect = new Rect(position.x, position.y, width, height);
        style = nodeStyle;
        CreateConnectionArea(connectionClicked);
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
        foreach (var a in connectPoints)
        {
            a.Draw();
        }

        GUI.Box(rect, title, style);
    }

    public ConnectionPoint GetClosestConnectionPoint(Vector2 origin)
    {
        var point = connectPoints
            .Aggregate((p, n) => Vector2.Distance(origin, p.rect.center) < Vector2.Distance(origin, n.rect.center) ? p : n);

        return point;
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

    private void CreateConnectionArea(Action<Node> areaClicked)
    {
        var left = new ConnectionPoint(this, ConnectionDirection.Left);
        var right = new ConnectionPoint(this, ConnectionDirection.Right);
        var up = new ConnectionPoint(this, ConnectionDirection.Up);
        var down = new ConnectionPoint(this, ConnectionDirection.Down);

        connectPoints = new ConnectionPoint[] { left, right, up, down };
    }
}