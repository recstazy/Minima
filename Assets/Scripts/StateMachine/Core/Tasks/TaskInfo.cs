using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Minima.StateMachine
{
    [Serializable]
    public class TaskInfo
    {
        #region Fields

        [SerializeField]
        private string typeName;

        [SerializeField]
        private string json;

        #endregion

        #region Properties

        public string TypeName { get => typeName; }

        #endregion

        public TaskInfo(Task task)
        {
            UpdateInfo(task);
        }

        public void UpdateInfo(Task task)
        {
            json = JsonUtility.ToJson(task);
            task.TaskInfo = this;
            var taskType = task.GetType();
            typeName = taskType.FullName;
        }

        public Task CreateTaskInstance()
        {
            var task = JsonUtility.FromJson(json, Type.GetType(typeName)) as Task;
            task.TaskInfo = this;
            return task;
        }
    }
}
