using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaWeapon : CoolingWeapon
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

    public void SetTarget(DamageTarget target)
    {
        damageTarget = target;
    }

    protected override void Use()
    {
        AreaParticles.Play();
        DamageArea();
    }

    protected virtual List<IDamagable> DamageArea()
    {
        List<Collider2D> overlapped = new List<Collider2D>();
        var filter = new ContactFilter2D();
        filter.useLayerMask = true;
        filter.SetLayerMask(LayerMask.GetMask(damageTarget.ToString()));

        int count = damageCollider.OverlapCollider(filter, overlapped);

        var damaged = new List<IDamagable>();

        foreach (var o in overlapped)
        {
            var iDamagable = o.gameObject.ApplyDamage(damage, Owner);
            
            if (iDamagable != null)
            {
                damaged.Add(iDamagable);
            }
        }

        return damaged;
    }
}
