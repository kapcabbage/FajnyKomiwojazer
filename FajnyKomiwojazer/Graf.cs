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
        public static Graf Decodify(Graf baseline, List<int> representation)
        {
            if(representation == null || representation.Count == 0)
            {
                return baseline;
            }
            for(int i = 0; i < representation.Count; i++)
            {
                if (i > 0)
                {
                    baseline.AddKrawedz(new Krawedz(baseline.Wierzcholki[representation[i - 1]], baseline.Wierzcholki[representation[i]]));
                }
            }
            baseline.AddKrawedz(new Krawedz(baseline.Wierzcholki[representation.Last()], baseline.Wierzcholki[representation.First()]));
            return baseline;
        }

        private List<Wierzcholek> _wierzcholki = new List<Wierzcholek>();
        private List<Krawedz> _krawedzie = new List<Krawedz>();

        public double Wynik
        {
            get;
            set;
        }

        public List<int> Codify()
        {
            List <int> coded = new List<int>();
            if(_krawedzie.Count == 0)
            {
                return coded;
            }
            Krawedz start = _krawedzie[0];
            Krawedz next = start.Nastepna();
            while (next != start)
            {
                coded.Add(next.Wierzcholek2.Indeks);
                next = next.Nastepna();
            }
            coded.Add(start.Wierzcholek2.Indeks);
            return coded;
        }

        public List<int> Representation
        {
            get;
            set;
        }

        public List<Krawedz> GetKrawedzie()
        {
            return _krawedzie.ToList();
        }

        public List<Wierzcholek> GetWierzcholki()
        {
            return _wierzcholki.ToList();
        }

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


        public double GetScore()
        {
            double score = 0.0d;
            foreach (var edge in _krawedzie)
            {
                score -= edge.Dlugosc;
                score += edge.Wierzcholek1.Wartosc;
            }
            return score;
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
            krawedz.Wierzcholek1.KrawedzOd = krawedz;
            krawedz.Wierzcholek2.KrawedzDo = krawedz;
        }

        public void RemoveKrawedz(Krawedz krawedz)
        {
            _krawedzie.Remove(krawedz);
            krawedz.Wierzcholek1.KrawedzOd = null;
            krawedz.Wierzcholek2.KrawedzDo = null;
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
            if(_krawedzie.Count == 0)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(_krawedzie[0].Wierzcholek1.Indeks);
            foreach (Krawedz e in _krawedzie)
            {
                sb.Append($" -> {e.Wierzcholek2.Indeks}");
            }

            Clipboard.SetText(sb.ToString());
        }

        
        public Graf Clone()
        {
            Graf graf = new Graf();
            graf.Wierzcholki = Wierzcholki.Select(w => w.Clone()).ToList();
            graf.Krawedzie = Krawedzie.Select(k => 
            {
                Krawedz newK = new Krawedz(graf.Wierzcholki[k.Wierzcholek1.Indeks], graf.Wierzcholki[k.Wierzcholek2.Indeks]);
                newK.Wierzcholek1.KrawedzOd = newK;
                newK.Wierzcholek2.KrawedzDo = newK;
                return newK;
            }).ToList();
            return graf;
        }
    }
}
