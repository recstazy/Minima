using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AgressionTrigger : MonoBehaviour, IEnemyTargetable
{
    public delegate void TargetTriggeredHandler(GameObject target);
    public event TargetTriggeredHandler OnTargetTriggered;

    #region Fields

    private List<string> targetTags = new List<string>();

    #endregion

    #region Properties

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetTags.Contains(collision.tag))
        {
            OnTargetTriggered?.Invoke(collision.gameObject);
        }
    }

    public void AddToTargets(string tag)
    {
        if (tag != "")
        {
            targetTags.Add(tag);
        }
    }

    public void UpdateTargets(List<string> targets)
    {
        targetTags = targets;
    }
}
