using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Minima.StateMachine.Editor
{
    public class StateMachineEditor : EditorWindow
    {
        #region Fields

        private StateMachineIO stateMachineIO = new StateMachineIO();
        private List<Node> nodes = new List<Node>();
        private SMEditorEventArgs eventArgs;
        private NodeConnector nodeConnector;
        private GenericMenu contextMenu;

        private Vector2 lastMousePosition;
        private Vector2 offset;
        private Vector2 drag;
        private bool isDragging = false;
        private bool isHoldingRightMouse = false;

        #endregion

        #region Properties

        public bool IsPerformingConnection { get => nodeConnector.IsPerformingConnection; }

        #endregion

        [MenuItem("Window/State Machine Editor")]
        public static void OpenWindow()
        {
            StateMachineEditor window = GetWindow<StateMachineEditor>();
            window.titleContent = new GUIContent("State Machine Editor");
        }

        private void OnEnable()
        {
            eventArgs = new SMEditorEventArgs(this);
            nodeConnector = new NodeConnector();
            CreateContextMenu();
            OpenAssetData();
        }

        private void OnDisable()
        {
            foreach (var n in nodes)
            {
                UnbindFromNode(n);
                nodes = null;
            }
        }

        private void OnGUI()
        {
            lastMousePosition = Event.current.mousePosition;

            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);

            DrawNodes();

            ProcessNodeEvents(Event.current);
            nodeConnector.ProcessEvents(Event.current);
            ProcessEvents(Event.current);

            if (GUI.changed)
            {
                Repaint();
            }
        }

        private void OpenAssetData()
        {
            if (stateMachineIO.IsAssetOpened)
            {
                nodes = stateMachineIO.GetNodes().ToList();

                foreach (var n in nodes)
                {
                    BindToNode(n);
                }
            }
        }

        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            offset += drag * 0.5f;
            Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        private void DrawNodes()
        {
            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Draw();
                }
            }
        }

        private void ProcessEvents(Event e)
        {
            drag = Vector2.zero;

            switch (e.type)
            {
                case EventType.MouseDown:
                    {
                        if (e.button == 1)
                        {
                            isHoldingRightMouse = true;
                        }
                        break;
                    }
                case EventType.MouseDrag:
                    {
                        if (e.button == 1 && isHoldingRightMouse)
                        {
                            isDragging = true;
                            OnDrag(e.delta);

                        }
                        break;
                    }
                case EventType.MouseUp:
                    {
                        if (e.button == 1)
                        {
                            isHoldingRightMouse = false;

                            if (!isDragging)
                            {
                                contextMenu.ShowAsContext();
                            }

                            isDragging = false;
                        }
                        break;
                    }
            }
        }

        private void ProcessNodeEvents(Event e)
        {
            if (nodes != null && !isDragging)
            {
                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    bool guiChanged = nodes[i].ProcessEvents(e, eventArgs);

                    if (guiChanged)
                    {
                        GUI.changed = true;
                    }
                }
            }
        }

        private void OnDrag(Vector2 delta)
        {
            drag = delta;

            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Drag(delta);
                }
            }

            GUI.changed = true;
        }

        private void AddNodeClicked()
        {
            CreateNode(lastMousePosition);
        }

        private void CreateNode(Vector2 position)
        {
            var node = new TaskNode(position);

            nodes.Add(node);
            stateMachineIO.AddNode(node);
            BindToNode(node);
        }

        private void NodeRemoved(Node node)
        {
            nodes.Remove(node);
            stateMachineIO.RemoveNode(node);
            UnbindFromNode(node);
        }

        private void BindToNode(Node node)
        {
            node.OnConnectClicked += nodeConnector.ConnectNodeClicked;
            node.OnRemoveNode += NodeRemoved;
        }

        private void UnbindFromNode(Node node)
        {
            node.OnConnectClicked -= nodeConnector.ConnectNodeClicked;
            node.OnRemoveNode -= NodeRemoved;
        }

        private void CreateContextMenu()
        {
            contextMenu = new GenericMenu();
            contextMenu.AddItem(new GUIContent("Add node"), false, AddNodeClicked);
        }
    }
}