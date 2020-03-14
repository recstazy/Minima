using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;

namespace Minima.StateMachine
{
    [Serializable]
    public class Task
    {
        #region Fields

        #endregion

        #region Properties

        public TaskInfo TaskInfo { get; set; }

        #endregion

        public void UpdateInfo()
        {
            if (TaskInfo != null)
            {
                TaskInfo.UpdateInfo(this);
            }
            else
            {
                TaskInfo = new TaskInfo(this);
            }
        }
    }
}