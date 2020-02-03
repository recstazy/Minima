using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.AI
{
    public class AIMoveToTarget : AITask
    {
        public override void OnTaskEnter()
        {
            if (BlackBoard.TargetCharacter != null)
            {
                aiControlled.StopMovement();
                aiControlled.MoveTo(BlackBoard.TargetCharacter.transform);
            }
        }
    }
}
