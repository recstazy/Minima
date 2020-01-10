using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public System.Action OnDeath;

    #region Fields

    [SerializeField]
    private float maxHealth = 100f;

    [SerializeField]
    private Character owner;

    private float _currentHealth;
    private bool isAlive = true;

    #endregion

    #region Properties

    public float MaxHealth { get => maxHealth; }
    public float CurrentHealth { get => _currentHealth; }
    public bool IsAlive { get => isAlive; }

    private float currentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Clamp(value, 0f, maxHealth);
        }
    }

    public Character Owner { get => owner; set => owner = value; }

    #endregion

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            isAlive = false;
            OnDeath?.Invoke();
        }
    }

    public void Restore(float amount)
    {
        if (isAlive)
        {
            currentHealth += amount;
        }
    }
}
