using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.AI
{
    public class AIWaitTime : AIUpdateTask
    {
        #region Fields

        [SerializeField]
        private float seconds = 1f;

        #endregion

        #region Properties

        #endregion

        public override void OnTaskEnter()
        {
            updateTime = seconds;
        }

        public override void OnTaskTimedUpdate()
        {
            EndTask(true);
        }
    }
}
