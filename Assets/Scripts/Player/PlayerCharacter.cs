using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IDamageble
{
    #region Fields

    [SerializeField]
    private HealthSystem healthSystem;

    #endregion

    #region Properties

    #endregion

    public void ApplyDamage(float amount)
    {
        healthSystem.ApplyDamage(amount);
    }
}
