using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Minima.StateMachine
{
    public class StateNode : Node
    {
        #region Fields

        private NodeContent content;

        #endregion

        #region Properties

        #endregion

        public StateNode(Vector2 position) : base(position)
        {
            content = new NodeContent(this);
        }

        public override void Draw()
        {
            base.Draw();
            content.Draw();
        }
    }
}
