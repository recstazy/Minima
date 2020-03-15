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
            //DeserializeUnityObjects(task);
            task.TaskInfo = this;
            return task;
        }

        private void DeserializeUnityObjects(Task task)
        {
            var type = Type.GetType(typeName);
            var serializedObjects = type.GetFields()
                .Where(f => f.GetCustomAttribute(typeof(SerializeField)) != null)
                .Where(f => typeof(UnityEngine.Object).IsAssignableFrom(f.FieldType));

            foreach (var o in serializedObjects)
            {
                var id = GetInstanceId(o.Name);
                var instance = EditorUtility.InstanceIDToObject(id);
                o.SetValue(task, instance);
            }
        }

        private int GetInstanceId(string name)
        {
            var leftPart = "\"" + name + "\":{\"instanceID\":";
            var regex = new Regex(leftPart + ".*}");
            var match = regex.Match(json);
            
            if (match.Success)
            {
                var cleared = match.Value
                    .Replace("}", "")
                    .Replace(leftPart, "");

                var id = int.Parse(cleared);
                return id;
            }
            return 0;
        }
    }
}
