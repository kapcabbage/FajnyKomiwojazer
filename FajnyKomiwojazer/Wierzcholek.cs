using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FajnyKomiwojazer
{
    public class Wierzcholek
    {
        private List<Krawedz> _krawedzie = new List<Krawedz>();

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
        

        public override string ToString()
        {
            return String.Format($"V{Indeks}");
        }

        public List<Krawedz> Krawedzie
        {
            get
            {
                return _krawedzie;
            }
            set
            {
                _krawedzie = value;
            }
        }
    }
}
