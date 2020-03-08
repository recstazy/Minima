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

    public Vector2 DefaultSize { get; set; } = new Vector2(200f, 50f);
    public bool UseParentRectCenter { get; set; } = true;
    public bool AutoUpdateSize { get; set; } = true;

    public Rect Rect
    {
        get
        {
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
        UpdateRect();
        GUI.Box(Rect, "", style);
    }

    public void SetRectPosition(Vector2 position)
    {
        if (!UseParentRectCenter)
        {
            rect.position = position;
        }
    }

    public void SetRectSize(Vector2 size)
    {
        if (!AutoUpdateSize)
        {
            rect.size = size;
        }
    }

    public virtual Vector2 GetRawSize()
    {
        return rect.size;
    }

    protected virtual void UpdateRect()
    {
        UpdateRectCenter();
        UpdateRectSize();
    }

    protected virtual void UpdateRectCenter()
    {
        if (UseParentRectCenter)
        {
            rect.center = parent.Rect.center;
        }
    }

    protected virtual void UpdateRectSize()
    {
        if (AutoUpdateSize)
        {
            rect.size = GetRawSize();
        }
    }

    protected virtual void CreateStyle()
    {
        style = (GUIStyle)"flow node 0";
    }
}
