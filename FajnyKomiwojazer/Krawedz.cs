using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FajnyKomiwojazer
{
    public class Krawedz : IEquatable<Krawedz>
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
            Wierzcholek2.KrawedzOd = this;
            Wierzcholek1.KrawedzDo = this;
            Wierzcholek tmp = Wierzcholek1;
            Wierzcholek1 = Wierzcholek2;
            Wierzcholek2 = tmp;
        }

        public Krawedz Nastepna()
        {
            return Wierzcholek2.KrawedzOd;
        }

        public Krawedz Poprzednia()
        {
            return Wierzcholek1.KrawedzDo;
        }

        public override string ToString()
        {
            return String.Format($"E({Wierzcholek1.Indeks}, {Wierzcholek2.Indeks})");
        }


        public override bool Equals(object obj)
        {
            if (!(obj is Krawedz))
                return false;

            return Equals((Krawedz)obj);
        }

        public bool Equals(Krawedz other)
        {
            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }

            return this.Wierzcholek1.Indeks == other.Wierzcholek1.Indeks && this.Wierzcholek2.Indeks == other.Wierzcholek2.Indeks;
        }
    }
}
