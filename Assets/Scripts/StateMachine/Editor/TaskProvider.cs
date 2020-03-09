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

        #endregion

        #region Properties

        public bool IsActive { get; set; } = true;

        #endregion

        #region Static

        private static GUIContent[] menu;
        private static Type[] taskTypes;

        public static Type[] GetTaskTypes(NodeType nodeType)
        {
            var rawTypes = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetTypes());

            var types = new Type[0];

            foreach (var t in rawTypes)
            {
                types = types.Concat(t).ToArray();
            }

            types = types.Where(t => t.IsSubclassOf(typeof(Task))).ToArray();
            return types;
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void ScriptsReloaded()
        {
            CreateDropDownMenu();
        }

        private static void CreateDropDownMenu()
        {
            var types = GetTaskTypes(NodeType.Task);

            menu = null;
            taskTypes = null;

            menu = new GUIContent[types.Length + 1];
            menu[0] = new GUIContent("None");
            
            for (int i = 0; i < types.Length; i++)
            {
                menu[i + 1] = new GUIContent(types[i].Name);
            }

            taskTypes = new Type[1];
            taskTypes = taskTypes.Concat(types).ToArray();
        }

        #endregion

        public TaskProvider(IGraphObject parent) : base(parent) 
        {
            if (menu == null)
            {
                CreateDropDownMenu();
            }

            DefaultSize = new Vector2(DefaultSize.x, 20f);
        }

        public override void Draw()
        {
            if (IsActive)
            {
                UpdateRect();

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
            if (currentIndex != 0 && currentIndex < taskTypes.Length)
            {
                var task = CreateTask(taskTypes[currentIndex]);
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
            dropDownRect.center = rect.center;
            dropDownRect.width = rect.width;
            dropDownRect.height = menu.Length * 15;
        }

        protected override void CreateStyle()
        {
            style = (GUIStyle)"GV Gizmo DropDown";
        }
    }
}
