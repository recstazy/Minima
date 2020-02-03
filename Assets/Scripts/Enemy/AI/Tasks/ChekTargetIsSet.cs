using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.AI
{
    public class ChekTargetIsSet : AIUpdateTask
    {
        public override void OnTaskEnter()
        {
            canUpdate = true;
        }

        public override void OnTaskTimedUpdate()
        {
            if (BlackBoard.TargetCharacter != null)
            {
                EndTask(true);
            }
        }
    }
}
