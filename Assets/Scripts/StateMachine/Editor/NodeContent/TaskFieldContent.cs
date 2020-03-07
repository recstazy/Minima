using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;

public class TaskFieldContent : NodeContent
{
    public event Action<Type> OnValueChanged;

    #region Fields

    protected FieldInfo field;
    protected TextContent name;
    protected EditableContent value;

    #endregion

    #region Properties

    #endregion

    public TaskFieldContent(IGraphObject parent, FieldInfo fieldInfo) : base(parent)
    {
        field = fieldInfo;
        DefaultSize = new Vector2(200f, 20f);
        Construct();
    }

    protected virtual void Construct()
    {

    }

    public override void Draw()
    {
        UpdateContentsSizes();
        SetContentsPositions();
        name.Draw();
        value.Draw();
    }

    private void SetContentsPositions()
    {
        name.SetRectPosition(rect.position);
        value.SetRectPosition(Rect.position + new Vector2(Rect.width - value.Rect.width, 0f));
    }

    protected virtual void UpdateContentsSizes()
    {
    }
}
