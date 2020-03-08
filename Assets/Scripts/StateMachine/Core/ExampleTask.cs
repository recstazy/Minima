using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Minima.StateMachine;

public class ExampleTask : Task
{
    #region Fields

    [NodeEditable]
    public bool boolField;

    [NodeEditable]
    public int intField = 10;

    [NodeEditable]
    public float floatField = 10.2f;

    [NodeEditable]
    public string stringField = "qwerty";

    #endregion

    #region Properties

    #endregion

}
