using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Minima.StateMachine.Editor
{
    public class SMEditorEventArgs
    {
        #region Fields

        StateMachineEditor editor;

        #endregion

        #region Properties

        public bool IsPerformingConnection { get => editor.IsPerformingConnection; }
        public bool IsDragging { get => editor.IsDragging; }

        #endregion

        public SMEditorEventArgs(StateMachineEditor editor)
        {
            this.editor = editor;
        }
    }
}
