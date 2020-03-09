using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        private Task[] tasks = new Task[0];

        [SerializeField]
        private uint[] connections = new uint[0];

        [SerializeField]
        private int connectionsCount;

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
        }

        public void RemoveTask(Task task)
        {
            tasks = tasks.Remove(task);
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
    }
}
