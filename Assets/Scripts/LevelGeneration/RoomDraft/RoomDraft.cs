using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Minima.Navigation;

namespace Minima.LevelGeneration
{
    public class RoomDraft : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private CornerPoints corners;

        [SerializeField]
        private ExitPoints exits;

        [SerializeField]
        private WallsGenerator wallsGenerator;

        [SerializeField]
        private SpawnGenerator spawnGenerator;

        #endregion

        #region Properties

        public bool IsStartRoom { get; set; }
        public List<WallCorner> Corners { get => corners.Corners; }
        public List<ExitCorner> Exits { get => exits.Exits; }
        public WallsGenerator WallsGenerator { get => wallsGenerator; }
        public SpawnGenerator SpawnGenerator { get => spawnGenerator; }

        #endregion

        public void Initialize(bool isStartRoom = false)
        {
            IsStartRoom = isStartRoom;

            corners.Initialize();
            exits.Initialize();
            WallsGenerator.Initialize(this);
            spawnGenerator.Initialize(this);
        }

        public void HideExits()
        {
            foreach (var e in Exits)
            {
                e.Sprite.enabled = false;
            }
        }

        public void DeleteExit(ExitCorner exit)
        {
            Exits.Remove(exit);

            exit.PreviousCorner.NextCorner = exit.NextCorner;
            Destroy(exit.gameObject);
        }

        public WallCorner GetRandomCorner()
        {
            int n = Random.Range(0, Corners.Count);
            return Corners[n];
        }
    }
}
