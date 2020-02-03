using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace Minima.AI
{
    public enum ValueType { Trigger, Bool }

    public class AITask : StateMachineBehaviour
    {
        #region Fields

        [SerializeField]
        protected string animatorValueName;

        [SerializeField]
        protected ValueType valueType = ValueType.Bool;

        protected Animator animator;
        protected AIControlled aiControlled;

        #endregion

        #region Properties

        protected AIBlackboard BlackBoard { get => aiControlled.Blackboard; }

        #endregion

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (aiControlled == null)
            {
                this.animator = animator;
                aiControlled = animator.GetComponent<AIControlled>();
            }

            OnTaskEnter();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnTaskExit();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            TaskUpdate();
        }

        public virtual void OnTaskEnter()
        {
        }

        public virtual void OnTaskExit()
        {
        }

        public virtual void TaskUpdate()
        {
        }

        public virtual void EndTask(bool succeed)
        {
            switch (valueType)
            {
                case ValueType.Bool:
                    {
                        animator.SetBool(animatorValueName, succeed);
                        break;
                    }
                case ValueType.Trigger:
                    {
                        animator.SetTrigger(animatorValueName);
                        break;
                    }
            }
        }
    }
}
