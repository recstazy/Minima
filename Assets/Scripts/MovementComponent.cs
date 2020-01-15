using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementComponent : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private float speed = 30f;

    [SerializeField]
    [Range(0.01f, 0.99f)]
    private float inertia = 0.5f;

    [SerializeField]
    protected bool rotateToDirection = true;

    protected Vector2 direction;
    new private Rigidbody2D rigidbody;
    protected Transform thisTransform;
    protected bool canMove = true;

    private Vector2 movingDirection;

    #endregion

    #region Properties

    public Vector2 CurrentDirection { get => direction; }

    #endregion

    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        thisTransform = transform;
    }

    protected virtual void Update()
    {
        UpdateSpeed();
        UpdateRotation();
    }

    public void MoveOnDirection(Vector2 newDirection)
    {
        SetDirection(newDirection);
    }

    public virtual void StopMoving()
    {
        SetDirection(Vector2.zero);
    }

    public virtual void SetCanMove(bool newCanMove)
    {
        canMove = newCanMove;

        if (!canMove)
        {
            StopMoving();
        }
    }

    protected void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }

    private void UpdateSpeed()
    {
        if (canMove)
        {
            movingDirection = direction;
        }
        else
        {
            movingDirection = Vector2.zero;
        }

        rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, movingDirection.normalized * speed, 1 - inertia);
    }

    private void UpdateRotation()
    {
        if (rotateToDirection && direction != Vector2.zero)
        {
            thisTransform.right = direction.normalized;
            Debug.Log(direction);
        }
    }
}
