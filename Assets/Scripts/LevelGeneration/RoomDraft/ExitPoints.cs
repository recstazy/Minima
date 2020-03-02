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
    }
}
