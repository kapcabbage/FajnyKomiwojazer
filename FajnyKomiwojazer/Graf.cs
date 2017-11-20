using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public double GetDistanceSoFar()
        {
            var distance = 0.0;
            for (int i = 1; i < _wierzcholki.Count; i++)
            {
                distance += Odleglosc(i - 1, i);
            }
            return distance;
        }

        public double GetDistanceSoFarByEdge()
        {
            var distance = 0.0;
            foreach(var edge in _krawedzie)
            {
                distance += edge.Dlugosc;
            }
            return distance;
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

        public double GetValueSoFarByEdge()
        {
            var value = 0.0;
            foreach (var edge in _krawedzie)
            {
                value += edge.Wierzcholek2.Wartosc;
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

        public void SaveToFile(string fullPath)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullPath))
            {
                foreach (Wierzcholek wierzcholek in _wierzcholki)
                {
                    file.WriteLine(String.Format("{0} {1} {2} {3}", wierzcholek.Indeks, wierzcholek.Wartosc, wierzcholek.X, wierzcholek.Y));
                }

                file.WriteLine();

                foreach(Krawedz krawedz in _krawedzie)
                {
                    file.WriteLine(String.Format("{0} {1}", krawedz.Wierzcholek1.Indeks, krawedz.Wierzcholek2.Indeks));
                }
            }
        }

        /// <summary>
        /// Zakładamy, że cykl istnieje i krawędzie są zgodnie z nim posortowana
        /// </summary>
        public void CopyCycleToClipboard()
        {
            if(Krawedzie.Count == 0)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(Krawedzie[0].Wierzcholek1.Indeks);
            foreach (Krawedz e in Krawedzie)
            {
                sb.Append($" -> {e.Wierzcholek2.Indeks}");
            }

            Clipboard.SetText(sb.ToString());
        }
    }
}
