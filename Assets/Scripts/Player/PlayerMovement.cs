using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class PlayerMovement : MovementComponent
{
    #region Fields

    [SerializeField]
    private JoysticInputHandler movementHandler;

    [SerializeField]
    private JoysticInputHandler rotationHandler;

    private InputHandler inputHandler;
    private bool isButtonInputActive;

    #endregion

    #region Properties
    
    #endregion

    override protected void Start()
    {
        base.Start();

        inputHandler = GetComponent<InputHandler>();
        inputHandler.OnInputChanged += ButtonInputChanged;

        if (movementHandler != null)
        {
            movementHandler.OnAxisChanged += MovementJoysticAxisChanged;
        }
    }

    private void OnDestroy()
    {
        inputHandler.OnInputChanged -= ButtonInputChanged;

        if (movementHandler != null)
        {
            movementHandler.OnAxisChanged -= MovementJoysticAxisChanged;
        }
    }

    private void MovementJoysticAxisChanged(Vector2 axisValue)
    {
        if (!isButtonInputActive)
        {
            if (axisValue.sqrMagnitude > Vector2.one.sqrMagnitude)
            {
                MoveOnDirection(axisValue.normalized);
            }
            else
            {
                StopMoving();
            }
        }
    }

    private void ButtonInputChanged(Vector2 direction, bool isActive)
    {
        isButtonInputActive = isActive;

        if (isActive)
        {
            MoveOnDirection(direction);
        }
        else
        {
            StopMoving();
        }
    }

}
