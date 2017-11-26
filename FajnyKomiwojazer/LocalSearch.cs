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
            RecombineEdges,
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
            foreach(Wierzcholek w in _solution.Wierzcholki)
            {
                w.Krawedzie = new List<Krawedz>();
            }
            foreach(Krawedz k in _solution.Krawedzie)
            {
                k.Wierzcholek1.Krawedzie.Add(k);
                k.Wierzcholek2.Krawedzie.Add(k);
            }
        }

        private void Clean()
        {
            _solution = new Graf();
            _solution.Wierzcholki = _instance.Wierzcholki.ToList();
            _notUsed = _instance.Wierzcholki.ToList();
        }

        private bool IsValid()
        {
            SortSolution();
            int liczba = _solution.Krawedzie.Count();
            for(int i = 0; i < liczba -1; i++)
            {
                if(_solution.Krawedzie[i].Wierzcholek2 != _solution.Krawedzie[i + 1].Wierzcholek1)
                {
                    return false;
                }
            }

            if(_solution.Krawedzie.First().Wierzcholek1 != _solution.Krawedzie.Last().Wierzcholek2)
            {
                return false;
            }

            return true;
        }

        public Graf Solve()
        {
            IsValid();
            while (Step())
            {
                IsValid();
            }
            return _solution;
        }



        private void AddVerticleMove(Wierzcholek wierzcholek, Krawedz krawedz)
        {
            _solution.RemoveKrawedz(krawedz);
            _solution.AddKrawedz(new Krawedz(krawedz.Wierzcholek1, wierzcholek));
            _solution.AddKrawedz(new Krawedz(wierzcholek, krawedz.Wierzcholek2));
            _notUsed.Remove(wierzcholek);
        }

        private void RemoveVerticleMove(Wierzcholek wierzcholek)
        {
            Krawedz e1 = _solution.Krawedzie.FirstOrDefault(k => k.Wierzcholek2 == wierzcholek);
            Krawedz e2 = _solution.Krawedzie.FirstOrDefault(k => k.Wierzcholek1 == wierzcholek);
            _solution.AddKrawedz(new Krawedz(e1.Wierzcholek1, e2.Wierzcholek2));
            _solution.RemoveKrawedz(e1);
            _solution.RemoveKrawedz(e2);
            _notUsed.Add(wierzcholek);
        }

        private void RecombineEdgesMove(Krawedz krawedz1, Krawedz krawedz2)
        {
            _solution.RemoveKrawedz(krawedz1);
            _solution.RemoveKrawedz(krawedz2);
            Krawedz poczatekOdwrocenia = new Krawedz(krawedz1.Wierzcholek1, krawedz2.Wierzcholek1);
            Krawedz koniecOdwrocenia = new Krawedz(krawedz1.Wierzcholek2, krawedz2.Wierzcholek2);
            _solution.AddKrawedz(poczatekOdwrocenia);
            _solution.AddKrawedz(koniecOdwrocenia);
            Krawedz doOdwrocenia = poczatekOdwrocenia.Nastepna();
            while (doOdwrocenia != koniecOdwrocenia)
            {
                doOdwrocenia.Obroc();
                doOdwrocenia = doOdwrocenia.Nastepna();
            }
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

        private Krawedz BestGainOtherEdgeRecombine(Krawedz pierwsza, out double zysk)
        {
            Krawedz najlepsza = null;
            zysk = 0;
            foreach (Krawedz druga in _solution.Krawedzie)
            {
                if(pierwsza == druga || druga.Wierzcholek1 == pierwsza.Wierzcholek2 || pierwsza.Wierzcholek1 == druga.Wierzcholek2)
                {
                    continue;
                }
                double tymZysk = pierwsza.Dlugosc + druga.Dlugosc - druga.Wierzcholek1.Odleglosc(pierwsza.Wierzcholek1) - pierwsza.Wierzcholek2.Odleglosc(druga.Wierzcholek2);
                if (najlepsza == null || tymZysk > zysk)
                {
                    najlepsza = druga;
                    zysk = tymZysk;
                }
            }
            return najlepsza;
        }

        private Krawedz BestGainRecombine(out Krawedz druga, out double zysk)
        {
            Krawedz najlepsza = null;
            zysk = 0;
            druga = null;
            foreach (Krawedz krawedz in _solution.Krawedzie)
            {
                double tymZysk;
                Krawedz tymKrawedz = BestGainOtherEdgeRecombine(krawedz, out tymZysk);
                if (tymKrawedz == null)
                {
                    continue;
                }
                if (najlepsza == null || tymZysk > zysk)
                {
                    najlepsza = krawedz;
                    druga = tymKrawedz;
                    zysk = tymZysk;
                }
            }
            return najlepsza;
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

            double recombineZysk;
            Krawedz druga;
            Krawedz pierwsza = BestGainRecombine(out druga, out recombineZysk);
            if (pierwsza != null && druga != null && recombineZysk > bestGain)
            {
                whatToDo = WhatToDo.RecombineEdges;
                bestGain = recombineZysk;
            }


            switch (whatToDo)
            {
                case WhatToDo.Nothing:
                    return false;
                case WhatToDo.RemoveVerticle:
                    RemoveVerticleMove(bestRemove);
                    break;
                case WhatToDo.AddVerticle:
                    AddVerticleMove(bestAddV, bestAddVEdge);
                    break;
                case WhatToDo.RecombineEdges:
                    RecombineEdgesMove(pierwsza, druga);
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
