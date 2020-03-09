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
        private Node[] nodes = new Node[0];

        #endregion

        #region Properties

        public Node[] Nodes { get => nodes; }

        #endregion

        public void AddNode(Node node)
        {
            nodes = nodes.ConcatOne(node);
        }

        public void RemoveNode(Node node)
        {
            nodes = nodes.Remove(node);
        }
    }
}
