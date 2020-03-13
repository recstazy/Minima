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

            //var taskFields = taskType.GetFields()
            //    .Where(f => f.GetCustomAttribute<SerializeField>() != null)
            //    .ToArray();

            //foreach (var f in taskFields)
            //{
            //    fields = fields.ConcatOne(new TaskFieldStruct(f.Name, f.GetValue(task)));
            //}
        }

        public Task CreateTaskInstance()
        {
            //var task = Activator.CreateInstance(Type.GetType(typeName));

            //foreach (var f in fields)
            //{
            //    var value = Activator.CreateInstance(Type.GetType(f.type));
            //    value = f.value;
            //    task.GetType().GetField(f.name).SetValue(task, value);
            //}

            //return task as Task;

            var task = JsonUtility.FromJson(json, Type.GetType(typeName));
            var type = Type.GetType(typeName);
            return task as Task;
        }
    }
}
