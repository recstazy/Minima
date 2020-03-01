using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public delegate void InputChangedHandler(Vector2 direction, bool isActive);
    public event InputChangedHandler OnInputChanged;

    public event System.Action OnActionPressed;

    #region Fields

    private int activeInputsCount = 0;
    private Vector2 currentDirection;

    [SerializeField]
    private KeyCode UpButton = KeyCode.W;

    [SerializeField]
    private KeyCode DownButton = KeyCode.S;

    [SerializeField]
    private KeyCode RightButton = KeyCode.D;

    [SerializeField]
    private KeyCode LeftButton = KeyCode.A;

    [SerializeField]
    private KeyCode ActionButton = KeyCode.Space;

    #endregion

    #region Properties

    public bool KeyboardActive { get => activeInputsCount > 0; }

    #endregion

    protected virtual void Update()
    {
        ProcessInput();
    }

    protected virtual void ProcessInput()
    {
        #region Movement

        if (Input.GetKeyDown(UpButton))
        {
            InputChanged(Vector2.up, true);
        }

        if (Input.GetKeyDown(DownButton))
        {
            InputChanged(Vector2.down, true);
        }

        if (Input.GetKeyDown(RightButton))
        {
            InputChanged(Vector2.right, true);
        }

        if (Input.GetKeyDown(LeftButton))
        {
            InputChanged(Vector2.left, true);
        }

        if (Input.GetKeyUp(UpButton))
        {
            InputChanged(Vector2.up, false);
        }

        if (Input.GetKeyUp(DownButton))
        {
            InputChanged(Vector2.down, false);
        }

        if (Input.GetKeyUp(RightButton))
        {
            InputChanged(Vector2.right, false);
        }

        if (Input.GetKeyUp(LeftButton))
        {
            InputChanged(Vector2.left, false);
        }

        #endregion

        if (Input.GetKeyDown(ActionButton))
        {
            CallActionPressed();
        }
    }

    private void InputChanged(Vector2 direction, bool isActive)
    {
        bool isActiveResult;

        if (isActive)
        {
            activeInputsCount++;
            currentDirection += direction;
            isActiveResult = true;
        }
        else
        {
            activeInputsCount--;

            if (activeInputsCount <= 0)
            {
                activeInputsCount = 0;
                currentDirection = Vector2.zero;
                isActiveResult = false;
            }
            else
            {
                currentDirection -= direction;
                isActiveResult = true;
            }
        }

        CallInputChanged(currentDirection, isActiveResult);
    }

    protected void CallInputChanged(Vector2 direction, bool isActive)
    {
        OnInputChanged?.Invoke(direction, isActive);
    }

    protected void CallActionPressed()
    {
        OnActionPressed?.Invoke();
    }
}
