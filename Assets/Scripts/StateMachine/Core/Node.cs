using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Minima.StateMachine
{
    public enum NodeType { Task, Condition }

    [System.Serializable]
    public class Node
    {
        #region Fields

        //[SerializeField]
        private NodeType nodeType;

        //[SerializeField]
        private Vector2 position;

        //[SerializeField]
        private Task[] tasks = new Task[0];

        //[SerializeField]
        private Node[] connections = new Node[0];

        [SerializeField]
        private int connectionsCount;

        #endregion

        #region Properties

        public NodeType NodeType { get => nodeType; set => nodeType = value; }
        public Vector2 Position { get => position; set => position = value; }
        public Task[] Tasks { get => tasks; private set => tasks = value; }
        public Node[] Connections { get => connections; private set => connections = value; }

        #endregion

        public void AddTask(Task task)
        {
            tasks = tasks.ConcatOne(task);
        }

        public void RemoveTask(Task task)
        {
            tasks = tasks.Remove(task);
        }

        public void UpdateConnections(Node[] connections)
        {
            this.connections = connections;
            connectionsCount = connections.Length;
        }

        public void AddConnectionTo(Node node)
        {
            connections = connections.ConcatOne(node);
            connectionsCount = connections.Length;
        }

        public void RemoveConnectionTo(Node node)
        {
            connections = connections.Remove(node);
            connectionsCount = connections.Length;
        }
    }
}
