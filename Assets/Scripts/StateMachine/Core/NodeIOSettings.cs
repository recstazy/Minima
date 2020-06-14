using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NodeIOSettings
{
    #region Properties

    public int MaxInputStates { get; set; }
    public int MaxOutputStates { get; set; }
    public int MaxInputConditions { get; set; }
    public int MaxOutputConditions { get; set; }
    public bool OneTypePerTime { get; set; }

    #endregion
}
