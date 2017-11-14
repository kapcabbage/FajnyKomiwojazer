using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FajnyKomiwojazer
{
    public class Graf
    {
        private List<Wierzcholek> _wierzcholki = new List<Wierzcholek>();


        public List<Wierzcholek> Wierzcholki
        {
            get
            {
                return _wierzcholki.ToList();
            }
            set
            {
                _wierzcholki = value;
            }
        }

        public void AddWiercholek(Wierzcholek wiercholek)
        {
            _wierzcholki.Add(wiercholek);
        }
    }
}
