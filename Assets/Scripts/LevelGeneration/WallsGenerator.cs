using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsGenerator : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private GameObject wallPrefab;

    #endregion

    public void CreateWalls()
    {
        InstantiateWall(new Vector2(3f, 3f));
    }

    protected virtual GameObject InstantiateWall(Vector2 position)
    {
        var wall = Instantiate(wallPrefab, position, Quaternion.identity, transform);
        return wall;
    }
}
