using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;

namespace Minima.StateMachine.Editor
{
    public class TaskProvider : NodeContent
    {
        public event Action<Task> OnTaskSelected;

        #region Fields

        private int lastSelectedIndex = 0;
        private int currentIndex = 0;
        private Rect dropDownRect;
        private NodeType nodeType;

        #endregion

        #region Properties

        public bool IsActive { get; set; } = true;

        #endregion

        #region Static

        private static GUIContent[] stateMenu;
        private static GUIContent[] conditionMenu;
        private static Type[] stateTasks;
        private static Type[] conditionTasks;

        public static Type[] GetTaskTypes(NodeType nodeType)
        {
            var rawTypes = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetTypes());

            var types = new Type[0];

            foreach (var t in rawTypes)
            {
                types = types.Concat(t).ToArray();
            }

            types = types.Where(t => t.IsSubclassOf(typeof(Task))).ToArray();

            if (nodeType == NodeType.State)
            {
                return types.Where(t => t.IsSubclassOf(typeof(StateTask))).ToArray();
            }
            else if (nodeType == NodeType.Condition)
            {
                return types.Where(t => t.IsSubclassOf(typeof(ConditionTask))).ToArray();
            }

            return types.ToArray();
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void ScriptsReloaded()
        {
            CreateDropDownMenus();
        }

        private static void CreateDropDownMenus()
        {
            var stateTaskTypes = GetTaskTypes(NodeType.State);
            var conditionTaskTypes = GetTaskTypes(NodeType.Condition);

            CreateMenu(stateTaskTypes, ref stateMenu, ref stateTasks);
            CreateMenu(conditionTaskTypes, ref conditionMenu, ref conditionTasks);
        }

        private static void CreateMenu(Type[] tasks, ref GUIContent[] contentContainer, ref Type[] tasksContainer)
        {
            contentContainer = null;
            tasksContainer = null;

            contentContainer = new GUIContent[tasks.Length + 1];
            contentContainer[0] = new GUIContent("Add task");

            for (int i = 0; i < tasks.Length; i++)
            {
                contentContainer[i + 1] = new GUIContent(tasks[i].Name);
            }

            tasksContainer = new Type[1];
            tasksContainer = tasksContainer.Concat(tasks).ToArray();
        }

        #endregion

        public TaskProvider(IGraphObject parent, NodeType nodeType) : base(parent) 
        {
            if (stateMenu == null || conditionMenu == null)
            {
                CreateDropDownMenus();
            }

            this.nodeType = nodeType;
            DefaultSize = new Vector2(DefaultSize.x, 20f);
        }

        public override void Draw()
        {
            if (IsActive)
            {
                UpdateRect();
                var menu = GetMenu();

                if (menu != null)
                {
                    lastSelectedIndex = EditorGUI.Popup(rect, lastSelectedIndex, menu, style);

                    if (currentIndex != lastSelectedIndex)
                    {
                        currentIndex = lastSelectedIndex;
                        IndexChanged();
                        lastSelectedIndex = 0;
                    }
                }
            }
        }

        public override bool ProcessEvent(Event e, SMEditorEventArgs eventArgs)
        {
            return false;
        }

        private void IndexChanged()
        {
            var tasks = GetTasks();

            if (currentIndex != 0 && currentIndex < tasks.Length)
            {
                var task = CreateTask(tasks[currentIndex]);
                OnTaskSelected(task);
            }
        }

        private Task CreateTask(Type type)
        {
            var taskInstance = Activator.CreateInstance(type) as Task;
            return taskInstance;
        }

        protected override void UpdateRect()
        {
            base.UpdateRect();
            UpdateDropDownRect();
        }

        private void UpdateDropDownRect()
        {
            var menu = GetMenu();

            if (menu != null)
            {
                dropDownRect.center = rect.center;
                dropDownRect.width = rect.width;
                dropDownRect.height = menu.Length * 15;
            }
        }

        private Type[] GetTasks()
        {
            if (nodeType == NodeType.State)
            {
                return stateTasks;
            }
            else
            {
                return conditionTasks;
            }
        }

        private GUIContent[] GetMenu()
        {
            if (nodeType == NodeType.State)
            {
                return stateMenu;
            }
            else
            {
                return conditionMenu;
            }
        }

        protected override void CreateStyle()
        {
            style = (GUIStyle)"GV Gizmo DropDown";
        }
    }
}
