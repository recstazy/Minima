using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class InputFieldContent : TaskFieldContent
{
    #region Fields

    #endregion

    #region Properties

    #endregion

    public InputFieldContent(IGraphObject parent, FieldInfo fieldInfo) : base(parent, fieldInfo)
    {

    }

    protected override void Construct()
    {
        name = new TextContent(this, field.Name);
        name.UseParentRectCenter = false;
        value = new InputField(this, field);
        value.UseParentRectCenter = false;
        value.UseExternalSize = true;
    }

    protected override void UpdateContentsSizes()
    {
        value.SetRectSize(new Vector2(name.Rect.height * 3f, name.Rect.height));
    }
}
