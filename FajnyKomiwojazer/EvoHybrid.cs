using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FajnyKomiwojazer
{
    public class EvoHybrid
    {
        public static int MaxPopulation = 20;

        private Graf _instance;
        private List<Graf> _population = new List<Graf>();
        private Graf _best;
        private Graf _contender;
        private Random _randomizer;

        private Stopwatch _timer = new Stopwatch();

        public EvoHybrid(Graf instance)
        {
            _instance = instance;
            _randomizer = new Random();
        }


        public void InitiatePopulation()
        {
            for (int i = 0; i < MaxPopulation; i++)
            {
                Graf clone = _instance.Clone();
                RandomCycle baseAlg = new RandomCycle(clone, _randomizer);
                Graf random = baseAlg.Compute();
                LocalSearch localSearch = new LocalSearch(clone, random, false);
                Graf ls = localSearch.Solve();
                ls.Wynik = ls.GetScore();
                ls.Representation = ls.Codify();
                Graf graf = Graf.Decodify(_instance.Clone(), ls.Representation);
                graf.Representation = graf.Codify();
                Console.WriteLine(SameGraph(ls.Representation, graf.Representation));
                if (!PerformSelection(ls))
                {
                    i -= 1;
                }
            }
        }


        private bool PerformSelection(Graf dodawany)
        {
            if(dodawany == null)
            {
                return false;
            }

            foreach(Graf graf in _population)
            {
                if(SameGraph(graf.Representation, dodawany.Representation))
                {
                    return false;
                }
            }

            if(_population.Count == MaxPopulation)
            {
                Graf worst = null;
                foreach(Graf graf in _population)
                {
                    if (worst == null || graf.Wynik < worst.Wynik)
                    {
                        worst = graf;
                    }
                }
                if(dodawany.Wynik < worst.Wynik)
                {
                    return false;
                }
                _population.Remove(worst);
            }
            _population.Add(dodawany);
            if (_best == null || dodawany.Wynik > _best.Wynik)
            {
                _best = dodawany;
            }
            return true;
        }

        public Graf Compute(int ms)
        {
            InitiatePopulation();

            _timer.Start();
            while (_timer.Elapsed.TotalMilliseconds < ms)
            {
                CreateGeneration();
                PerformSelection(_contender);
            }
            _timer.Stop();
            return _best;
        }

        private bool SameGraph(List<int> repr1, List<int> repr2)
        {
            if(repr1.Count != repr2.Count)
            {
                return false;
            }
            if(repr1.Count == 0)
            {
                return true;
            }
            int starting = repr1[0];
            int pos1 = 0;
            int pos2 = -1;
            for (int i = 0; i < repr2.Count; i++)
            {
                if(repr2[i] == starting)
                {
                    pos2 = i;
                    break;
                }
            }
            if(pos2 == -1)
            {
                return false;
            }

            if (repr1.Count == 1)
            {
                return true;
            }

            pos1 += 1;
            int current = repr1[1];
            bool? sameDirection = null;

            int next2 = (pos2 + 1) % repr2.Count;
            int prev2 = (pos2 - 1 + repr2.Count) % repr2.Count;

            if (repr2[next2] == current)
            {
                sameDirection = true;
            }
            if (repr2[prev2] == current)
            {
                sameDirection = false;
            }

            if(sameDirection == null)
            {
                return false;
            }
            
            if (repr1.Count == 2)
            {
                return true;
            }

            for (int i = 2; i < repr1.Count; i++)
            {
                current = repr1[i];
                if (sameDirection.Value)
                {
                    next2 = (next2 + 1) % repr2.Count;
                    if(current != repr2[next2])
                    {
                        return false;
                    }
                }
                else
                {
                    prev2 = (prev2 - 1 + repr2.Count) % repr2.Count;
                    if(current != repr2[prev2])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void CreateGeneration()
        {
            int index1 = _randomizer.Next(_population.Count);
            int index2 = _randomizer.Next(_population.Count);
            while(index2 == index1)
            {
                index2 = _randomizer.Next(_population.Count);
            }
            EvoRecombinator recomb = new EvoRecombinator();
            List<int> newRepr = recomb.Recombine(_population[index1].Representation, _population[index2].Representation);
            _contender = Graf.Decodify(_instance.Clone(), newRepr);
            _contender.Wynik = _contender.GetScore();
            _contender.Representation = _contender.Codify();
        }
    }
}
