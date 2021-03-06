﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FajnyKomiwojazer
{
    public class Wierzcholek : IEquatable<Wierzcholek>
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

            return 6 * Math.Sqrt(vx * vx + vy * vy);
        }
        

        public override string ToString()
        {
            return String.Format($"V{Indeks}");
        }

        public Krawedz KrawedzDo
        {
            get;
            set;
        }
        public Krawedz KrawedzOd
        {
            get;
            set;
        }


        public Wierzcholek Clone()
        {
            Wierzcholek wierzcholek = new Wierzcholek();
            wierzcholek.Indeks = Indeks;
            wierzcholek.X = X;
            wierzcholek.Y = Y;
            wierzcholek.Wartosc = Wartosc;
            return wierzcholek;
        }


        public override bool Equals(object obj)
        {
            if (!(obj is Krawedz))
                return false;

            return Equals((Krawedz)obj);
        }

        public bool Equals(Wierzcholek other)
        {
            if (Object.ReferenceEquals(this, other))
                return true;

            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }

            return this.Indeks == other.Indeks;
        }
    }
}
