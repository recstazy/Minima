using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Minima.StateMachine
{
    public enum NodeType { Task, Condition }

    [Serializable]
    public class Node
    {
        #region Fields

        [SerializeField]
        private uint id;

        [SerializeField]
        private NodeType nodeType;

        [SerializeField]
        private Vector2 position;

        [SerializeField]
        private uint[] connections = new uint[0];

        [SerializeField]
        private int connectionsCount;

        [SerializeField]
        private int tasksCount;

        [SerializeField]
        private Task[] tasks = new Task[0];

        #endregion

        #region Properties

        public uint ID { get => id; }
        public NodeType NodeType { get => nodeType; set => nodeType = value; }
        public Vector2 Position { get => position; set => position = value; }
        public Task[] Tasks { get => tasks; private set => tasks = value; }
        public uint[] Connections { get => connections; private set => connections = value; }

        #endregion

        public void SetId(uint id)
        {
            this.id = id;
        }

        public void AddTask(Task task)
        {
            tasks = tasks.ConcatOne(task);
            tasksCount = tasks.Length;
        }

        public void RemoveTask(Task task)
        {
            tasks = tasks.Remove(task);
            tasksCount = tasks.Length;
        }

        public void AddConnectionTo(Node node)
        {
            if (node.ID != this.id && !connections.Contains(node.ID))
            {
                connections = connections.ConcatOne(node.ID);
            }

            connectionsCount = connections.Length;
        }

        public void RemoveConnectionTo(Node node)
        {
            if (connections.Contains(node.ID))
            {
                connections = connections.Remove(node.ID);
            }

            connectionsCount = connections.Length;
        }

        //private void ParseTasks()
        //{
        //    if (taskInfo.Length > 0)
        //    {
        //        foreach (var t in taskInfo)
        //        {
        //            var type = Type.GetType(t.TypeName);

        //            if (type != null && type == typeof(Task))
        //            {
        //                var task = Activator.CreateInstance(type) as Task;

        //                foreach (var f in t.Fields)
        //                {
        //                    var field = type.GetField(f.name);
        //                    field.SetValue(task, GetValue(field.FieldType, f.value));
        //                }

        //                tasks = tasks.ConcatOne(task);
        //            }
        //        }
        //    }

        //    tasksCount = tasks.Length;
        //}

        private object GetValue(Type type, string value)
        {
            if (type == typeof(string))
            {
                return value;
            }
            else if (type == typeof(int))
            {
                return int.Parse(value, System.Globalization.NumberStyles.Integer);
            }
            else if (type == typeof(float))
            {
                return float.Parse(value, System.Globalization.NumberStyles.Float);
            }
            else if (type == typeof(double))
            {
                return double.Parse(value, System.Globalization.NumberStyles.Number);
            }
            else if (type == typeof(bool))
            {
                return bool.Parse(value);
            }

            return value;
        }
    }
}
