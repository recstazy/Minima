using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.AI
{
    public class AIBehaviorLooper : AIUpdateTask
    {
        #region Fields

        [SerializeField]
        private AITask taskToLoop;

        private ICallableTask task;

        #endregion

        #region Properties

        #endregion

        public override void OnTaskEnter()
        {
            if (taskToLoop != null && taskToLoop is ICallableTask)
            {
                task = taskToLoop as ICallableTask;
                canUpdate = true;
            }
            else if (taskToLoop != null)
            {
                Debug.LogError("AIBehaviorLooper: Task must implement ICallableTask interface");
            }
        }

        public override void OnTaskTimedUpdate()
        {
            task.Call();
        }
    }
}
