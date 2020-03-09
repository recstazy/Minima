using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Minima.StateMachine.Editor
{
    public class NodeConnector
    {
        #region Fields

        private Node selectedInNode;
        private Node selectedOutNode;

        #endregion

        #region Properties

        public bool IsPerformingConnection { get; private set; }

        #endregion

        public void ProcessEvents(Event e)
        {
            DrawConnectionLine(e);
            ProcessEvent(e);
        }

        public void ConnectNodeClicked(Node clickedNode)
        {
            IsPerformingConnection = true;

            if (selectedInNode == null)
            {
                selectedInNode = clickedNode;
            }
            else
            {
                selectedOutNode = clickedNode;
            }

            if (selectedOutNode != null)
            {
                if (selectedOutNode != selectedInNode)
                {
                    CreateConnection();
                }

                ClearConnectionSelection();
            }
        }

        private void DrawConnectionLine(Event e)
        {
            if (selectedInNode != null && selectedOutNode == null)
            {
                Handles.DrawBezier(
                    selectedInNode.Rect.center,
                    e.mousePosition,
                    selectedInNode.Rect.center,
                    e.mousePosition,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }
        }

        private void ProcessEvent(Event e)
        {
            if (e.type == EventType.MouseDown)
            {
                if (e.button == 0)
                {
                    ClearConnectionSelection();
                }
            }
            else if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.Escape)
                {
                    ClearConnectionSelection();
                }
            }
        }

        private void CreateConnection()
        {
            new Connection(selectedInNode, selectedOutNode);
        }

        private void ClearConnectionSelection()
        {
            selectedInNode = null;
            selectedOutNode = null;
            IsPerformingConnection = false;
        }
    }
}
