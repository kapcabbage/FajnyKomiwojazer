using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FajnyKomiwojazer
{
    public class GreedyCycleWithRegrets
    {

        private Graf _solution;
        private List<Wierzcholek> _notUsed;
        private Graf _instance;

        public GreedyCycleWithRegrets(Graf graf)
        {
            _instance = graf;
            Clean();
        }

        private void Clean()
        {
            _solution = new Graf();
            _solution.Wierzcholki = _instance.GetWierzcholki();
            _notUsed = _instance.GetWierzcholki();
        }

        public Graf Solve(int start)
        {
            Clean();
            Wierzcholek wierzcholek = _solution.GetWiercholek(start);
            _solution.AddKrawedz(new Krawedz(wierzcholek, wierzcholek));
            _notUsed.Remove(wierzcholek);
            while (Step())
            {

            }
            SortSolution();
            return _solution;
        }


        private void AddToSolution(Wierzcholek wierzcholek, Krawedz krawedz)
        {
            _solution.RemoveKrawedz(krawedz);
            _solution.AddKrawedz(new Krawedz(krawedz.Wierzcholek1, wierzcholek));
            _solution.AddKrawedz(new Krawedz(wierzcholek, krawedz.Wierzcholek2));
            _notUsed.Remove(wierzcholek);
        }

        private void RollBack(Krawedz removed)
        {
            _solution.RemoveKrawedz(_solution.Krawedzie.Last());
            _notUsed.Add(_solution.Krawedzie.Last().Wierzcholek2);
            _solution.RemoveKrawedz(_solution.Krawedzie.Last());
            _solution.AddKrawedz(removed);
        }


        private Krawedz BestGainEdge(Wierzcholek wierzcholek, out double zysk)
        {
            Krawedz najlepsza = null;
            zysk = 0;
            foreach (Krawedz krawedz in _solution.Krawedzie)
            {
                double tymZysk = krawedz.Dlugosc - wierzcholek.Odleglosc(krawedz.Wierzcholek1) - wierzcholek.Odleglosc(krawedz.Wierzcholek2) + wierzcholek.Wartosc;
                if(najlepsza == null || tymZysk > zysk)
                {
                    najlepsza = krawedz;
                    zysk = tymZysk;
                }
            }
            return najlepsza;
        }


        private bool Step()
        {
            if(_notUsed.Count == 0)
            {
                return false;
            }

            var statystyki = _notUsed.Select(v =>
            {
                double zysk;
                Krawedz przecinana = BestGainEdge(v, out zysk);
                return new { Wierzcholek = v, Zysk = zysk, Przecinana = przecinana };
            });

            var oplacalne = statystyki.Where(s => s.Zysk >= 0);

            if (oplacalne.Count() > 0)
            {
                var newStats = oplacalne.ToList().Select(s =>
                {
                    Wierzcholek v = s.Wierzcholek;
                    return new { Wierzcholek = v, Zal = Regret(v, oplacalne.Select(ver => ver.Wierzcholek).ToList()), Przecinana = s.Przecinana };
                });
                var ranking = newStats.ToList().OrderBy(s => -1 * s.Zal);

                AddToSolution(ranking.First().Wierzcholek, ranking.First().Przecinana);

                return true;
            }
            else
            {
                return false;
            }
        }


        private double Regret(Wierzcholek v, List<Wierzcholek> vertices)
        {
            double regret = 0;
            double gainNow;
            BestGainEdge(v, out gainNow);
            foreach(Wierzcholek other in vertices.Where(ver => ver != v))
            {
                double notUsed;
                Krawedz tmpEdge = BestGainEdge(other, out notUsed);

                AddToSolution(other, tmpEdge);
                double gainLater;
                BestGainEdge(v, out gainLater);
                RollBack(tmpEdge);

                regret += gainNow - gainLater;
            }

            return regret;
        }


        private void SortSolution()
        {
            for(int i = 0; i < _solution.Krawedzie.Count; i++)
            {
                Wierzcholek szukany = _solution.Krawedzie[i].Wierzcholek2;
                Krawedz nastepna = null;
                int iteNastepna = 0;
                for (int j = i+1; j < _solution.Krawedzie.Count; j++)
                {
                    if(_solution.Krawedzie[j].Wierzcholek1 == szukany)
                    {
                        nastepna = _solution.Krawedzie[j];
                        iteNastepna = j;
                        break;
                    }
                }
                if(nastepna != null)
                {
                    _solution.Krawedzie.Swap(i + 1, iteNastepna);
                }
            }
        }

        private void WypiszRozwiazanie()
        {
            foreach(Krawedz krawedz in _solution.Krawedzie)
            {
                Console.WriteLine($"{krawedz.Wierzcholek1.Indeks} {krawedz.Wierzcholek2.Indeks}");
            }
        }
    }
}
