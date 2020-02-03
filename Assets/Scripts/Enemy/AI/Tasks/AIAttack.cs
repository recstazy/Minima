using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.AI
{
    public class AIAttack : AITask
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        public override void OnTaskEnter()
        {
            aiControlled.Attack();
        }
    }
}
