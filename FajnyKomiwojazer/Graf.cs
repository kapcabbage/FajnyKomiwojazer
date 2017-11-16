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

        public double Odleglosc(int indeks1, int indeks2)
        {
            return _wierzcholki[indeks1].Odleglosc(_wierzcholki[indeks2]);
        }

        public void AddWiercholek(Wierzcholek wiercholek)
        {
            _wierzcholki.Add(wiercholek);
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
