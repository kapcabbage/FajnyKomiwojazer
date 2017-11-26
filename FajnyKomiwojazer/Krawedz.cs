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

        public void Obroc()
        {
            Wierzcholek tmp = Wierzcholek1;
            Wierzcholek1 = Wierzcholek2;
            Wierzcholek2 = tmp;
        }

        public Krawedz Nastepna()
        {
            return Wierzcholek2.Krawedzie.FirstOrDefault(k => k != this);
        }
        
        public override string ToString()
        {
            return String.Format($"E({Wierzcholek1.Indeks}, {Wierzcholek2.Indeks})");
        }
    }
}
