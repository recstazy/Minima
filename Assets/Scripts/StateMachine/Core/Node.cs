using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Minima.StateMachine
{
    public enum NodeType { State, Condition }

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
        private TaskInfo[] taskInfo = new TaskInfo[0];

        [NonSerialized]
        private Task[] tasks;

        #endregion

        #region Properties

        public uint ID { get => id; }
        public NodeType NodeType { get => nodeType; set => nodeType = value; }
        public Vector2 Position { get => position; set => position = value; }
        public Task[] Tasks { get => tasks; private set => tasks = value; }
        public uint[] Connections { get => connections; private set => connections = value; }
        public TaskInfo[] TaskInfo { get => taskInfo; }

        #endregion

        public Node()
        {
            tasks = new Task[0];
        }

        public void SetId(uint id)
        {
            this.id = id;
        }

        public void SetTasks(Task[] tasks)
        {
            this.tasks = tasks;
        }

        public void AddTask(Task task)
        {
            tasks = tasks.ConcatOne(task);

            if (task.TaskInfo == null)
            {
                taskInfo = taskInfo.ConcatOne(new TaskInfo(task));
            }
        }

        public void RemoveTask(Task task)
        {
            tasks = tasks.Remove(task);
            taskInfo = taskInfo.Remove(task.TaskInfo);
        }

        public void AddConnectionTo(Node node)
        {
            if (node.ID != this.id && !connections.Contains(node.ID))
            {
                connections = connections.ConcatOne(node.ID);
            }
        }

        public void RemoveConnectionTo(Node node)
        {
            if (connections.Contains(node.ID))
            {
                connections = connections.Remove(node.ID);
            }
        }

        ~Node()
        {
            tasks = null;
        }
    }
}
