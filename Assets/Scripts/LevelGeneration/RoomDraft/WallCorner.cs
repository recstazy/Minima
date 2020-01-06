using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCorner : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private bool isExit;

    private Transform thisTransform;

    #endregion

    #region Properties
    
    public Vector3 position { get => ThisTransform.position; set => ThisTransform.position = value; }
    public Vector3 localPosition { get => ThisTransform.localPosition; set => ThisTransform.localPosition = value; }
    public Quaternion rotation { get => ThisTransform.rotation; set => ThisTransform.rotation = value; }
    public Vector3 localScale { get => ThisTransform.localScale; set => ThisTransform.localScale = value; }

    public bool IsExit { get => isExit; }
    public WallCorner NearestCorner { get; set; }

    public Transform ThisTransform
    {
        get
        { 
            if (thisTransform == null)
            {
                thisTransform = transform;
            }

            return thisTransform;
        }
    }

    #endregion

}
