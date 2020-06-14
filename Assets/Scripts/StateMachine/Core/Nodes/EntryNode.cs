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
            var ioSettings = new NodeIOSettings();

            ioSettings.MaxInputStates = 0;
            ioSettings.MaxOutputStates = 1;

            ioSettings.MaxInputConditions = 0;
            ioSettings.MaxOutputConditions = 1;

            ioSettings.OneTypePerTime = true;
            IOSettings = ioSettings;
        }
    }
}
