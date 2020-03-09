using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Minima.StateMachine.Editor
{
    public class TaskNode : Node
    {
        #region Fields

        private TaskProvider taskProvider;

        #endregion

        #region Properties

        #endregion

        public TaskNode(Vector2 position) : base(position) 
        {
            taskProvider = new TaskProvider(this);
            taskProvider.UseParentRectCenter = false;
            taskProvider.AutoUpdateSize = false;
            taskProvider.OnTaskSelected += TaskCreated;
            AddContent(taskProvider);
        }

        public override void Draw()
        {
            base.Draw();

            if (taskProvider != null)
            {
                taskProvider.Draw();
            }
        }

        private void TaskCreated(Task task)
        {
            var taskView = new TaskView(this, task);
            taskView.UseParentRectCenter = false;
            taskView.AutoUpdateSize = false;
            AddContent(taskView);
        }
    }
}
