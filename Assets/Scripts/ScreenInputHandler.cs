using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenInputHandler : InputHandler
{
    #region Fields

    [SerializeField]
    private bl_Joystick movementStick;

    #endregion

    #region Properties
    
    public Vector2 Axis { get => new Vector2(movementStick.Horizontal, movementStick.Vertical); }

    #endregion

    protected override void Update()
    {
        base.Update();
        ProcessAxis();
    }

    public void ActionClicked()
    {
        CallActionPressed();
    }

    private void ProcessAxis()
    {
        if (!KeyboardActive)
        {
            CallInputChanged(Axis, true);
        }
    }
}
