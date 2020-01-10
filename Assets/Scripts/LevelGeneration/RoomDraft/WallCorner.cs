using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.LevelGeneration
{
    public class WallCorner : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private WallCorner nextCorner;

        private Transform thisTransform;

        #endregion

        #region Properties

        public Vector3 position { get => ThisTransform.position; set => ThisTransform.position = value; }
        public Vector2 Vector2Position { get => position.ToVector2(); set => position = value.ToVector3(); }

        public Vector3 localPosition { get => ThisTransform.localPosition; set => ThisTransform.localPosition = value; }
        public Quaternion rotation { get => ThisTransform.rotation; set => ThisTransform.rotation = value; }
        public Vector3 localScale { get => ThisTransform.localScale; set => ThisTransform.localScale = value; }

        public bool IsExit 
        {
            get
            {
                return this is ExitCorner;
            }
        }

        public WallCorner NextCorner { get => nextCorner; set => nextCorner = value; }
        public WallCorner PreviousCorner { get; set; }

        public Transform ThisTransform
        {
            get
            {
                if (thisTransform == null)
                {
                    thisTransform = transform;
                }

                return thisTransform;
            }
        }

        #endregion

        public virtual void BindPrevious()
        {
            NextCorner.PreviousCorner = this;
        }

        public virtual WallCorner GetWallEndPoint(WallCorner fromPoint)
        {
            return this;
        }
    }
}
