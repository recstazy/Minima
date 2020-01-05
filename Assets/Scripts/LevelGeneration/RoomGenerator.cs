using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private WallsGenerator wallsGenerator;

    [SerializeField]
    private GameObject floorPrefab;

    [SerializeField]
    private GameObject exitPrefab;

    [SerializeField]
    private Transform wallsParent;

    [SerializeField]
    [Range(2, 4)]
    private int exitsCount = 2;

    #endregion

    #region Properties

    #endregion

    private void Awake()
    {
        GenerateRoom();
    }

    protected virtual void GenerateRoom()
    {
        CreateFloor();
        wallsGenerator.CreateWalls();
    }

    private void CreateFloor()
    {
        var floor = Instantiate(floorPrefab, floorPrefab.transform.position, floorPrefab.transform.rotation, transform);
    }
}
