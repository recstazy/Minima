using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeContent : IGraphObject
{
    #region Fields

    protected Rect rect;
    protected IGraphObject parent;
    protected GUIStyle style;

    #endregion

    #region Properties

    public Vector2 DefaultSize { get; } = new Vector2(200f, 50f);

    public Rect Rect
    {
        get
        {
            UpdateRect();
            return rect;
        }
    }

    #endregion

    public NodeContent(IGraphObject parent)
    {
        this.parent = parent;

        CreateStyle();
        UpdateRect();
    }

    public virtual void Draw()
    {
        GUI.Box(Rect, "", style);
    }

    protected virtual void UpdateRect()
    {
        rect.center = parent.Rect.center;
        rect.size = DefaultSize;
    }

    protected virtual void CreateStyle()
    {
        style = new GUIStyle();
        style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        style.border = new RectOffset(12, 12, 12, 12);
    }
}
