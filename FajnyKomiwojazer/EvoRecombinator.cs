using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FajnyKomiwojazer
{
    public class EvoRecombinator
    {
        private Random _rand;

        private List<List<int>> _fragments = new List<List<int>>();
        private List<int> _lone = new List<int>();
        private List<int> _initial1;
        private List<int> _initial2;
        private List<int> _current1;
        private List<int> _current2;

        public EvoRecombinator(List<int> graf1, List<int> graf2, Random rand)
        {
            _rand = rand;
            _initial1 = graf1.ToList();
            _current1 = graf1.ToList();
            _initial2 = graf2.ToList();
            _current2 = graf2.ToList();
        }

        public List<int> Recombine()
        {
            TearDown();
            return BuildUp();
        }

        private List<int> BuildUp()
        {
            int sharedCount = _fragments.Sum(f => f.Count);
            int difference = Math.Abs(_initial1.Count - _initial2.Count);
            int randomized = _rand.Next(difference);
            int desiredCount = Math.Min(_initial1.Count, _initial2.Count) + randomized;
            int verticlesToAdd = desiredCount - sharedCount;

            while (verticlesToAdd > 0)
            {
                int addedIndex = _rand.Next(_lone.Count);
                int fragmentChosen = _rand.Next(_fragments.Count);
                int end = _rand.Next(1);
                if(end == 0)
                {
                    _fragments[fragmentChosen].Insert(0, _lone[addedIndex]);
                }
                else
                {
                    _fragments[fragmentChosen].Add(_lone[addedIndex]);
                }
                _lone.RemoveAt(addedIndex);
                verticlesToAdd -= 1;
            }

            while (_fragments.Count > 1)
            {
                int fragmentChosen = _rand.Next(_fragments.Count - 1);
                int flip = _rand.Next(1);
                if (flip == 1)
                {
                    _fragments.Last().Reverse();
                }
                int end = _rand.Next(1);
                if (end == 0)
                {
                    _fragments.Last().AddRange(_fragments[fragmentChosen]);
                    _fragments[fragmentChosen] = _fragments.Last();
                }
                else
                {
                    _fragments[fragmentChosen].AddRange(_fragments.Last());
                }
                _fragments.RemoveAt(_fragments.Count - 1);
            }
            return _fragments[0];
        }

        private void TearDown()
        {
            while (Bite()) { };
            _lone.AddRange(_current2.Where(i => i != -1));
        }

        private void SeparateShared(List<int> shared)
        {
            _fragments.Add(shared);
            foreach(int verticle in shared)
            {
                _current1.Remove(verticle);
                _current2.Remove(verticle);
            }
        }

        private bool Bite()
        {
            int startingPos1 = -1;
            for(int i = 0; i < _current1.Count; i++)
            {
                if(_current1[i] != -1)
                {
                    startingPos1 = i;
                    break;
                }
            }

            if(startingPos1 == -1)
            {
                return false;
            }

            int startingPos2 = -1;
            for (int i = 0; i < _current2.Count; i++)
            {
                if (_current2[i] == _current1[startingPos1])
                {
                    startingPos2 = i;
                    break;
                }
            }

            if (startingPos2 == -1)
            {
                _lone.Add(_current1[startingPos1]);
                _current1[startingPos1] = -1;
                return true;
            }

            List<int> shared = new List<int>();
            shared.Add(_current1[startingPos1]);

            if (_current1.Count == 1 || _current2.Count == 1)
            {
                SeparateShared(shared);
                return true;
            }

            bool? sameDirection = null;

            int next2 = (startingPos2 + 1) % _current2.Count;
            int prev2 = (startingPos2 - 1 + _current2.Count) % _current2.Count;

            int next1 = (startingPos1 + 1) % _current1.Count;
            //check for direction forward
            if (next1 != -1)
            {

                if (_current2[next2] != -1 && _current2[next2] == _current1[next1])
                {
                    sameDirection = true;
                }

                if (_current2[prev2] != -1 && _current2[prev2] == _current1[next1])
                {
                    sameDirection = false;
                }
            }

            int prev1 = (startingPos1 + -1 + _current1.Count) % _current1.Count;
            //check for direction backward
            if (sameDirection != null && prev1 != -1)
            {
                if (_current2[next2] != -1 && _current2[next2] == _current1[prev1])
                {
                    sameDirection = false;
                }

                if (_current2[prev2] != -1 && _current2[prev2] == _current1[prev1])
                {
                    sameDirection = true;
                }
            }


            if (sameDirection == null)
            {
                SeparateShared(shared);
                return true;
            }

            if (sameDirection.Value)
            {
                //forward
                while (_current1[next1] == _current2[next2] && next1 != startingPos1)
                {
                    shared.Add(_current1[next1]);
                    next1 = (next1 + 1) % _current1.Count;
                    next2 = (next2 + 1) % _current2.Count;
                }
                //backward
                while (_current1[prev1] == _current2[prev2] && prev1 != (next1 - 1 + _current1.Count) % _current1.Count)
                {
                    shared.Insert(0, _current1[prev1]);
                    prev1 = (prev1 - 1 + _current1.Count) % _current1.Count;
                    prev2 = (prev2 - 1 + _current2.Count) % _current2.Count;
                }
            }
            else
            {
                //forward
                while (_current1[next1] == _current2[prev2] && next1 != startingPos1)
                {
                    shared.Add(_current1[next1]);
                    next1 = (next1 + 1) % _current1.Count;
                    prev2 = (prev2 - 1 + _current2.Count) % _current2.Count;
                }
                //backward
                while (_current1[prev1] == _current2[next2] && prev1 != (next1 - 1 + _current1.Count) % _current1.Count)
                {
                    shared.Insert(0, _current1[prev1]);
                    prev1 = (prev1 - 1 + _current1.Count) % _current1.Count;
                    next2 = (next2 + 1) % _current2.Count;
                }
            }

            SeparateShared(shared);
            return true;
        }
    }
}
