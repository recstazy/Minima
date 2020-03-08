using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Minima.StateMachine
{
    [CreateAssetMenu(fileName = "NewStateMachine", menuName = "State Machine / New State Machine", order = 131)]
    public class StateMachine : ScriptableObject
    {
        #region Fields

        [SerializeField]
        private Node[] nodes;

        #endregion

        #region Properties

        #endregion
    }
}
