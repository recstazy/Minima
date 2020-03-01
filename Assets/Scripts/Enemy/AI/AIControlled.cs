using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.AI
{
    public class AIControlled : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private WeaponComponent weapon;

        [SerializeField]
        private PathMovement movementComp;

        [SerializeField]
        private Animator stateMachine;

        #endregion

        #region Properties

        public WeaponComponent Weapon { get => weapon; }
        public MovementComponent MovementComp { get => movementComp; }
        public AIBlackboard Blackboard { get; private set; } = new AIBlackboard();

        #endregion

        public void SetBlackboardValue(string name, bool value)
        {
            stateMachine.SetBool(name, value);
        }

        public void SetBlackboardTrigger(string name)
        {
            stateMachine.SetTrigger(name);
        }

        public virtual void MoveTo(Transform target, MovementType movementType)
        {
            if (movementComp != null)
            {
                movementComp.MoveToTarget(target, movementType);
            }
        }

        public virtual void BindMovement(System.Action<bool> movementStopCallback)
        {
            movementComp.BindMovementStop(movementStopCallback);
        }

        public void StopMovement()
        {
            if (movementComp != null)
            {
                movementComp.StopMoving();
            }
        }

        public void ContinueMovement()
        {
            if (movementComp != null)
            {
                movementComp.SetCanMove(true);
            }
        }

        public virtual void Attack(Character target = null)
        {
            if (Weapon != null)
            {
                Weapon.UseWeapon();
            }
        }
    }
}
