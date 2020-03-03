using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Minima.StateMachine
{
    public class StateMachineWindow : EditorWindow
    {
        #region Fields

        private NodesContainer nodes = new NodesContainer();

        #endregion

        #region Properties

        #endregion

        [MenuItem("Window/State machine/State machine window")]
        private static void OpenWindow()
        {
            StateMachineWindow window = GetWindow<StateMachineWindow>();
            window.titleContent = new GUIContent("State machine window");
        }

        private void OnGUI()
        {
            DrawNodes();
            ProcessEvents(Event.current);
            DrawButtons();

            if (GUI.changed)
            {
                Repaint();
            }
        }

        private void DrawNodes()
        {
            foreach (var n in nodes.Nodes)
            {
                n.Draw();
            }
        }

        private void ProcessEvents(Event e)
        {
            
        }

        private void DrawButtons()
        {
            if (GUI.Button(new Rect(50, 50, 100, 100), "button"))
            {
                nodes.CreateNode(new Vector2(100, 100));
                Debug.Log(nodes.Nodes.Length);
            }
        }
    }
}
