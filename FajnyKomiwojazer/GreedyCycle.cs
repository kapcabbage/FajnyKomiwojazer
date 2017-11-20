using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FajnyKomiwojazer
{
    public class GreedyCycle
    {


        private Graf _solution;
        private List<Wierzcholek> _notUsed;
        private Graf _instance;
        private double _profit;

        public GreedyCycle(Graf graf)
        {
            _instance = graf;
            Clean();
        }

        private void Clean()
        {
            _solution = new Graf();
            _solution.Wierzcholki = _instance.Wierzcholki.ToList();
            _notUsed = _instance.Wierzcholki.ToList();
        }

        public Graf Solve(int start)
        {
            Clean();
            Wierzcholek wierzcholek = _solution.GetWiercholek(start);
            _solution.Krawedzie.Add(new Krawedz(wierzcholek, wierzcholek));
            _notUsed.Remove(wierzcholek);
            while (Step())
            {

            }
            SortSolution();
            //WypiszRozwiazanie();
            return _solution;
        }


        private void AddToSolution(Wierzcholek wierzcholek, Krawedz krawedz)
        {
            _solution.Krawedzie.Remove(krawedz);
            _solution.Krawedzie.Add(new Krawedz(krawedz.Wierzcholek1, wierzcholek));
            _solution.Krawedzie.Add(new Krawedz(wierzcholek, krawedz.Wierzcholek2));
            _notUsed.Remove(wierzcholek);
        }


        private Krawedz BestGainEdge(Wierzcholek wierzcholek, out double zysk)
        {
            Krawedz najlepsza = null;
            zysk = 0;
            foreach (Krawedz krawedz in _solution.Krawedzie)
            {
                double tymZysk = krawedz.Dlugosc - wierzcholek.Odleglosc(krawedz.Wierzcholek1) - wierzcholek.Odleglosc(krawedz.Wierzcholek2) + wierzcholek.Wartosc;
                if (najlepsza == null || tymZysk > zysk)
                {
                    najlepsza = krawedz;
                    zysk = tymZysk;
                }
            }
            return najlepsza;
        }


        private bool Step()
        {
            if (_notUsed.Count == 0)
            {
                return false;
            }

            var statystyki = _notUsed.Select(v =>
            {
                double zysk;
                Krawedz przecinana = BestGainEdge(v, out zysk);
                return new { Wierzcholek = v, Zysk = zysk, Przecinana = przecinana };
            });

            var ranking = statystyki.OrderBy(s => -1 * s.Zysk);

            if (ranking.First().Zysk > 0)
            {
                AddToSolution(ranking.First().Wierzcholek, ranking.First().Przecinana);
                return true;
            }
            else
            {
                return false;
            }
        }


        private void SortSolution()
        {
            for (int i = 0; i < _solution.Krawedzie.Count; i++)
            {
                Wierzcholek szukany = _solution.Krawedzie[i].Wierzcholek2;
                Krawedz nastepna = null;
                int iteNastepna = 0;
                for (int j = i + 1; j < _solution.Krawedzie.Count; j++)
                {
                    if (_solution.Krawedzie[j].Wierzcholek1 == szukany)
                    {
                        nastepna = _solution.Krawedzie[j];
                        iteNastepna = j;
                        break;
                    }
                }
                if (nastepna != null)
                {
                    _solution.Krawedzie.Swap(i + 1, iteNastepna);
                }
            }
        }

        private void WypiszRozwiazanie()
        {
            foreach (Krawedz krawedz in _solution.Krawedzie)
            {
                Console.WriteLine($"{krawedz.Wierzcholek1.Indeks} {krawedz.Wierzcholek2.Indeks}");
            }
        }
    }
}
