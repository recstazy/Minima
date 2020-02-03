using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.AI
{
    public class SetBlackboardTargetByTrigger : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private AIControlled aIControlled;

        [SerializeField]
        private TriggerDelegate trigger;

        #endregion

        private void Awake()
        {
            trigger.OnTargetTriggered += SetValue;
        }

        private void OnDestroy()
        {
            trigger.OnTargetTriggered -= SetValue;
        }

        private void SetValue(GameObject target)
        {
            aIControlled.Blackboard.TargetCharacter = target.GetCharacter();
        }
    }
}
