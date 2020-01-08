using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaWeapon : WeaponComponent
{
    #region Fields

    [SerializeField]
    private DamageTarget damageTarget;

    [SerializeField]
    private ParticleSystem AreaParticles;

    [SerializeField]
    private CircleCollider2D damageCollider;

    [SerializeField]
    private float damage = 50;

    #endregion

    #region Properties
    
    #endregion

    public override void UseWeapon()
    {
        AreaParticles.Play();
        DamageArea();
    }

    void DamageArea()
    {
        List<Collider2D> overlapped = new List<Collider2D>();
        var filter = new ContactFilter2D();
        filter.useLayerMask = true;
        filter.SetLayerMask(LayerMask.GetMask(damageTarget.ToString()));

        int count = damageCollider.OverlapCollider(filter, overlapped);

        foreach (var o in overlapped)
        {
            Debug.Log("Damage to " + o.name);
            o.gameObject.ApplyDamage(damage);
        }
    }
}
