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

        [SerializeField]
        protected bool shouldUpdate = false;

        protected Animator animator;
        protected AIControlled aiControlled;
        protected Transform thisTransform;

        protected bool canCallUpdate = false;

        #endregion

        #region Properties

        protected AIBlackboard BlackBoard 
        { 
            get
            {
                if (aiControlled != null)
                {
                    return aiControlled.Blackboard;
                }

                return null;
            }
        }

        #endregion

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (aiControlled == null)
            {
                this.animator = animator;
                thisTransform = animator.transform;
                aiControlled = animator.GetComponent<AIControlled>();
            }

            canCallUpdate = true;
            OnTaskEnter();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            canCallUpdate = false;
            OnTaskExit();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (shouldUpdate && canCallUpdate)
            {
                TaskUpdate();
            }
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

        public virtual void EndTask()
        {
            animator.SetTrigger(animatorValueName);
        }
    }
}
