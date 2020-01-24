using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public class NavCell
    {
        #region Fields

        #endregion

        #region Properties

        // Clockwise
        public NavPoint A { get; set; }
        public NavPoint B { get; set; }
        public NavPoint C { get; set; }
        public NavPoint D { get; set; }

        public NavEdge[] Edges { get; set; } = new NavEdge[2];

        int cellActivation = 0;

        #endregion

        public NavCell(NavPoint a, NavPoint b, NavPoint c, NavPoint d)
        {
            A = a;
            B = b;
            C = c;
            D = d;

            CreateEdge();
        }

        public void CreateEdge()
        {
            BitArray array = new BitArray(4);
            array[3] = A.Activated;
            array[2] = B.Activated;
            array[1] = C.Activated;
            array[0] = D.Activated;

            cellActivation = GetIntFromBitArray(array);

            switch (cellActivation)
            {
                case 0:
                    return;
                case 1:
                    Edges[0] = CreateEdge(C, D, D, A);
                    break;
                case 2:
                    Edges[0] = CreateEdge(B, C, C, D);
                    break;
                case 3:
                    Edges[0] = CreateEdge(B, C, D, A);
                    break;
                case 4:
                    Edges[0] = CreateEdge(A, B, B, C);
                    break;
                case 5:
                    {
                        bool includeCenter = !StaticHelpers.CheckVisibility(B.Position, D.Position);
                        if (includeCenter)
                        {
                            Edges[0] = CreateEdge(A, B, B, C);
                            Edges[1] = CreateEdge(C, D, D, A);
                        }
                        else
                        {
                            Edges[0] = CreateEdge(A, B, D, A);
                            Edges[1] = CreateEdge(B, C, C, D);
                        }
                        break;
                    }
                case 6:
                    Edges[0] = CreateEdge(A, B, C, D);
                    break;
                case 7:
                    Edges[0] = CreateEdge(A, B, D, A);
                    break;
                case 8:
                    Edges[0] = CreateEdge(D, A, A, B);
                    break;
                case 9:
                    Edges[0] = CreateEdge(C, D, A, B);
                    break;
                case 10:
                    {
                        bool includeCenter = !StaticHelpers.CheckVisibility(B.Position, D.Position);
                        if (includeCenter)
                        {
                            Edges[0] = CreateEdge(A, B, D, A);
                            Edges[1] = CreateEdge(B, C, C, D);
                        }
                        else
                        {
                            Edges[0] = CreateEdge(A, B, B, C);
                            Edges[1] = CreateEdge(C, D, D, A);
                        }
                        break;
                    }
                case 11:
                    Edges[0] = CreateEdge(B, C, A, B);
                    break;
                case 12:
                    Edges[0] = CreateEdge(D, A, B, C);
                    break;
                case 13:
                    Edges[0] = CreateEdge(C, D, B, C);
                    break;
                case 14:
                    Edges[0] = CreateEdge(D, A, C, D);
                    break;
                case 15:
                    return;
                default:
                    return;
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
