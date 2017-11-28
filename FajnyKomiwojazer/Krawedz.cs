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

        public static bool operator ==(Krawedz k1, Krawedz k2)
        {
            if (Object.ReferenceEquals(k1, null))
            {
                if (Object.ReferenceEquals(k2, null))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return k1.Equals(k2);
        }

        public static bool operator !=(Krawedz k1, Krawedz k2)
        {
            return !(k1 == k2);
        }
    }
}
