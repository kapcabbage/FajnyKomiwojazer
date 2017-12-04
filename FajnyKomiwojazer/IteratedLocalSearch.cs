using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FajnyKomiwojazer
{
    public class IteratedLocalSearch
    {
        private Graf Graf;
        private double Milisec;
        private Stopwatch Timer;
        private Random Randomizer;

        public IteratedLocalSearch(Graf graf, double milisec)
        {
            Graf = graf;
            Milisec = milisec;
            Timer = new Stopwatch();
            Randomizer = new Random();
        }


        public Graf Compute()
        {
            Graf computed = new Graf();
            Timer.Start();
            RandomCycle baseAlg = new RandomCycle(Graf);
            var randomGraph = baseAlg.Compute();
            LocalSearch lsAlg = new LocalSearch(Graf,randomGraph);
            computed = lsAlg.Solve();
            Graf best = computed;
            while(Timer.Elapsed.TotalMilliseconds < Milisec)
            {
                lsAlg = new LocalSearch(Graf,Perturbance(computed));
                computed = lsAlg.Solve();
                if((computed.GetValueSoFarByEdge() - computed.GetDistanceSoFarByEdge()) > (best.GetValueSoFarByEdge() - best.GetDistanceSoFarByEdge()))
                {
                    best = computed;
                }
            }
            Timer.Stop();
            return best;
        }

        private Graf Perturbance(Graf graf)
        {
            Graf computed = new Graf();
            var iter = Randomizer.Next(5);
            for (int i = 0; i < iter; i++)
            {
                var alg = new LocalSearch(Graf,graf);
                var currGraph = alg.GetCurrentGraph();
                var addableVert = graf.Wierzcholki.Where(x => !currGraph.Wierzcholki.Contains(x)).ToList();
                var randVert = Randomizer.Next(currGraph.Wierzcholki.Count);
                var randEdge1 = Randomizer.Next(currGraph.Krawedzie.Count);
                var randEdge2 = Randomizer.Next(currGraph.Krawedzie.Count);
                while(Math.Abs(randEdge1-randEdge2) == 1)
                {
                    randEdge1 = Randomizer.Next(alg.GetCurrentGraph().Krawedzie.Count);
                    randEdge2 = Randomizer.Next(alg.GetCurrentGraph().Krawedzie.Count);
                }
                var rand = Randomizer.Next(1);
                switch (rand)
                {
                    case 0:
                        
                        alg.RemoveVerticleMove(currGraph.Wierzcholki[randVert]);
                        computed = alg.GetCurrentGraph();
                        break;
                    case 1:
                        alg.RecombineEdgesMove(currGraph.Krawedzie[randEdge1], currGraph.Krawedzie[randEdge2]);
                        computed = alg.GetCurrentGraph();
                        break;
                    case 2:

                        alg.AddVerticleMove(addableVert[Randomizer.Next(addableVert.Count)], currGraph.Krawedzie[randEdge2]);
                        computed = alg.GetCurrentGraph();
                        break;
                    case 3:
                        
                        alg.SwitchVerticleMove(addableVert[Randomizer.Next(addableVert.Count)], currGraph.Wierzcholki[randVert]);
                        computed = alg.GetCurrentGraph();
                        break;

                }
            }
            return computed;
        }

    }
}
