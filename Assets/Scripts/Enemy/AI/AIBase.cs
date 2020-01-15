using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private AIControlled aiControlled;

    #endregion

    #region Properties
    
    public AIControlled AIControlled { get => aiControlled; }

    #endregion
}
