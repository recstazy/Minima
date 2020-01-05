using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public delegate void InputChangedHandler(Vector2 direction, bool isActive);
    public event InputChangedHandler OnInputChanged;

    public event System.Action OnActionPressed;

    void Update()
    {
        ProcessInput();
    }

    protected virtual void ProcessInput()
    {
        #region Movement

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnInputChanged?.Invoke(Vector2.up, true);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnInputChanged?.Invoke(Vector2.down, true);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnInputChanged?.Invoke(Vector2.right, true);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnInputChanged?.Invoke(Vector2.left, true);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            OnInputChanged?.Invoke(Vector2.up, false);
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            OnInputChanged?.Invoke(Vector2.down, false);
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            OnInputChanged?.Invoke(Vector2.right, false);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            OnInputChanged?.Invoke(Vector2.left, false);
        }

        #endregion

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnActionPressed?.Invoke();
        }
    }
}
