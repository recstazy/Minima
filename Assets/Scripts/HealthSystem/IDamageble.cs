using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    HealthSystem HealthSystem { get; }
    void ApplyDamage(float amount, Character from = null);
}
