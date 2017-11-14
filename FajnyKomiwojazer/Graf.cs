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

        public double Odleglosc(int indeks1, int indeks2)
        {
            if(indeks1 == indeks2)
            {
                return 0.0;
            }


            double vx = _wierzcholki[indeks1].X - _wierzcholki[indeks2].X;
            double vy = _wierzcholki[indeks1].Y - _wierzcholki[indeks2].Y;

            return (Math.Sqrt(vx*vx + vy*vy));
        }

        public void AddWiercholek(Wierzcholek wiercholek)
        {
            _wierzcholki.Add(wiercholek);
        }
    }
}
