﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FajnyKomiwojazer
{
    public class Graf
    {
        private List<Wierzcholek> _wierzcholki = new List<Wierzcholek>();
        private List<Krawedz> _krawedzie = new List<Krawedz>();


        public List<Wierzcholek> Wierzcholki
        {
            get
            {
                return _wierzcholki;
            }
            set
            {
                _wierzcholki = value;
            }
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

        public Wierzcholek GetFirst()
        {
            return _wierzcholki.First();
        }

        public Wierzcholek GetLast()
        {
            return _wierzcholki.Last();
        }



        public double Odleglosc(int indeks1, int indeks2)
        {
            return _wierzcholki[indeks1].Odleglosc(_wierzcholki[indeks2]);
        }

        public void AddWiercholek(Wierzcholek wiercholek)
        {
            _wierzcholki.Add(wiercholek);
        }

        public double GetDistanceSoFar(int weight)
        {
            var distance = 0.0;
            for (int i = 1; i < _wierzcholki.Count; i++)
            {
                distance += Odleglosc(i - 1, i);
            }
            return distance * weight;
        }

        public double GetValueSoFar()
        {
            var value = 0.0;
            for (int i = 0; i < _wierzcholki.Count; i++)
            {
                value += _wierzcholki[i].Wartosc;
            }

            return value;
        }

        public void AddKrawedz(Krawedz krawedz)
        {
            _krawedzie.Add(krawedz);
        }

        public Wierzcholek GetWiercholek(int indeks)
        {
            return _wierzcholki[indeks];
        }
    }
}
