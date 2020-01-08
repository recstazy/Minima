using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AgressionTrigger : MonoBehaviour, IEnemyTargetable
{
    public delegate void TargetTriggeredHandler(GameObject target);
    public event TargetTriggeredHandler OnTargetTriggered;

    #region Fields

    private List<DamageTarget> targetTypes = new List<DamageTarget>();

    #endregion

    #region Properties

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var layer = collision.gameObject.layer;

        if (GetTargetLayers().Contains(layer))
        {
            OnTargetTriggered?.Invoke(collision.gameObject);
        }
    }

    public void AddToTargets(DamageTarget target)
    {
        if (tag != "")
        {
            targetTypes.Add(target);
        }
    }

    public void UpdateTargets(List<DamageTarget> targets)
    {
        targetTypes = targets;
    }

    private List<int> GetTargetLayers()
    {
        List<int> targetLayers = new List<int>();

        foreach (var t in targetTypes)
        {
            targetLayers.Add((int)t);
        }

        return targetLayers;
    }
}
