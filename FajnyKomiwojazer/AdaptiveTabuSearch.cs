using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FajnyKomiwojazer
{
    public class AdaptiveTabuSearch
    {
        private int _tabuLevel = 0;
        private int _minTabuRaise = 1;
        private int _randTabuRaise = 49;
        private int _minTabu = 0;

        private Queue<Wierzcholek> _wierzcholkiTabu = new Queue<Wierzcholek>();
        private Queue<Krawedz> _krawedzieTabu = new Queue<Krawedz>();

        private double _highScore = 0;
        private double _lastScore = 0;
        private Graf _scoreHolder;

        private ulong _iterations = 0;
        private ulong _tabuDecayTick = 13;

        private Stopwatch _timer = new Stopwatch();

        private Random _rand = new Random();

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
        public AdaptiveTabuSearch(Graf graf)
        {
            _instance = graf;
            Clean();
        }

        /// <summary>
        /// For continuation
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="solution"></param>
        public AdaptiveTabuSearch(Graf instance, Graf solution)
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
            foreach (Wierzcholek w in _solution.Wierzcholki)
            {
                w.KrawedzDo = null;
                w.KrawedzOd = null;
            }
            foreach (Krawedz k in _solution.Krawedzie)
            {
                k.Wierzcholek1.KrawedzOd = k;
                k.Wierzcholek2.KrawedzDo = k;
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
            for (int i = 0; i < liczba - 1; i++)
            {
                if (_solution.Krawedzie[i].Wierzcholek2 != _solution.Krawedzie[i + 1].Wierzcholek1)
                {
                    Console.WriteLine("Graph isn't valid");
                    return false;
                }
            }

            if (_solution.Krawedzie.First().Wierzcholek1 != _solution.Krawedzie.Last().Wierzcholek2)
            {
                Console.WriteLine("Graph isn't valid");
                return false;
            }

            return true;
        }

        public Graf Solve()
        {
            IsValid();
            _timer.Start();
            while (_timer.Elapsed.TotalMilliseconds < 7000)
            {
                Step();
                _iterations += 1;
                //IsValid();
            }
            _timer.Stop();
            _solution = _scoreHolder;
            SortSolution();
            return _scoreHolder;
        }



        private void AddVerticleMove(Wierzcholek wierzcholek, Krawedz krawedz)
        {
            _solution.RemoveKrawedz(krawedz);
            _solution.AddKrawedz(new Krawedz(krawedz.Wierzcholek1, wierzcholek));
            _solution.AddKrawedz(new Krawedz(wierzcholek, krawedz.Wierzcholek2));
            _notUsed.Remove(wierzcholek);
        }

        public void RemoveVerticleMove(Wierzcholek wierzcholek)
        {
            Krawedz e1 = wierzcholek.KrawedzDo;
            Krawedz e2 = wierzcholek.KrawedzOd;
            _solution.RemoveKrawedz(e1);
            _solution.RemoveKrawedz(e2);
            _solution.AddKrawedz(new Krawedz(e1.Wierzcholek1, e2.Wierzcholek2));
            _notUsed.Add(wierzcholek);
        }

        public void RecombineEdgesMove(Krawedz krawedz1, Krawedz krawedz2)
        {
            _solution.RemoveKrawedz(krawedz1);
            _solution.RemoveKrawedz(krawedz2);
            Krawedz poczatekOdwrocenia = new Krawedz(krawedz1.Wierzcholek1, krawedz2.Wierzcholek1);
            Krawedz koniecOdwrocenia = new Krawedz(krawedz1.Wierzcholek2, krawedz2.Wierzcholek2);
            Krawedz doOdwrocenia = poczatekOdwrocenia.Wierzcholek2.KrawedzDo;
            while (doOdwrocenia != null)
            {
                Krawedz obracana = doOdwrocenia;
                doOdwrocenia = doOdwrocenia.Poprzednia();
                obracana.Obroc();
            }
            _solution.AddKrawedz(poczatekOdwrocenia);
            _solution.AddKrawedz(koniecOdwrocenia);
        }


        private void SwitchVerticleMove(Wierzcholek usuwany, Wierzcholek dodawany)
        {
            Wierzcholek w1 = null;
            Wierzcholek w2 = null;
            w1 = usuwany.KrawedzDo.Wierzcholek1;
            w2 = usuwany.KrawedzOd.Wierzcholek2;
            _solution.RemoveKrawedz(usuwany.KrawedzDo);
            _solution.RemoveKrawedz(usuwany.KrawedzOd);
            _solution.AddKrawedz(new Krawedz(w1, dodawany));
            _solution.AddKrawedz(new Krawedz(dodawany, w2));
            _notUsed.Remove(dodawany);
            _notUsed.Add(usuwany);
        }


        private Krawedz BestEdgeToAddVerticle(Wierzcholek wierzcholek, out double zysk)
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

        private Wierzcholek BestAddVerticle(out Krawedz bestEdge, out double zysk)
        {
            Wierzcholek najlepszy = null;
            zysk = 0;
            bestEdge = null;
            foreach (Wierzcholek w in _notUsed)
            {
                if(_wierzcholkiTabu.Any(t => t.Equals(w)))
                {
                    continue;
                }
                double tymZysk;
                Krawedz tymKrawedz = BestEdgeToAddVerticle(w, out tymZysk);
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


        private Wierzcholek BestRemoveVerticle(out double zysk)
        {
            Wierzcholek najlepszy = null;
            zysk = 0;
            foreach (Krawedz krawedz1 in _solution.Krawedzie)
            {
                Wierzcholek wierzcholek = krawedz1.Wierzcholek2;
                if (_wierzcholkiTabu.Any(t => t.Equals(wierzcholek)))
                {
                    continue;
                }
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

        private Krawedz BestOtherEdgeRecombine(Krawedz pierwsza, out double zysk)
        {
            Krawedz najlepsza = null;
            zysk = 0;
            foreach (Krawedz druga in _solution.Krawedzie)
            {
                if (pierwsza == druga || druga.Wierzcholek1 == pierwsza.Wierzcholek2 || pierwsza.Wierzcholek1 == druga.Wierzcholek2)
                {
                    continue;
                }
                if (_krawedzieTabu.Any(t => t.Equals(druga)))
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

        private Krawedz BestRecombine(out Krawedz druga, out double zysk)
        {
            Krawedz najlepsza = null;
            zysk = 0;
            druga = null;
            foreach (Krawedz krawedz in _solution.Krawedzie)
            {
                if (_krawedzieTabu.Any(t => t.Equals(krawedz)))
                {
                    continue;
                }

                double tymZysk;
                Krawedz tymKrawedz = BestOtherEdgeRecombine(krawedz, out tymZysk);
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

        private Wierzcholek BestAddedSwitchVerticle(Wierzcholek usuwany, out double zysk)
        {
            Wierzcholek w1 = null;
            Wierzcholek w2 = null;
            w1 = usuwany.KrawedzDo.Wierzcholek1;
            w2 = usuwany.KrawedzOd.Wierzcholek2;
            Wierzcholek najlepszy = null;
            zysk = 0;
            foreach (Wierzcholek dodawany in _notUsed)
            {
                if (_wierzcholkiTabu.Any(t => t.Equals(dodawany)))
                {
                    continue;
                }
                double tymZysk = usuwany.KrawedzDo.Dlugosc + usuwany.KrawedzOd.Dlugosc + dodawany.Wartosc
                            - w1.Odleglosc(dodawany) - w2.Odleglosc(dodawany) - usuwany.Wartosc;
                if (najlepszy == null || tymZysk > zysk)
                {
                    najlepszy = dodawany;
                    zysk = tymZysk;
                }
            }
            return najlepszy;
        }

        /// <summary>
        /// Returns verticle to remove
        /// </summary>
        /// <param name="dodawany"></param>
        /// <param name="zysk"></param>
        /// <returns></returns>
        private Wierzcholek BestSwitchVerticle(out Wierzcholek dodawany, out double zysk)
        {
            Wierzcholek najlepszy = null;
            zysk = 0;
            dodawany = null;
            foreach (Krawedz k in _solution.Krawedzie)
            {
                Wierzcholek tmpUsuwany = k.Wierzcholek1;
                if (_wierzcholkiTabu.Any(t => t.Equals(tmpUsuwany)))
                {
                    continue;
                }
                double tymZysk;
                Wierzcholek tmpDodawany = BestAddedSwitchVerticle(tmpUsuwany, out tymZysk);
                if (tmpDodawany == null)
                {
                    continue;
                }
                if (najlepszy == null || tymZysk > zysk)
                {
                    najlepszy = tmpUsuwany;
                    dodawany = tmpDodawany;
                    zysk = tymZysk;
                }
            }
            return najlepszy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>gain</returns>
        private bool Step()
        {
            WhatToDo whatToDo = WhatToDo.Nothing;
            double bestGain = -2000000000d;

            double removeZysk;
            Wierzcholek bestRemove;
            bestRemove = BestRemoveVerticle(out removeZysk);

            if (bestRemove != null && removeZysk > bestGain)
            {
                whatToDo = WhatToDo.RemoveVerticle;
                bestGain = removeZysk;
            }

            double addVZysk;
            Wierzcholek bestAddV;
            Krawedz bestAddVEdge;
            bestAddV = BestAddVerticle(out bestAddVEdge, out addVZysk);

            if (bestAddV != null && addVZysk > bestGain)
            {
                whatToDo = WhatToDo.AddVerticle;
                bestGain = addVZysk;
            }

            double recombineZysk;
            Krawedz druga;
            Krawedz pierwsza = BestRecombine(out druga, out recombineZysk);
            if (pierwsza != null && druga != null && recombineZysk > bestGain)
            {
                whatToDo = WhatToDo.RecombineEdges;
                bestGain = recombineZysk;
            }

            double switchZysk;
            Wierzcholek dodawany;
            Wierzcholek usuwany = BestSwitchVerticle(out dodawany, out switchZysk);
            if (dodawany != null && usuwany != null && switchZysk > bestGain)
            {
                whatToDo = WhatToDo.SwitchVerticle;
                bestGain = switchZysk;
            }


            if(bestGain < 0)
            {
                double current = _solution.GetValueSoFarByEdge() - _solution.GetDistanceSoFarByEdge();
                if (current - _highScore > 0.01)
                {
                    _highScore = current;
                    _scoreHolder = _solution.Clone();
                    //Console.WriteLine(_highScore);
                }
                if (Math.Abs(_highScore - current) < 0.01 || Math.Abs(_lastScore - current) < 0.01)
                {
                    //Console.WriteLine($"Returned to solution, current length: {_tabuLevel}, iteration: {_iterations}");
                    _tabuLevel = _minTabuRaise + (_rand.Next() % _randTabuRaise);
                }
                
            }

            if (_iterations % _tabuDecayTick == 0)
            {
                _tabuLevel -= 1;
            }

            if (_tabuLevel < _minTabu)
            {
                _tabuLevel = _minTabu;
            }


            switch (whatToDo)
            {
                case WhatToDo.RemoveVerticle:
                    RemoveVerticleMove(bestRemove);
                    _wierzcholkiTabu.Enqueue(bestRemove);
                    break;
                case WhatToDo.AddVerticle:
                    AddVerticleMove(bestAddV, bestAddVEdge);
                    _wierzcholkiTabu.Enqueue(bestAddV);
                    break;
                case WhatToDo.RecombineEdges:
                    RecombineEdgesMove(pierwsza, druga);
                    _krawedzieTabu.Enqueue(pierwsza);
                    _krawedzieTabu.Enqueue(druga);
                    break;
                case WhatToDo.SwitchVerticle:
                    SwitchVerticleMove(usuwany, dodawany);
                    _wierzcholkiTabu.Enqueue(usuwany);
                    _wierzcholkiTabu.Enqueue(dodawany);
                    break;
                default:
                    throw new Exception("This shouldn't happen.");
            }

            while (_wierzcholkiTabu.Count > _tabuLevel)
            {
                _wierzcholkiTabu.Dequeue();
            }

            while (_krawedzieTabu.Count > _tabuLevel)
            {
                _krawedzieTabu.Dequeue();
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
