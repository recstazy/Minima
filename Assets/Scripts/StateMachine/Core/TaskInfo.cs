using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

namespace Minima.StateMachine
{
    [Serializable]
    public class TaskInfo
    {
        #region Fields

        [SerializeField]
        private string typeName;

        [SerializeField]
        private TaskFieldStruct[] fields = new TaskFieldStruct[0];

        [SerializeField]
        private string json;

        #endregion

        #region Properties

        public TaskFieldStruct[] Fields { get => fields; }
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
