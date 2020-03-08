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
        }

        public override void Draw()
        {
            base.Draw();

            if (taskProvider != null)
            {
                taskProvider.Draw();
            }
        }
    }
}
