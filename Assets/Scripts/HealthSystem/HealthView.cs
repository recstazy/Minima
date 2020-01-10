using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthView : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private HealthSystem healthSystem;

    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
    private Color minHealthColor;

    private Color maxHealthColor;

    #endregion

    #region Properties

    #endregion

    private void Awake()
    {
        maxHealthColor = sprite.color;
    }

    private void Start()
    {
        healthSystem.OnHealthChanged += UpdateColor;
    }

    private void OnDestroy()
    {
        healthSystem.OnHealthChanged -= UpdateColor;
    }

    private void UpdateColor()
    {
        var alpha = healthSystem.CurrentHealth / healthSystem.MaxHealth;
        var color = Color.Lerp(minHealthColor, maxHealthColor, alpha);
        sprite.color = color;

        StartCoroutine(BlinkWhite(color));
    }

    private IEnumerator BlinkWhite(Color targetColor)
    {
        for (int i = 0; i < 4; i++)
        {
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            sprite.color = targetColor;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
