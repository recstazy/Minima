using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.AI
{
    public class AIMoveToTarget : AITask
    {
        [SerializeField]
        private MovementType movementType = MovementType.Path;

        public override void OnTaskEnter()
        {
            if (BlackBoard.TargetCharacter != null)
            {
                aiControlled.StopMovement();
                aiControlled.MoveTo(BlackBoard.TargetCharacter.transform, movementType, TargetReached);
            }
        }

        private void TargetReached()
        {
            EndTask();
        }
    }
}
