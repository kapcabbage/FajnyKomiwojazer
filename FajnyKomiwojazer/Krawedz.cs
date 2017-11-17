using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FajnyKomiwojazer
{
    public class Krawedz
    {
        public Wierzcholek Wierzcholek1
        {
            get;
            set;
        }

        public Wierzcholek Wierzcholek2
        {
            get;
            set;
        }

        public double Dlugosc
        {
            get;
            set;
        }

        public Krawedz(Wierzcholek wierzcholek1, Wierzcholek wierzcholek2)
        {
            Wierzcholek1 = wierzcholek1;
            Wierzcholek2 = wierzcholek2;
            Dlugosc = wierzcholek1.Odleglosc(wierzcholek2);
        }
    }
}
