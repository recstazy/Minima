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

        #endregion

        #region Properties

        public int ExitsCount
        {
            get => exitsCount;
            set
            {
                exitsCount = Mathf.Clamp(value, 1, 4);
            }
        }

        public RoomDraft RoomDraft { get; private set; }

        #endregion

        public void Initialize()
        {
            RoomDraft = InstantiateDraft();
            foreach (var e in RoomDraft.Exits)
            {
                e.ThisRoom = this;
                e.Initialize();
            }

            GenerateRoom();
        }

        public void GenerateRoom()
        {
            //DeleteExits();
            //Generate();
        }

        private RoomDraft InstantiateDraft()
        {
            var draft = Instantiate(roomDraftPrefab, this.transform.position, Quaternion.identity, this.transform);
            return draft.GetComponent<RoomDraft>();
        }

        protected virtual void Generate()
        {
            RoomDraft.Initialize();
            RoomDraft.WallsGenerator.CreateWalls();
        }

        private void DeleteExits()
        {
            for (int i = 4 - exitsCount; i > 0; i--)
            {
                var number = Random.Range(0, RoomDraft.Exits.Count);
                RoomDraft.DeleteExit(number);
            }
        }
    }
}
