using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.AI
{
    public class EnemyAI : AIBase
    {
        #region Fields

        [SerializeField]
        private TriggerDelegate agressionTrigger;

        [SerializeField]
        private TriggerDelegate shootTrigger;

        #endregion

        #region Properties

        #endregion

        protected virtual void Awake()
        {
            BindTriggers(true);
        }

        private void OnDestroy()
        {
            BindTriggers(false);
        }

        protected virtual void ShootTriggerCallback(GameObject target)
        {
            AIControlled.Blackboard.TargetCharacter = target.GetCharacter();
        }

        protected virtual void AgressionTriggerCallback(GameObject target)
        {
            AIControlled.MoveTo(target.transform);
        }

        protected virtual void BindTriggers(bool newBinded)
        {
            if (newBinded)
            {
                if (agressionTrigger != null)
                {
                    agressionTrigger.OnTargetTriggered += AgressionTriggerCallback;
                }

                if (shootTrigger != null)
                {
                    shootTrigger.OnTargetTriggered += ShootTriggerCallback;
                }
            }
            else
            {
                if (agressionTrigger != null)
                {
                    agressionTrigger.OnTargetTriggered -= AgressionTriggerCallback;
                }

                if (shootTrigger != null)
                {
                    shootTrigger.OnTargetTriggered -= ShootTriggerCallback;
                }
            }
        }
    }
}
