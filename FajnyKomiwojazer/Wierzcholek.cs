using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FajnyKomiwojazer
{
    public class Wierzcholek
    {
        public int Index { get; set; }

        public Double X
        {
            get;
            set;
        }

        public Double Y
        {
            get;
            set;
        }

        public Double Wartosc
        {
            get;
            set;
        }

        public int Indeks
        {
            get;
            set;
        }

        public double Odleglosc(Wierzcholek wierzcholek)
        {
            if (this == wierzcholek)
            {
                return 0.0;
            }


            double vx = wierzcholek.X - X;
            double vy = wierzcholek.Y - Y;

            return 5 * Math.Sqrt(vx * vx + vy * vy);
        }
    }
}
