using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private Transform followObject;

    [SerializeField]
    private float forwardOffset = 2f;

    [SerializeField]
    private bool enableOffset;

    [SerializeField]
    [Range(0.1f, 1f)]
    private float lerpAlpha = 0.5f;

    [SerializeField]
    private float height = 10;

    private Transform thisTransform;
    private Rigidbody2D followBody;
    private bool useOffset = false;

    #endregion

    #region Properties

    public Transform FollowedObject { get => followObject; }

    #endregion

    private void Start()
    {
        thisTransform = transform;

        if (enableOffset)
        {
            followBody = followObject.GetComponent<Rigidbody2D>();
            useOffset = followBody != null;
        }
    }

    private void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        Vector3 targetPosition;

        if (useOffset)
        {
            Vector2 targetPoint = 
                new Vector2(followObject.position.x, followObject.position.y) 
                + followBody.velocity.normalized * forwardOffset;

            targetPosition = new Vector3(targetPoint.x, targetPoint.y, -height);
        }
        else
        {
            targetPosition = new Vector3(followObject.position.x, followObject.position.y, -height);
        }

        thisTransform.position = Vector3.Lerp(thisTransform.position, targetPosition, lerpAlpha);
    }
}
