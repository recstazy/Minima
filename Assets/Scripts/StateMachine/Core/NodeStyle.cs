using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeStyle
{
    private static GUIStyle style = new GUIStyle(GUI.skin.box);

    public static GUIStyle GetStyle()
    {
        return style;
    }
}
