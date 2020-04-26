using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.StateMachine
{
    public sealed class EntryNode : Node
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        public EntryNode() : base(NodeType.State)
        {
            AddTask(new EntryTask());
            tasksEditable = false;
            Title = "Entry";
        }

        protected override void InitInOut()
        {
            maxInputs = 0;
            maxOutputs = 1;
        }
    }
}
