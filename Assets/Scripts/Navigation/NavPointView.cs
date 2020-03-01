using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavPointView : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private SpriteRenderer sprite;

    #endregion

    #region Properties
    
    #endregion

    public void SetColor(Color color)
    {
        sprite.color = color;
    }
}
