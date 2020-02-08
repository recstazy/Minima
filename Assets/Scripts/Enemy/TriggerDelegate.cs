using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerDelegate : MonoBehaviour, IEnemyTargetable
{
    public delegate void TargetTriggeredHandler(GameObject target);
    public event TargetTriggeredHandler OnTargetTriggered;

    #region Fields

    protected List<TargetType> targetTypes = new List<TargetType>();

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

        if (GetTargetLayers().Contains(layer))
        {
            CallTargetTriggered(collision.gameObject);
        }
    }

    protected void CallTargetTriggered(GameObject target)
    {
        OnTargetTriggered?.Invoke(target);
    }

    public void UpdateTargets(List<TargetType> targets)
    {
        targetTypes = targets;
    }

    protected List<int> GetTargetLayers()
    {
        List<int> targetLayers = new List<int>();

        foreach (var t in targetTypes)
        {
            targetLayers.Add((int)t);
        }

        return targetLayers;
    }
}
