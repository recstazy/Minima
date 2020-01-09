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
        private float snapAreaRadius;

        [SerializeField]
        private float exitWidth;

        [SerializeField]
        private List<WallCorner> wallEndPoints = new List<WallCorner>();

        [SerializeField]
        private bool randomizeWidth = true;

        [SerializeField]
        [Range(1f, 20f)]
        private float minWidth = 3f;

        [SerializeField]
        [Range(1f, 20f)]
        private float maxWidth = 3f;

        #endregion

        #region Properties

        public RoomGenerator ThisRoom { get; set; }
        public RoomGenerator NextRoom { get; set; }
        public List<WallCorner> ConnectPoints { get; }

        public ExitCorner NextExit { get; set; }

        public SpriteRenderer Sprite { get; private set; }

        #endregion

        private void Awake()
        {
            RandomizeExitWidth();
            Sprite = GetComponent<SpriteRenderer>();
        }

        public void Initialize()
        {
            BindNearestExit();
        }

        public override WallCorner GetWallEndPoint(WallCorner fromPoint)
        {
            var points = new Dictionary<float, WallCorner>();

            foreach (var e in wallEndPoints)
            {
                var distance = Vector2.Distance(e.position, fromPoint.position);
                if (!points.ContainsKey(distance))
                {
                    points.Add(distance, e);
                }
            }

            var nearest = points.Keys.Min();
            return points[nearest];
        }

        private void RandomizeExitWidth()
        {
            if (randomizeWidth)
            {
                var width = Random.Range(minWidth, maxWidth);
                Debug.Log("random = " + width + " : " + minWidth + " ... " + maxWidth);
                SetExitWidth(width);
            }
        }

        public void SetExitWidth(float newWidth)
        {
            exitWidth = newWidth;
            UpdateWidth();
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
                    var foundExit = found.GetComponent<ExitCorner>();
                    if (foundExit.NextRoom == null)
                    {
                        foundExit.NextRoom = ThisRoom;
                        NextRoom = foundExit.ThisRoom;

                        foundExit.NextExit = this;
                        NextExit = foundExit;

                        foundExit.SetExitWidth(exitWidth);
                    }
                }
            }
        }

        private void UpdateWidth()
        {
            localScale = new Vector3(exitWidth, 1f, 1f);
            snapArea.radius = snapAreaRadius / localScale.x;
        }

        #region EditorFunctions

        private void OnValidate()
        {
            UpdateWidth();   
        }

        #endregion
    }
}
