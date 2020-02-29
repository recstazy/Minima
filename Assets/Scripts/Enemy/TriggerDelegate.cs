using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class TriggerDelegate : MonoBehaviour, IEnemyTargetable
{
    public delegate void TargetTriggeredHandler(GameObject target);
    public event TargetTriggeredHandler OnTargetTriggered;

    #region Fields

    protected TargetType[] targetTypes;
    protected int[] targetLayers = new int[0];

    #endregion

    #region Properties

    #endregion

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var layer = collision.gameObject.layer;

        if (layer > 0)
        {
            if (targetLayers.Contains(layer))
            {
                CallTargetTriggered(collision.gameObject);
            }
        }
    }

    protected void CallTargetTriggered(GameObject target)
    {
        OnTargetTriggered?.Invoke(target);
    }

    public void UpdateTargets(List<TargetType> targets)
    {
        targetTypes = targets.ToArray();
        SetTargetLayers();
    }

    protected void SetTargetLayers()
    {
        targetLayers = targetTypes.Select(t => (int)t).ToArray();
    }
}
