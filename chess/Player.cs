using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace chess
{
    class Player
    {
        private static Player _player1 = new Player("Player 1", PieceColor.white);
        private static Player _player2 = new Player("Player 2", PieceColor.black);
        private static Player _currentPlayer = _player1;

        public static void switchPlayer()
        {
            if (_currentPlayer == _player1)
                _currentPlayer = _player2;
            else
                _currentPlayer = _player1;
        }

        public static Player currentPlayer => _currentPlayer;
        public static Player otherPlayer => _currentPlayer == _player1 ? _player2 : _player1;

        public static Player whitePlayer => _player1;
        public static Player blackPlayer => _player2;

        //-------------------------------------------------------------------------------------------

        private string _name;
        private int _score;
        private PieceColor _color;
        private bool _castle;
        private bool _isAttacked;
        private bool _virtualAttack = false;
        private BoardTile _kingTile;

        public void setVirtualAttack(bool val) => _virtualAttack = val;
        public bool virtualAttack => _virtualAttack;
        public bool castle => _castle;
        public bool isAttacked => _isAttacked;
        public void moveKing(Coordinates c)
        {
            _kingTile = Board.of(c);
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
        public Player(string name, PieceColor color)
        {
            _name = name;
            _score = 39;
            _color = color;
            _castle = true;
            _isAttacked = false;
            _kingTile = (color == PieceColor.white) ? Board.of(new Coordinates(7, 4)) : Board.of(new Coordinates(0, 4));

        }
        public string name=> _name;
        
        public int score => _score;

        public void minusScore(int val)
        {
            _score -= val;
            GameObserver.updateScore();
        }
        public PieceColor color => _color;
        public void attack()
        {
            BoardTile[,] board = Board.instance;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    board[i, j].markDanger(false);

            for (int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if (!board[i, j].isEmpty() && isMyPiece(board[i, j].piece))
                    {
                        board[i, j].piece.attack();
                    }
                }
            }
        }
        public bool isMyPiece(Piece piece) => piece.color == color;
    }
}
