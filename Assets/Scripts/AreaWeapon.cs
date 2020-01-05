using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaWeapon : WeaponComponent
{
    #region Fields

    [SerializeField]
    private ParticleSystem AreaParticles;

    [SerializeField]
    private CircleCollider2D damageCollider;

    #endregion

    #region Properties
    
    #endregion

    void Start()
    {
        
    }

    void Update()
    {
        
    }

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
        filter.SetLayerMask(LayerMask.GetMask("Enemy"));

        int count = damageCollider.OverlapCollider(filter, overlapped);

        foreach (var o in overlapped)
        {
            Destroy(o.gameObject);
        }
    }
}
