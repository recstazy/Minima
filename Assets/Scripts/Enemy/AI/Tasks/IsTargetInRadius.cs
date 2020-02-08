using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.AI
{
    public class IsTargetInRadius : AIUpdateTask
    {
        [SerializeField]
        private float radius = 2f;

        private Transform targetTransform;
        
        public override void OnTaskEnter()
        {
            if (BlackBoard.TargetCharacter != null)
            {
                targetTransform = BlackBoard.TargetCharacter.transform;
                canUpdate = true;
            }
        }

        public override void OnTaskTimedUpdate()
        {
            CheckDistance();
        }

        private void CheckDistance()
        {
            float distance = Vector2.Distance(thisTransform.position, targetTransform.position);

            if (distance < radius)
            {
                EndTask(true);
            }
        }
    }
}
