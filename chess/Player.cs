using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace chess
{
    class Player
    {
        private string _name;
        private int _score;
        private PieceColor _color;
        private bool _castle;
        private bool _isAttacked;
        private bool _virtualAttack = false;
        private BoardTile _kingTile;

        public Player(string name, PieceColor color)
        {
            _name = name;
            _score = 39;
            _color = color;
            _castle = true;
            _isAttacked = false;
            _kingTile = (color == PieceColor.white) ? Board.instance.of(7, 4) : Board.instance.of(0, 4);

        }
        public string name => _name;
        public int score => _score;
        public PieceColor color => _color;
        public bool virtualAttack => _virtualAttack;
        public bool castle => _castle;
        public bool isAttacked => _isAttacked;
        public void setName(string name)
        {
            _name = name;
        }

        public void resetData()
        {
            _score = 39;
            _castle = true;
            _isAttacked = false;
            _virtualAttack = false;
            _kingTile = _color == PieceColor.white ? Board.instance.of(7, 4) : Board.instance.of(0, 4);
        }
        public void moveKing(Coordinates c)
        {
            inDanger(false);
            _kingTile = Board.instance.of(c);
        }
        public void inDanger(bool val)
        {
            _isAttacked = val;
            _kingTile.BackColor = Color.Red;
            if(val==false)
            {
                //switch king's tile color to normal
                _kingTile.BackColor = _kingTile.color == PieceColor.white ? Color.White : Color.DarkGray;
            }
        }

        public void minusScore(int val)
        {
            _score -= val;
            GameObserver.instance.updateScore();
        }
        public void attack(bool virtually = false)
        {
            _virtualAttack = virtually;
            Board board = Board.instance;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    board.of(i,j).markDanger(false);

            for (int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if (!board.of(i,j).isEmpty() && isMyPiece(board.of(i,j).piece))
                    {
                        board.of(i,j).piece.attack();
                    }
                }
            }
        }
        public bool isMyPiece(Piece piece) => piece.color == color;
    }
}
