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

        private int ExitsCount
        {
            get => exitsCount;
            set
            {
                exitsCount = Mathf.Clamp(value, 1, 4);
            }
        }

        public RoomDraft RoomDraft { get; private set; }
        bool exitsDeleted = false;

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
            RoomDraft.HideExits();
        }

        public void OnPostSnap()
        {
            RoomDraft.SpawnGenerator.GeneratePoints();
            RoomDraft.NavMeshBuilder.BuildDelayed(1f);
        }

        public void SetExitsCount(int count)
        {
            if (!exitsDeleted)
            {
                ExitsCount = count;

                for (int i = 0; i < 4 - ExitsCount; i++)
                {
                    int n = Random.Range(0, RoomDraft.Exits.Count);
                    var exitToDelete = RoomDraft.Exits[n];
                    RoomDraft.DeleteExit(exitToDelete);
                }

                exitsDeleted = true;
            }
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
                if (e.NextRoom == null || e.NextExit == null)
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
