using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.LevelGeneration
{
    public class RoomGenerator : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private GameObject roomDraftPrefab;

        [SerializeField]
        [Range(1, 4)]
        private int exitsCount = 2;


        [SerializeField]
        private RoomDraft roomDraft;
        #endregion

        #region Properties

        #endregion

        private void Awake()
        {
            GenerateRoom();
        }

        protected virtual void GenerateRoom()
        {
            roomDraft.Initialize();
            roomDraft.WallsGenerator.CreateWalls();
        }

    }
}
