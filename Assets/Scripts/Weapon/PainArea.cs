using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PainArea : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private float dmgPerSecond;

    [SerializeField]
    private List<TargetType> targets = new List<TargetType>();

    private List<Character> damageTargets = new List<Character>();

    #endregion

    #region Properties

    #endregion

    private void Update()
    {
        DamageTargets();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var character = GetCharacter(collision);

        if (character != null)
        {
            damageTargets.Add(character);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        var character = GetCharacter(collision);

        if (character != null)
        {
            damageTargets.Remove(character);
        }
    }

    private void DamageTargets()
    {
        foreach (var t in damageTargets)
        {
            t.ApplyDamage(dmgPerSecond * Time.deltaTime);
        }
    }

    private Character GetCharacter(Collision2D collision)
    {
        var damageTarget = (TargetType)collision.gameObject.layer;

        if (targets.Contains(damageTarget))
        {
            var character = collision.gameObject.GetComponent<Character>();
            return character;
        }

        return null;
    }
}
