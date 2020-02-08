using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public struct NavCell
    {
        private enum Half
        {
            Left,
            Right,
            Top,
            Bottom,
        }

        private enum Corner
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
        }

        private enum MiddleType
        {
            AC,
            BD,
        }

        #region Fields

        private NavPoint ab;
        private NavPoint bc;
        private NavPoint cd;
        private NavPoint ad;

        #endregion

        #region Properties

        // Clockwise from left bottom

        // B C
        // A D

        public NavPoint A { get; set; }
        public NavPoint B { get; set; }
        public NavPoint C { get; set; }
        public NavPoint D { get; set; }
        public Vector2 Center { get; set; }

        public NavEdge AB { get; private set; }
        public NavEdge BC { get; private set; }
        public NavEdge CD { get; private set; }
        public NavEdge AD { get; private set; }

        public NavTriangle[] Triangles { get; private set; } 
        public NavEdge[] Edges { get; private set; }
        public NavPoint[] Points { get; private set; }
        public bool IsValid { get; private set; }

        public int Activation { get; private set; }

        #endregion

        public NavCell(NavPoint a, NavPoint b, NavPoint c, NavPoint d)
        {
            A = a;
            B = b;
            C = c;
            D = d;

            AB = new NavEdge();
            BC = new NavEdge();
            CD = new NavEdge();
            AD = new NavEdge();
            Center = A.Position + (C.Position - A.Position) / 2;

            ab = new NavPoint(Vector2.Lerp(A.Position, B.Position, 0.5f));
            bc = new NavPoint(Vector2.Lerp(B.Position, C.Position, 0.5f));
            cd = new NavPoint(Vector2.Lerp(C.Position, D.Position, 0.5f));
            ad = new NavPoint(Vector2.Lerp(A.Position, D.Position, 0.5f));

            Activation = 0;
            IsValid = true;

            Points = new NavPoint[0];
            Edges = new NavEdge[0];
            Triangles = new NavTriangle[0];

            AddPointsUniq(A, B, C, D);

            SetActivation();
            Triangulate();
        }

        public NavTriangle GetNearestTriangle(Vector2 point)
        {
            return Helpers.GetNearestTriangle(point, Triangles);
        }

        #region Triangulation

        private void Triangulate()
        {
            switch (Activation)
            {
                case 0:
                    return;
                case 1:
                    {
                        TriangulateCorner(Corner.BottomLeft);
                        break;
                    }
                case 2:
                    {
                        TriangulateCorner(Corner.BottomRight);
                        break;
                    }
                case 3:
                    {
                        TriangulateHalf(Half.Bottom);
                        break;
                    }
                case 4:
                    {
                        TriangulateCorner(Corner.TopRight);
                        break;
                    }
                case 5:
                    {
                        TriangulateMiddle(MiddleType.AC);
                        break;
                    }
                case 6:
                    {
                        TriangulateHalf(Half.Right);
                        break;
                    }
                case 7:
                    {
                        TriangulateWithoutCorner(Corner.TopLeft);
                        break;
                    }
                case 8:
                    {
                        TriangulateCorner(Corner.TopLeft);
                        break;
                    }
                case 9:
                    {
                        TriangulateHalf(Half.Left);
                        break;
                    }
                case 10:
                    {
                        TriangulateMiddle(MiddleType.BD);
                        break;
                    }
                case 11:
                    {
                        TriangulateWithoutCorner(Corner.TopRight);
                        break;
                    }
                case 12:
                    {
                        TriangulateHalf(Half.Top);
                        break;
                    }
                case 13:
                    {
                        TriangulateWithoutCorner(Corner.BottomRight);
                        break;
                    }
                case 14:
                    {
                        TriangulateWithoutCorner(Corner.BottomLeft);
                        break;
                    }
                case 15:
                    {
                        TriangulateFull();
                        break;
                    }
            }

        }

        private void TriangulateCorner(Corner corner)
        {
            switch (corner)
            {
                case Corner.BottomLeft:
                    {
                        var edge = CreateEdge(ab, ad);
                        CreateTriangle(edge, A);
                        break;
                    }
                case Corner.BottomRight:
                    {
                        var edge = CreateEdge(ad, cd);
                        CreateTriangle(edge, D);
                        break;
                    }
                case Corner.TopLeft:
                    {
                        var edge = CreateEdge(ab, bc);
                        CreateTriangle(edge, B);
                        break;
                    }
                case Corner.TopRight:
                    {
                        var edge = CreateEdge(bc, cd);
                        CreateTriangle(edge, C);
                        break;
                    }
            }

        }

        private void TriangulateHalf(Half half)
        {
            switch (half)
            {
                case Half.Left:
                    {
                        var middle = CreateEdge(ad, B);

                        CreateTriangle(middle, A);
                        CreateTriangle(middle, bc);
                        break;
                    }
                case Half.Right:
                    {
                        var middle = CreateEdge(bc, D);

                        CreateTriangle(middle, C);
                        CreateTriangle(middle, ad);
                        break;
                    }
                case Half.Top:
                    {
                        var middle = CreateEdge(cd, B);

                        CreateTriangle(middle, C);
                        CreateTriangle(middle, ab);

                        break;
                    }
                case Half.Bottom:
                    {
                        var middle = CreateEdge(ab, D);

                        CreateTriangle(middle, A);
                        CreateTriangle(middle, cd);
                        break;
                    }
            }
        }

        private void TriangulateWithoutCorner(Corner corner)
        {
            switch (corner)
            {
                case Corner.TopLeft:
                    {
                        var edge = CreateEdge(ab, bc);
                        var abA = CreateEdge(ab, A);
                        var bcA = CreateEdge(bc, A);
                        CreateTriangle(edge, bcA, abA);

                        var bcD = CreateEdge(bc, D);
                        var adLine = CreateEdge(A, D);
                        CreateTriangle(bcA, adLine, bcD);
                        CreateTriangle(bcD, C);

                        break;
                    }
                case Corner.TopRight:
                    {
                        var edge = CreateEdge(bc, cd);
                        var cdD = CreateEdge(cd, D);
                        var bcD = CreateEdge(bc, D);
                        CreateTriangle(edge, bcD, cdD);

                        var bcA = CreateEdge(bc, A);
                        var adLine = CreateEdge(A, D);
                        CreateTriangle(bcA, adLine, bcD);
                        CreateTriangle(bcA, B);

                        break;
                    }
                case Corner.BottomLeft:
                    {
                        var edge = CreateEdge(ab, ad);
                        var adD = CreateEdge(ad, D);
                        var abD = CreateEdge(ab, D);
                        CreateTriangle(edge, abD, adD);

                        var abC = CreateEdge(ab, C);
                        var cdLine = CreateEdge(C, D);
                        CreateTriangle(abC, cdLine, abD);
                        CreateTriangle(abC, B);

                        break;
                    }
                case Corner.BottomRight:
                    {
                        var edge = CreateEdge(ad, cd);
                        var cdA = CreateEdge(cd, A);
                        var adA = CreateEdge(ad, A);
                        CreateTriangle(edge, adA, cdA);

                        var cdB = CreateEdge(cd, B);
                        var abLine = CreateEdge(A, B);
                        CreateTriangle(cdB, abLine, cdA);
                        CreateTriangle(cdB, C);

                        break;
                    }
            }

        }

        private void TriangulateMiddle(MiddleType middleType)
        {
            switch (middleType)
            {
                case MiddleType.AC:
                    {
                        bool includeMiddle = Helpers.CheckVisibility(A.Position, C.Position);

                        if (includeMiddle)
                        {
                            var ac = CreateEdge(A, C);
                            var adA = CreateEdge(ad, A);
                            var adC = CreateEdge(ad, C);
                            CreateTriangle(ac, adC, adA);
                            CreateTriangle(adC, cd);

                            var abC = CreateEdge(ab, C);
                            var abA = CreateEdge(ab, A);
                            CreateTriangle(ac, abC, abA);
                            CreateTriangle(abC, bc);
                        }
                        else
                        {
                            TriangulateCorner(Corner.TopRight);
                            TriangulateCorner(Corner.BottomLeft);
                        }

                        break;
                    }
                case MiddleType.BD:
                    {
                        bool includeMiddle = Helpers.CheckVisibility(B.Position, D.Position);

                        if (includeMiddle)
                        {
                            var bd = CreateEdge(B, D);
                            var cdD = CreateEdge(cd, D);
                            var cdB = CreateEdge(cd, B);
                            CreateTriangle(bd, cdD, cdB);
                            CreateTriangle(cdB, bc);

                            var adD = CreateEdge(ad, D);
                            var adB = CreateEdge(ad, B);
                            CreateTriangle(bd, adD, adB);
                            CreateTriangle(adB, ab);
                        }
                        else
                        {
                            TriangulateCorner(Corner.BottomRight);
                            TriangulateCorner(Corner.TopLeft);
                        }

                        break;
                    }
            }
        }

        private void TriangulateFull()
        {
            var middle = CreateEdge(B, D);

            CreateTriangle(middle, A);
            CreateTriangle(middle, C);
        }

        #endregion

        private NavEdge CreateEdge(NavPoint a, NavPoint b)
        {
            return new NavEdge(a, b);
        }

        private void CreateTriangle(NavEdge edge, NavPoint point)
        {
            var triangle = new NavTriangle(edge, point);
            Triangles = Triangles.ConcatOne(triangle);

            AddEdgesUniq(edge, triangle.BC, triangle.AC);
            AddPointsUniq(triangle.A, triangle.B, triangle.C);
        }

        private void CreateTriangle(NavEdge ab, NavEdge bc, NavEdge ac)
        {
            var triangle = new NavTriangle(ab, bc, ac);
            Triangles = Triangles.ConcatOne(triangle);

            AddEdgesUniq(ab, bc, ac);
            AddPointsUniq(triangle.A, triangle.B, triangle.C);
        }

        private void AddEdgesUniq(params NavEdge[] edges)
        {
            foreach (var edge in edges)
            {
                Edges = Edges.AddUniq(edge);
            }
        }

        private void AddPointsUniq(params NavPoint[] points)
        {
            foreach (var point in points)
            {
                Points = Points.AddUniq(point);
            }
        }

        private void SetActivation()
        {
            BitArray array = new BitArray(4);

            array[3] = B.Activated;
            array[2] = C.Activated;
            array[1] = D.Activated;
            array[0] = A.Activated;

            Activation = GetIntFromBitArray(array);
        }

        // From StackOverflow
        private int GetIntFromBitArray(BitArray bitArray)
        {
            if (bitArray.Length > 32)
            {
                throw new System.ArgumentException("Argument length should be at most 32 bits.");
            }

            int[] array = new int[1];
            bitArray.CopyTo(array, 0);
            return array[0];
        }
    }
}
