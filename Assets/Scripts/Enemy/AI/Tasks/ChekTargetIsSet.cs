using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.AI
{
    public class ChekTargetIsSet : AIUpdateTask
    {
        public override void OnTaskEnter()
        {
            CheckTarget();
            canUpdate = true;
        }

        public override void OnTaskTimedUpdate()
        {
            CheckTarget();
        }

        private void CheckTarget()
        {
            EndTask(BlackBoard.TargetTransform != null);
        }
    }
}
