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
            var taskType = task.GetType();
            typeName = taskType.FullName;

            var taskFields = taskType.GetFields()
                .Where(f => f.GetCustomAttribute<NodeEditable>() != null)
                .ToArray();

            foreach (var f in taskFields)
            {
                fields = fields.ConcatOne(new TaskFieldStruct(f.Name, f.GetValue(task)));
            }
        }
    }
}
