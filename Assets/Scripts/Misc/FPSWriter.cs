using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSWriter : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private Text fpsText;

    #endregion

    #region Properties
    
    #endregion

    void Update()
    {
        fpsText.text = (1 / Time.deltaTime).ToString("F0");
    }
}
