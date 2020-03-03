using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeEditorEventArgs
{
    #region Fields

    NodeBasedEditor editor;

    #endregion

    #region Properties

    public bool IsPerformingConnection { get => editor.IsPerformingConnection; }

    #endregion

    public NodeEditorEventArgs(NodeBasedEditor editor)
    {
        this.editor = editor;
    }
}
