using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FajnyKomiwojazer
{
    public class Graf
    {
        private List<Wierzcholek> _wierzcholki = new List<Wierzcholek>();


        public List<Wierzcholek> Wierzcholki
        {
            get
            {
                return _wierzcholki.ToList();
            }
            set
            {
                _wierzcholki = value;
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
            return _wierzcholki[indeks1].Distance(_wierzcholki[indeks2]);
        }

        public void AddWiercholek(Wierzcholek wiercholek)
        {
            _wierzcholki.Add(wiercholek);
        }

        public double GetDistanceSoFar(int weight)
        {
            var distance = 0.0;
            for(int i =1; i<_wierzcholki.Count;i++)
            {
                distance += Odleglosc(i - 1, i);
            }
            return distance*weight;
        }

        public double GetValueSoFar()
        {
            var value = 0.0;
            for (int i = 0; i < _wierzcholki.Count; i++)
            {
                value += _wierzcholki[i].Value;
            }

            return value;
        }
    }
}
