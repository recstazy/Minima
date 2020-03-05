using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeContent : IGraphObject
{
    #region Fields

    private Rect rect;
    private IGraphObject parent;
    private string text = "text";
    private GUIStyle style;

    #endregion

    #region Properties

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

    public void Draw()
    {
        GUI.Box(Rect, "", style);
        //GUI.Label(Rect, new GUIContent(text), style);
    }

    private void UpdateRect()
    {
        rect.center = parent.Rect.center;
        rect.size = new Vector2(200f, 100f);
    }

    private void CreateStyle()
    {
        style = new GUIStyle();
        style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        style.border = new RectOffset(12, 12, 12, 12);
    }
}
