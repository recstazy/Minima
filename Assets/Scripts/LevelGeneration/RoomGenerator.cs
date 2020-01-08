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
            RoomDraft.Initialize();

            foreach (var e in RoomDraft.Exits)
            {
                e.ThisRoom = this;
                e.Initialize();
            }
        }

        public void GenerateRoom()
        {
            DeleteExits();
            GenerateWalls();
        }

        private RoomDraft InstantiateDraft()
        {
            var draft = Instantiate(roomDraftPrefab, this.transform.position, Quaternion.identity, this.transform);
            return draft.GetComponent<RoomDraft>();
        }

        protected virtual void GenerateWalls()
        {
            RoomDraft.WallsGenerator.CreateWalls();
        }

        private void DeleteExits()
        {
            var exitsToDelete = new List<ExitCorner>();

            foreach (var e in RoomDraft.Exits)
            {
                if (e.NextRoom == null)
                {
                    exitsToDelete.Add(e);
                }
            }

            foreach (var e in exitsToDelete)
            {
                RoomDraft.DeleteExit(e);
            }
        }
    }
}
