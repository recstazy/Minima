using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Minima.StateMachine.Editor
{
    public class NodeConnector
    {
        #region Fields

        private Node selectedOutput;
        private Node selectedInput;

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

            if (selectedOutput == null)
            {
                if (CanConnect(clickedNode))
                {
                    selectedOutput = clickedNode;
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
            else
            {
                if (CanConnect(clickedNode))
                {
                    selectedInput = clickedNode;
                }
            }

            if (selectedInput != null)
            {
                if (selectedInput != selectedOutput)
                {
                    CreateConnection();
                }

                ClearConnectionSelection();
            }
        }

        private bool CanConnect(Node node)
        {
            var ioSettings = node.SMNode.IOSettings;

            if (selectedOutput == null)
            {
                bool outputStatesAvailable = node.OutputStates < ioSettings.MaxOutputStates;
                bool outputConditionsAvailable = node.OutputConditions < ioSettings.MaxOutputConditions;

                if (ioSettings.OneTypePerTime)
                {
                    return outputStatesAvailable && outputConditionsAvailable;
                }
                else
                {
                    return outputStatesAvailable || outputConditionsAvailable;
                }
            }
            else
            {
                var outIoSettings = selectedOutput.SMNode.IOSettings;

                bool inCanBeConnected = false;
                bool outCanBeConnected = false;

                if (node.NodeType == NodeType.State)
                {
                    if (selectedOutput.OutputStates < outIoSettings.MaxOutputStates)
                    {
                        outCanBeConnected = true;
                    }

                }
                else if (node.NodeType == NodeType.Condition)
                {
                    if (selectedOutput.OutputConditions < outIoSettings.MaxOutputConditions)
                    {
                        outCanBeConnected = true;
                    }
                }

                if (selectedOutput.NodeType == NodeType.State)
                {
                    if (node.InputStates < ioSettings.MaxInputStates)
                    {
                        inCanBeConnected = true;
                    }
                }
                else if (selectedOutput.NodeType == NodeType.Condition)
                {
                    if (node.InputConditions < ioSettings.MaxInputConditions)
                    {
                        inCanBeConnected = true;
                    }
                }

                return inCanBeConnected && outCanBeConnected;
            }

            return false;
        }

        private void DrawConnectionLine(Event e)
        {
            if (selectedOutput != null && selectedInput == null)
            {
                Handles.DrawBezier(
                    selectedOutput.Rect.center,
                    e.mousePosition,
                    selectedOutput.Rect.center,
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
            if (!selectedOutput.Outputs.Contains(selectedInput))
            {
                new Connection(selectedOutput, selectedInput);
            }
        }

        private void ClearConnectionSelection()
        {
            selectedOutput = null;
            selectedInput = null;
            IsPerformingConnection = false;
        }
    }
}
