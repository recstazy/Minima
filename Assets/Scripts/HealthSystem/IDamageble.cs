using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    HealthSystem HealthSystem { get; }
    void ApplyDamage(float amount);
}
