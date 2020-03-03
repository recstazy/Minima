using System;
using UnityEngine;
using UnityEditor;

public enum ConnectionDirection { Left, Right, Up, Down }

public class ConnectionPoint
{
    public Action<ConnectionPoint> OnClickConnectionPoint;

    public Rect rect;
    public ConnectionDirection type;
    public Node node;
    public GUIStyle style;
    
    public ConnectionPoint(Node node, ConnectionDirection type)
    {
        this.node = node;
        this.type = type;
        CreateStyle();
        rect = new Rect(0, 0, node.rect.width * 0.1f, node.rect.height * 0.5f);
    }

    public void Draw()
    {
        switch (type)
        {
            case ConnectionDirection.Left:
                {
                    rect.x = node.rect.x - rect.width * 0.5f;
                    rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;
                    break;
                }
            case ConnectionDirection.Right:
                {
                    rect.x = node.rect.x + node.rect.width - rect.width * 0.5f;
                    rect.y = node.rect.y + node.rect.height * 0.5f - rect.height * 0.5f;
                    break;
                }
            case ConnectionDirection.Up:
                {
                    rect.x = node.rect.x + node.rect.width * 0.5f - rect.width * 0.5f;
                    rect.y = node.rect.y - rect.height / 2;
                    break;
                }
            case ConnectionDirection.Down:
                {
                    rect.x = node.rect.x + node.rect.width * 0.5f - rect.width * 0.5f;
                    rect.y = node.rect.y + node.rect.height - rect.height * 0.5f;
                    break;
                }
        }

        if (GUI.Button(rect, "", style))
        {
            //OnClickConnectionPoint?.Invoke(this);
        }
    }

    private void CreateStyle()
    {
        style = new GUIStyle();
        style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        style.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        style.border = new RectOffset(4, 4, 12, 12);
    }
}