using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minima.Navigation
{
    public class NavCell
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

        #region Fields

        #endregion

        #region Properties

        // Clockwise from left bottom
        public NavPoint A { get; set; }
        public NavPoint B { get; set; }
        public NavPoint C { get; set; }
        public NavPoint D { get; set; }

        public List<NavTriangle> Triangles { get; private set; } = new List<NavTriangle>();

        public List<NavEdge> Edges { get; private set; } = new List<NavEdge>();

        int activation = 0;

        #endregion

        public NavCell(NavPoint a, NavPoint b, NavPoint c, NavPoint d)
        {
            A = a;
            B = b;
            C = c;
            D = d;

            SetActivation();
            Triangulate();
        }

        private void Triangulate()
        {
            switch (activation)
            {
                case 3:
                    {
                        TriangulateHalf(Half.Bottom);
                        break;
                    }
                case 6:
                    {
                        TriangulateHalf(Half.Right);
                        break;
                    }
                case 9:
                    {
                        TriangulateHalf(Half.Left);
                        break;
                    }
                case 12:
                    {
                        TriangulateHalf(Half.Top);
                        break;
                    }
                case 15:
                    {
                        TriangulateFull();
                        break;
                    }
            }

        }
       

        /// <summary>
        /// Creates edge between middles of ab and cd
        /// </summary>
        private NavEdge CreateEdge(NavPoint a, NavPoint b, NavPoint c, NavPoint d)
        {
            var ab = new NavPoint(Vector2.Lerp(a.Position, b.Position, 0.5f));
            var cd = new NavPoint(Vector2.Lerp(c.Position, d.Position, 0.5f));
            
            return new NavEdge(ab, cd);
        }

        private NavEdge CreateEdge(NavPoint a, NavPoint b)
        {
            return new NavEdge(a, b);
        }

        private void TriangulateFull()
        {
            var middle = CreateEdge(B, D);

            CreateTriangle(middle, A);
            CreateTriangle(middle, C);
        }

        private void TriangulateHalf(Half half)
        {
            switch (half)
            {
                case Half.Left:
                    {
                        var bc = new NavPoint(Vector2.Lerp(B.Position, C.Position, 0.5f));
                        var ad = new NavPoint(Vector2.Lerp(A.Position, D.Position, 0.5f));
                        
                        var middle = CreateEdge(ad, B);

                        CreateTriangle(middle, A);
                        CreateTriangle(middle, bc);
                        break;
                    }
                case Half.Right:
                    {
                        var bc = new NavPoint(Vector2.Lerp(B.Position, C.Position, 0.5f)); // B C
                        var ad = new NavPoint(Vector2.Lerp(A.Position, D.Position, 0.5f)); // A D

                        var middle = CreateEdge(bc, D);

                        CreateTriangle(middle, C);
                        CreateTriangle(middle, ad);
                        break;
                    }
                case Half.Top:
                    {
                        var ab = new NavPoint(Vector2.Lerp(A.Position, B.Position, 0.5f));
                        var cd = new NavPoint(Vector2.Lerp(C.Position, D.Position, 0.5f));
                        var middle = CreateEdge(cd, B);

                        CreateTriangle(middle, C);
                        CreateTriangle(middle, ab);

                        Debug.Log("TOP: " + A.Position + " - " + B.Position + " - " + C.Position + " - " + D.Position);

                        break;
                    }
                case Half.Bottom:
                    {
                        var ab = new NavPoint(Vector2.Lerp(A.Position, B.Position, 0.5f));
                        var cd = new NavPoint(Vector2.Lerp(C.Position, D.Position, 0.5f));
                        var middle = CreateEdge(ab, D);

                        CreateTriangle(middle, A);
                        CreateTriangle(middle, cd);
                        break;
                    }
            }
        }

        private void CreateTriangle(NavPoint a, NavPoint b, NavPoint c)
        {
            Triangles.Add(new NavTriangle(a, b, c));
        }

        private void CreateTriangle(NavEdge edge, NavPoint point)
        {
            var triangle = new NavTriangle(edge, point);
            Triangles.Add(triangle);

            if (!Edges.Contains(edge))
            {
                Edges.Add(edge);
            }

            Edges.Add(triangle.BC);
            Edges.Add(triangle.AC);
        }

        private void SetActivation()
        {
            BitArray array = new BitArray(4);
            
            array[3] = B.Activated;
            array[2] = C.Activated;
            array[1] = D.Activated;
            array[0] = A.Activated;

            activation = GetIntFromBitArray(array);
        }

        private int GetIntFromBitArray(BitArray bitArray)
        {
            if (bitArray.Length > 32)
            {
                throw new System.ArgumentException("Argument length shall be at most 32 bits.");
            }

            int[] array = new int[1];
            bitArray.CopyTo(array, 0);
            return array[0];
        }
    }
}
