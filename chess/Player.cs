using System;
using System.Collections.Generic;
using System.Text;

namespace chess
{
    class Player
    {
        private string _name;
        private int _score;
        private bool _isWhite;

        public Player(string name, bool isWhite)
        {
            _name = name;
            _score = 39;
            _isWhite = isWhite;

        }
        public string name()
        {
            return _name;
        }
        public int score => _score;
        public bool isWhite()
        {
            return _isWhite;
        }
    }
}
