using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FajnyKomiwojazer
{
    public class LocalSearch
    {
        private enum WhatToDo
        {
            Nothing,
            RemoveVerticle,
            AddVerticle,
            SwapEdges,
            SwitchVerticle
        }

        private Graf _solution;
        private List<Wierzcholek> _notUsed;
        private Graf _instance;

        /// <summary>
        /// For solving from scratch
        /// </summary>
        /// <param name="graf"></param>
        public LocalSearch(Graf graf)
        {
            _instance = graf;
            Clean();
        }

        /// <summary>
        /// For continuation
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="solution"></param>
        public LocalSearch(Graf instance, Graf solution)
        {
            _instance = instance;
            _solution = solution;
            _notUsed = _instance.Wierzcholki.ToList().Where(w =>
            {
                foreach (Krawedz e in _solution.Krawedzie)
                {
                    if (e.Wierzcholek1 == w || e.Wierzcholek2 == w)
                    {
                        return false;
                    }
                }
                return true;
            }).ToList();
        }

        private void Clean()
        {
            _solution = new Graf();
            _solution.Wierzcholki = _instance.Wierzcholki.ToList();
            _notUsed = _instance.Wierzcholki.ToList();
        }

        public Graf Solve()
        {
            while (Step())
            {

            }
            SortSolution();
            return _solution;
        }


        private void AddVerticle(Wierzcholek wierzcholek, Krawedz krawedz)
        {
            _solution.Krawedzie.Remove(krawedz);
            _solution.Krawedzie.Add(new Krawedz(krawedz.Wierzcholek1, wierzcholek));
            _solution.Krawedzie.Add(new Krawedz(wierzcholek, krawedz.Wierzcholek2));
            _notUsed.Remove(wierzcholek);
        }

        private void RemoveVerticle(Wierzcholek wierzcholek)
        {
            Krawedz e1 = _solution.Krawedzie.FirstOrDefault(k => k.Wierzcholek2 == wierzcholek);
            Krawedz e2 = _solution.Krawedzie.FirstOrDefault(k => k.Wierzcholek1 == wierzcholek);
            _solution.Krawedzie.Add(new Krawedz(e1.Wierzcholek1, e2.Wierzcholek2));
            _solution.Krawedzie.RemoveAll(e => e == e1 || e == e2);
            _notUsed.Add(wierzcholek);
        }


        private Krawedz BestGainEdgeAddVerticle(Wierzcholek wierzcholek, out double zysk)
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

        private Wierzcholek BestGainAddVerticle(out Krawedz bestEdge, out double zysk)
        {
            Wierzcholek najlepszy = null;
            zysk = 0;
            bestEdge = null;
            foreach(Wierzcholek w in _notUsed)
            {
                double tymZysk;
                Krawedz tymKrawedz = BestGainEdgeAddVerticle(w, out tymZysk);
                if (tymKrawedz == null)
                {
                    continue;
                }
                if (najlepszy == null || tymZysk > zysk)
                {
                    najlepszy = w;
                    bestEdge = tymKrawedz;
                    zysk = tymZysk;
                }
            }
            return najlepszy;
        }


        private Wierzcholek BestGainRemoveVerticle(out double zysk)
        {
            Wierzcholek najlepszy = null;
            zysk = 0;
            foreach (Krawedz krawedz1 in _solution.Krawedzie)
            {
                Wierzcholek wierzcholek = krawedz1.Wierzcholek2;
                Krawedz krawedz2 = _solution.Krawedzie.FirstOrDefault(k => k.Wierzcholek1 == wierzcholek);
                double tymZysk = krawedz1.Dlugosc + krawedz2.Dlugosc - wierzcholek.Wartosc - krawedz1.Wierzcholek1.Odleglosc(krawedz2.Wierzcholek2);
                if (najlepszy == null || tymZysk > zysk)
                {
                    najlepszy = wierzcholek;
                    zysk = tymZysk;
                }
            }
            return najlepszy;
        }


        private bool Step()
        {
            WhatToDo whatToDo = WhatToDo.Nothing;
            double bestGain = 0;

            double removeZysk;
            Wierzcholek bestRemove;
            bestRemove = BestGainRemoveVerticle(out removeZysk);

            if (bestRemove != null && removeZysk > bestGain)
            {
                whatToDo = WhatToDo.RemoveVerticle;
                bestGain = removeZysk;
            }

            double addVZysk;
            Wierzcholek bestAddV;
            Krawedz bestAddVEdge;
            bestAddV = BestGainAddVerticle(out bestAddVEdge, out addVZysk);

            if (bestAddV != null && addVZysk > bestGain)
            {
                whatToDo = WhatToDo.AddVerticle;
                bestGain = addVZysk;
            }


            switch (whatToDo)
            {
                case WhatToDo.Nothing:
                    return false;
                case WhatToDo.RemoveVerticle:
                    RemoveVerticle(bestRemove);
                    break;
                case WhatToDo.AddVerticle:
                    AddVerticle(bestAddV, bestAddVEdge);
                    break;
            }

            return true;
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
