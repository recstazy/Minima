using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public struct ChunkConnection
    {
        #region Properties

        public bool IsValid { get => Builder != null; }

        public NavMeshBuilder Builder { get; set; }
        public bool TriedConnect { get; set; }
        public Vector2 Direction { get; set; }
        public bool IsConnected { get; set; }

        #endregion

        public ChunkConnection(Vector2 direction)
        {
            Direction = direction;
            Builder = null;
            TriedConnect = false;
            IsConnected = false;
        }

        public void SetBuilder(NavMeshBuilder builder)
        {
            Builder = builder;
        }

        public void SetTried(bool tried)
        {
            TriedConnect = tried;
        }

        public void SetConnected(bool isConnected)
        {
            IsConnected = isConnected;
        }
    }
}
