﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        #endregion

        #region Properties

        public List<WallCorner> Corners { get => corners.Corners; }
        public List<ExitCorner> Exits { get => exits.Exits; }
        public WallsGenerator WallsGenerator { get => wallsGenerator; }

        #endregion

        public void Initialize()
        {
            corners.Initialize();
            exits.Initialize();
            WallsGenerator.Initialize(this);
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
    }
}
