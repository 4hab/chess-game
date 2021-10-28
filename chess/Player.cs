using System;
using System.Collections.Generic;
using System.Text;

namespace chess
{
    class Player
    {
        private string _name;
        private int _score;
        private Colors _color;

        public Player(string name, Colors color)
        {
            _name = name;
            _score = 39;
            _color = color;

        }
        public string name=> _name;
        
        public int score => _score;

        public void minusScore(int val)
        {
            _score -= val;
        }
        public Colors color => _color;
    }
}
