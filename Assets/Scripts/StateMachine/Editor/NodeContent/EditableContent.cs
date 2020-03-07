using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditableContent : NodeContent
{
    public event System.Action OnValueChanged;

    #region Fields

    #endregion

    #region Properties

    public bool UseExternalSize = false;

    #endregion

    public EditableContent(IGraphObject parent) : base(parent)
    {

    }

    protected void CallValueChanged()
    {
        OnValueChanged?.Invoke();
    }

    public void SetRectSize(Vector2 size)
    {
        if (UseExternalSize)
        {
            rect.size = size;
        }
    }

    protected override void UpdateRectSize()
    {
        if (!UseExternalSize)
        {
            base.UpdateRectSize();
        }
    }
}
