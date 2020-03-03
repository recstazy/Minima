using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.StateMachine
{
    public class NodesContainer
    {
        #region Fields

        private Node[] nodes = new Node[0];

        #endregion

        #region Properties

        public Node[] Nodes { get => nodes; }

        #endregion

        public void CreateNode(Vector2 position)
        {
            var node = new Node(position, 50, 50, NodeStyle.GetStyle());
            nodes = nodes.ConcatOne(node);
        }
    }
}
