using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

public class BoolFieldContent : TaskFieldContent
{
    #region Fields

    #endregion

    #region Properties
    
    #endregion

    public BoolFieldContent(IGraphObject parent, FieldInfo fieldInfo) : base(parent, fieldInfo)
    {
    }

    protected override void Construct()
    {
        name = new TextContent(this, field.Name);
        name.UseParentRectCenter = false;
        value = new Toggle(this, false);
        value.UseParentRectCenter = false;
        value.UseExternalSize = true;
    }

    protected override void UpdateContentsSizes()
    {
        value.SetRectSize(new Vector2(name.Rect.height, name.Rect.height));
    }
}
