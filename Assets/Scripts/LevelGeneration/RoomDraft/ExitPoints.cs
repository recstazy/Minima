using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Minima.LevelGeneration
{
    public class ExitPoints : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private List<ExitCorner> exits = new List<ExitCorner>();

        [SerializeField]
        [Range(0, 100)]
        private int exitCloseChance = 50;

        #endregion

        #region Properties

        public List<ExitCorner> Exits { get => exits; }

        #endregion

        public void Initialize()
        {
            foreach (var e in exits)
            {
                e.BindPrevious();
            }
        }

        public void ExitsDeleted()
        {
            DeleteNulls();
        }

        private void DeleteNulls()
        {
            exits.RemoveAll(e => e == null);
        }

        public void CloseExitsRandomly()
        {
            var openedExit = Exits.Random();
            openedExit.SetIsClosed(false);

            if (Exits.Count > 1)
            {
                var copy = Exits.ToList();
                copy.Remove(openedExit);

                foreach (var exit in copy)
                {
                    exit.SetIsClosed(Helpers.FakeRandomBool(exitCloseChance));
                }
            }
        }
    }
}
