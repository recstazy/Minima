using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.AI
{
    public class AIUpdateTask : AITask
    {
        #region Fields

        [SerializeField]
        protected float updateTime = 0.25f;

        protected float timer = 0f;
        protected bool canUpdate = false;

        #endregion

        #region Properties

        #endregion

        public override void TaskUpdate()
        {
            if (canUpdate)
            {
                timer += Time.deltaTime;

                if (timer >= updateTime)
                {
                    timer = 0f;
                    OnTaskTimedUpdate();
                }
            }
        }

        public virtual void OnTaskTimedUpdate()
        {
        }
    }
}
