using System.Collections;
using System.Reflection;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using Minima.StateMachine;

namespace Minima.StateMachine.Editor
{
    public class TaskView : ColumnContent
    {
        #region Fields

        private Type taskType;
        private Task task;
        private FieldInfo[] fields;

        #endregion

        #region Properties

        public Task Task { get => task; }

        #endregion

        public TaskView(IGraphObject parent, Task task) : base(parent)
        {
            taskType = task.GetType();
            this.task = task;
            Construct();
            CreateContextMenu();
        }

        private void Construct()
        {
            var title = new TextContent(this, taskType.Name);
            AddContent(title);

            fields = taskType.GetFields()
                .Where(f => f.GetCustomAttribute<NodeEditable>() != null)
                .ToArray();

            foreach (var f in fields)
            {
                var field = new TaskFieldContent(this, f, task);
                AddContent(field);
            }
        }

        public override bool ProcessEvent(Event e, SMEditorEventArgs eventArgs)
        {
            return base.ProcessEvent(e, eventArgs);
        }

        protected override void CreateContextMenu()
        {
            if (taskType != null)
            {
                contextMenu = new UnityEditor.GenericMenu();
                contextMenu.AddItem(new GUIContent("Remove " + taskType.Name), false, CallOnRemove);
            }
        }
    }
}
