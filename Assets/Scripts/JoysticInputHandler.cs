using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoysticInputHandler : MonoBehaviour
{
    public delegate void AxisHandler(Vector2 axisValue);
    public event AxisHandler OnAxisChanged;

    #region Fields

    [SerializeField]
    private bl_Joystick stick;

    #endregion

    #region Properties
    
    public Vector2 Axis { get => new Vector2(stick.Horizontal, stick.Vertical); }

    #endregion

    private void Update()
    {
        OnAxisChanged?.Invoke(Axis);
    }
}
