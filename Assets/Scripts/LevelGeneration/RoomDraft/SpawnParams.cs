using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnParams
{
    #region Fields

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private int count;

    #endregion

    #region Properties

    public GameObject Prefab { get => prefab; }
    public int Count { get => count; }

    #endregion

}
