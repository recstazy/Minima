using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.LevelGeneration
{
    public class WallsGeneratorOrtho : WallsGeneratorWithExits
    {
        #region Fields

        [SerializeField]
        private CornerPoints corners;

        #endregion

        #region Properties

        #endregion

        protected override void CreateWallBetweenCorners(WallCorner cornerA, WallCorner cornerB)
        {
            Vector2 orthoPosition = GetOrthoPosition(cornerA, cornerB);
            var orthoCorner = corners.CreateNewCorner(orthoPosition);
            cornerA.NextCorner = orthoCorner;
            orthoCorner.NextCorner = cornerB;
            orthoCorner.name = cornerA.name + " - " + cornerB.name;

            cornerA.BindPrevious();
            orthoCorner.BindPrevious();

            base.CreateWallBetweenCorners(cornerA, cornerA.NextCorner);
            base.CreateWallBetweenCorners(orthoCorner, orthoCorner.NextCorner);
        }

        private Vector2 GetOrthoPosition(WallCorner cornerA, WallCorner cornerB)
        {
            Vector2 center = transform.position;

            var endA = cornerA.GetWallEndPoint(cornerB);
            var endB = cornerB.GetWallEndPoint(cornerA);

            Vector2 positionVariantA = new Vector2(endA.position.x, endB.position.y);
            Vector2 positionVariantB = new Vector2(endB.position.x, endA.position.y);
            Vector2 result = positionVariantA;

            //if ((positionVariantA - center).sqrMagnitude < (positionVariantB - center).sqrMagnitude)
            //{
            //    result = positionVariantB;
            //}

            return result;
        }
    }
}
