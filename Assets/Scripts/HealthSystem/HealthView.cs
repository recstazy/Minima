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
    private Coroutine blinkCoroutine;

    #endregion

    #region Properties

    #endregion

    private void Awake()
    {
        maxHealthColor = sprite.color;
    }

    private void Start()
    {
        healthSystem.OnHealthChanged += HealthChanged;
    }

    private void OnDestroy()
    {
        healthSystem.OnHealthChanged -= HealthChanged;
    }

    private void HealthChanged(HealthChangeType changeType)
    {
        var alpha = healthSystem.CurrentHealth / healthSystem.MaxHealth;
        var color = Color.Lerp(minHealthColor, maxHealthColor, Mathf.Pow(alpha, 2));
        sprite.color = color;

        if (changeType == HealthChangeType.Restore && blinkCoroutine == null)
        {
            blinkCoroutine = StartCoroutine(BlinkWhite(color));
        }  
    }

    private IEnumerator BlinkWhite(Color targetColor)
    {
        for (int i = 0; i < 4; i++)
        {
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.075f);
            sprite.color = targetColor;
            yield return new WaitForSeconds(0.075f);
        }

        blinkCoroutine = null;
    }
}
