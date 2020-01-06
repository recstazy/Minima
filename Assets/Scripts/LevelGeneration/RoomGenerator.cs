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

        private RoomDraft roomDraft;

        #endregion

        #region Properties

        #endregion

        public void GenerateRoom()
        {
            roomDraft = InstantiateDraft();
            Generate();
        }

        private RoomDraft InstantiateDraft()
        {
            var draft = Instantiate(roomDraftPrefab, Vector3.zero, Quaternion.identity, this.transform);
            return draft.GetComponent<RoomDraft>();
        }

        protected virtual void Generate()
        {
            roomDraft.Initialize();
            roomDraft.WallsGenerator.CreateWalls();
        }

    }
}
