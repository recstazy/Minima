using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

namespace Minima.StateMachine.Editor
{
    public class NodeProvider
    {
        public event Action<Node> OnNodeCreated;

        #region Fields

        private GenericMenu contextMenu;
        private Vector2 lastMousePosition;

        #endregion

        #region Properties

        #endregion

        public NodeProvider()
        {
            CreateDropDownMenu();
        }

        public void ProcessEvents(Event e, SMEditorEventArgs eventArgs)
        {
            lastMousePosition = e.mousePosition;

            if (e.type == EventType.MouseUp)
            {
                if (e.button == 1 && !eventArgs.IsDragging)
                {
                    Show();
                    e.Use();
                }
            }
        }

        public void Show()
        {
            if (contextMenu != null)
            {
                contextMenu.ShowAsContext();
            }
        }

        private void CreateDropDownMenu()
        {
            string addGroupName = "Add";

            contextMenu = new GenericMenu();
            contextMenu.AddItem(new GUIContent(addGroupName + "/State node"), false, AddStateNodeClicked);
            contextMenu.AddItem(new GUIContent(addGroupName + "/Condition node"), false, AddConditionNodeClicked);
        }

        private void AddStateNodeClicked()
        {
            var node = new TaskNode(lastMousePosition, NodeType.State);
            CallNodeCreated(node);
        }

        private void AddConditionNodeClicked()
        {
            var node = new TaskNode(lastMousePosition, NodeType.Condition);
            CallNodeCreated(node);
        }

        private void CallNodeCreated(Node node)
        {
            OnNodeCreated?.Invoke(node);
        }
    }
}
