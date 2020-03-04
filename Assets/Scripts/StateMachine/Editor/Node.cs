using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Node
{
    public Action<Node> OnRemoveNode;
    public Action<Node> OnConnectClicked;

    private Rect rect;
    private bool isDragged;
    private bool isSelected;

    private GUIStyle currentStyle;
    private GUIStyle defaultStyle;
    private GUIStyle selectedStyle;

    public Rect Rect { get => rect; }
    public string Title { get; private set; }

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

    public Node(Vector2 position, float width, float height, Action<Node> connectionClicked, Action<Node> OnClickRemoveNode)
    {
        rect = new Rect(position.x, position.y, width, height);
        CreateStyle();
        OnRemoveNode = OnClickRemoveNode;
        OnConnectClicked = connectionClicked;
    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;
    }

    public void Draw()
    {
        GUI.Box(Rect, Title, currentStyle);

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
        var b = Rect.center;
        var slope = (a.y - b.y) / (a.x - b.x);

        float x;
        float y;

        var yOffset = slope * Rect.width / 2f;

        if (yOffset.InBounds(-Rect.height / 2f, Rect.height / 2f))
        {
            if (a.x > b.x)
            {
                x = b.x + Rect.width / 2f;
                y = b.y + yOffset;
            }
            else
            {
                x = b.x - Rect.width / 2f;
                y = b.y - yOffset;
            }
        }
        else
        {
            var xOffset = (Rect.height / 2f) / slope;

            if (a.y > b.y)
            {
                x = b.x + xOffset;
                y = b.y + Rect.height / 2f;
            }
            else
            {
                x = b.x - xOffset;
                y = b.y - Rect.height / 2f;
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
                        if (Rect.Contains(e.mousePosition))
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
    }

    private void ConnectNodeClicked()
    {
        OnConnectClicked?.Invoke(this);
    }

    private void CreateStyle()
    {
        defaultStyle = new GUIStyle();
        defaultStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        defaultStyle.border = new RectOffset(12, 12, 12, 12);

        selectedStyle = new GUIStyle();
        selectedStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
        selectedStyle.border = new RectOffset(12, 12, 12, 12);

        currentStyle = defaultStyle;
    }
}