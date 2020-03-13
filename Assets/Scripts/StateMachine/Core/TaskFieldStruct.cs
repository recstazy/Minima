using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.StateMachine
{
    [System.Serializable]
    public struct TaskFieldStruct
    {
        #region Fields

        [SerializeField]
        public string type;

        [SerializeField]
        public string name;

        [SerializeField]
        public string value;

        #endregion

        public TaskFieldStruct(string name, object value)
        {
            this.name = name;
            this.value = value.ToString();
            type = typeof(object).ToString();
        }
    }
}
