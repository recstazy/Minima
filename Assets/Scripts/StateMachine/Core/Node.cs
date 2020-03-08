using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.StateMachine
{
    public enum NodeType { State, Condition }

    [System.Serializable]
    public class Node
    {
        #region Fields

        [SerializeField]
        private NodeType nodeType;

        [SerializeField]
        private Vector2 position;

        [SerializeField]
        private Task task;

        [SerializeField]
        private Node[] connections;

        #endregion

        #region Properties

        #endregion
    }
}
