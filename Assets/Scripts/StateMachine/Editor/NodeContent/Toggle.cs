using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Toggle : EditableContent
{
    #region Fields

    private bool lastValue;
    private bool value = false;

    #endregion

    #region Properties
    
    #endregion

    public Toggle(IGraphObject parent, bool defaultValue) : base(parent)
    {
        DefaultSize = new Vector2(25, 25);
        value = defaultValue;
        lastValue = value;
    }

    public override void Draw()
    {
        UpdateRect();
        value = EditorGUI.Toggle(rect, value);

        if (lastValue != value)
        {
            lastValue = value;
            CallValueChanged();
        }
    }

    protected override void CreateStyle()
    {
        style = (GUIStyle)"OL Toggle";
    }
}
