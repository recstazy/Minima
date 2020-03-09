using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

namespace Minima.StateMachine
{
    [CreateAssetMenu(fileName = "NewStateMachine", menuName = "State Machine / New State Machine", order = 131)]
    public class StateMachine : ScriptableObject
    {
        #region Fields

        [SerializeField]
        private uint maxId;

        [SerializeField]
        private Node[] nodes;

        #endregion

        #region Properties

        public Node[] Nodes { get => nodes; }

        #endregion

        public void AddNode(Node node)
        {
            if (nodes == null)
            {
                nodes = new Node[0];
            }

            nodes = nodes.ConcatOne(node);
            maxId++;
            node.SetId(maxId);
        }

        public void RemoveNode(Node node)
        {
            if (nodes == null)
            {
                nodes = new Node[0];
            }

            nodes = nodes.Remove(node);
        }
    }
}
