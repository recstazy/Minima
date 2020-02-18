using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavPointView : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private bool showDebug = false;

    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
    private Text text;

    #endregion

    #region Properties
    
    #endregion

    void Start()
    {
        if (!showDebug)
        {
            text.transform.parent.gameObject.SetActive(false);
        }
    }

    public void SetText(string value)
    {
        text.text = value;
    }

    public void SetColor(Color color)
    {
        sprite.color = color;
    }
}
