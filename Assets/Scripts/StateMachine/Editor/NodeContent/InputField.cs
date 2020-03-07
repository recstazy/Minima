using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class InputField : EditableContent
{
    #region Fields

    protected FieldInfo field;

    protected int intValue;
    protected float floatValue;
    protected double doubleValue;
    protected string stringValue;

    #endregion

    #region Properties

    #endregion

    public InputField(IGraphObject parent, FieldInfo fieldInfo) : base(parent)
    {
        DefaultSize = new Vector2(200f, 20f);
        field = fieldInfo;
    }

    public override void Draw()
    {
        UpdateRect();
        DrawField();
    }

    protected override void CreateStyle()
    {
        base.CreateStyle();
    }

    private void DrawField()
    {
        if (field.FieldType == typeof(int))
        {
            intValue = EditorGUI.IntField(rect, intValue);
        }
        else if (field.FieldType == typeof(float))
        {
            floatValue = EditorGUI.FloatField(rect, floatValue);
        }
        else if (field.FieldType == typeof(string))
        {
            stringValue = EditorGUI.TextField(rect, stringValue);
        }
    }
}
