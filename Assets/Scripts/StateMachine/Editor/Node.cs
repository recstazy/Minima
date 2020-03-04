using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Node
{
    public event Action<Node> OnRemoveNode;
    public event Action<Node> OnConnectClicked;

    #region Fields

    private Rect rect;
    private bool isDragged;
    private bool isSelected;

    private GUIStyle currentStyle;
    private GUIStyle defaultStyle;
    private GUIStyle selectedStyle;

    #endregion

    #region Properties

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

    #endregion

    public Node(Vector2 position, float width, float height)
    {
        rect = new Rect(position.x, position.y, width, height);
        CreateStyle();
    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;
    }

    public void Draw()
    {
        GUI.Box(Rect, Title, currentStyle);

        // Draw only connections where this node is InNode
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
        return Helpers.GetClosestPointOnRect(origin, Rect);
    }

    public bool ProcessEvents(Event e, NodeEditorEventArgs eventArgs)
    {
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

    private void ProcessMouseDown(Event e, NodeEditorEventArgs eventArgs)
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

    private void ProcessKeyUp(Event e, NodeEditorEventArgs eventArgs)
    {
        if (isSelected && !eventArgs.IsPerformingConnection)
        {
            if (e.keyCode == KeyCode.C)
            {
                ConnectNodeClicked();
                GUI.changed = true;
            }
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
        defaultStyle = new GUIStyle();
        defaultStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        defaultStyle.border = new RectOffset(12, 12, 12, 12);

        selectedStyle = new GUIStyle();
        selectedStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
        selectedStyle.border = new RectOffset(12, 12, 12, 12);

        currentStyle = defaultStyle;
    }
}