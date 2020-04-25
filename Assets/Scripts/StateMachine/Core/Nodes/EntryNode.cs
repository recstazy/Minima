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

        public EntryNode()
        {
            AddTask(new EntryTask());
            tasksEditable = false;
            Title = "Entry";
        }
    }
}
