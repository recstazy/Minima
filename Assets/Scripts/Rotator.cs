using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private float rotationSpeed = 5f;

    private Transform thisTransform;

    #endregion

    #region Properties
    
    #endregion

    void Start()
    {
        thisTransform = transform;
    }

    void Update()
    {
        if (thisTransform != null)
        {
            thisTransform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }
}
