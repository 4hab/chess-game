using System;
using System.Collections.Generic;
using System.Text;

namespace chess
{
    public delegate void Notify();
    class GameObserver
    {
        private static GameObserver _instance;
        public static GameObserver instance => _instance = _instance == null ? new GameObserver() : _instance;
        private GameObserver()
        {
            _player1 = new Player("Player 1", PieceColor.white);
            _player2 = new Player("Player 2", PieceColor.black);
            _currentPlayer = _player1;
        }

        public  Notify gameOver;
        public  Notify gameOverDraw;
        public  Notify scoreUpdated;
        private Player _player1;
        private Player _player2;
        private Player _currentPlayer;


        public int score => whitePlayer.score - blackPlayer.score;
        public void switchPlayer()
        {
            if (_currentPlayer == _player1)
                _currentPlayer = _player2;
            else
                _currentPlayer = _player1;
        }

        public Player currentPlayer => _currentPlayer;
        public Player otherPlayer => _currentPlayer == _player1 ? _player2 : _player1;

        public Player whitePlayer => _player1;
        public Player blackPlayer => _player2;

        public void setPlayersNames(string name1, string name2)
        {
            whitePlayer.setName(name1);
            blackPlayer.setName(name2);
        }

        public void updateScore()
        {
            scoreUpdated.Invoke();
        }

        public void reset()
        {
            _currentPlayer = _player1;
            _player1.resetData();
            _player2.resetData();
            Board.reset();
            History.instance.clear();
        }
        public void isGameOver()
        {
            Board board = Board.instance;
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    BoardTile tile = board.of(new Coordinates(i, j));
                    if (!tile.isEmpty() && currentPlayer.isMyPiece(tile.piece) && tile.piece.countAvailableTiles() > 0) 
                    {
                        return;
                    }
                }
            }
            if(!currentPlayer.isAttacked)
            {
                //draw
                gameOverDraw.Invoke();
                return;
            }
            gameOver.Invoke();
            return;
        }
    }


}
