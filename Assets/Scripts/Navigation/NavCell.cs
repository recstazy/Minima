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

        public NavEdge Edge { get; set; }

        int number = 0;

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

            int integer = GetIntFromBitArray(array);

            switch (integer)
            {
                case 0:
                    return;
                case 1:
                    Edge = CreateEdge(C, D, D, A);
                    break;
                case 2:
                    Edge = CreateEdge(B, C, C, D);
                    break;
                case 3:
                    Edge = CreateEdge(B, C, D, A);
                    break;
                case 4:
                    Edge = CreateEdge(A, B, B, C);
                    break;
                case 5:
                    //Edge = CreateEdge();
                    break;
                case 6:
                    Edge = CreateEdge(A, B, C, D);
                    break;
                case 7:
                    Edge = CreateEdge(A, B, D, A);
                    break;
                case 8:
                    Edge = CreateEdge(D, A, A, B);
                    break;
                case 9:
                    Edge = CreateEdge(C, D, A, B);
                    break;
                case 10:
                    //Edge = CreateEdge();
                    break;
                case 11:
                    Edge = CreateEdge(B, C, A, B);
                    break;
                case 12:
                    Edge = CreateEdge(D, A, B, C);
                    break;
                case 13:
                    Edge = CreateEdge(C, D, B, C);
                    break;
                case 14:
                    Edge = CreateEdge(D, A, C, D);
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
