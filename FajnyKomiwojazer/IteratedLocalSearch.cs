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
            LocalSearch lsAlg = new LocalSearch(Graf);
            computed = lsAlg.Solve();
            Graf best = computed;
            while(Timer.ElapsedMilliseconds < Milisec)
            {
                lsAlg = new LocalSearch(Perturbance(computed));
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
                var rand = Randomizer.Next(4);
                switch (rand)
                {
                    case 0:
                        
                        var alg = new LocalSearch(graf);
                        //alg.AddVerticleMove();
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;

                }
            }
            return computed;
        }

    }
}
