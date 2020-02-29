using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.AI
{
    public class IsTargetVisible : AITask, ICallableTask
    {
        [SerializeField]
        private bool checkOnEnter = true;

        [SerializeField]
        private TargetType[] ignoreTypes;

        private Transform targetTransform;

        public override void OnTaskEnter()
        {
            targetTransform = BlackBoard.TargetTransform.transform;

            if (checkOnEnter)
            {
                CheckVisibility();
            }
        }

        private void CheckVisibility()
        {
            if (BlackBoard != null && BlackBoard.TargetTransform != null)
            {
                bool isVisible = Helpers.CheckVisibility(thisTransform.position, targetTransform.position, ignoreTypes);
                EndTask(isVisible);
            }
        }

        public void Call()
        {
            CheckVisibility();
        }
    }
}
