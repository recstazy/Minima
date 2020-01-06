using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.LevelGeneration
{
    public class ExitCorner : WallCorner
    {
        #region Fields

        [SerializeField]
        private CircleCollider2D snapArea;

        [SerializeField]
        private List<WallCorner> connectPoints;

        #endregion

        #region Properties

        public RoomGenerator ThisRoom { get; set; }
        public RoomGenerator NextRoom { get; set; }
        public List<WallCorner> ConnectPoints { get; }

        #endregion

        public void Initialize()
        {
            BindNearestExit();
        }

        private void BindNearestExit()
        {
            var filter = new ContactFilter2D();
            filter.useLayerMask = true;
            filter.layerMask = LayerMask.GetMask("Exits");
            filter.useTriggers = true;

            List<Collider2D> foundExits = new List<Collider2D>();
            int foundNum = snapArea.OverlapCollider(filter, foundExits);

            if (foundNum > 0)
            {
                var found = foundExits.FirstOrDefault(e => e.gameObject != this.gameObject);

                if (found != null)
                {
                    var foundCorner = found.GetComponent<ExitCorner>();
                    if (foundCorner.NextRoom == null)
                    {
                        foundCorner.NextRoom = ThisRoom;
                        NextRoom = foundCorner.ThisRoom;
                    }
                }
            }
        }
    }
}
