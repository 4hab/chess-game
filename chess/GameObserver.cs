using System;
using System.Collections.Generic;
using System.Text;

namespace chess
{
    public delegate void Notify();
    class GameObserver
    {
        public static BoardTile selectedTile = null;
        public static Notify gameOver;
        public static Notify gameOverDraw;
        public static Notify scoreUpdated;
        public static int score=> Player.whitePlayer.score - Player.blackPlayer.score;

        public static void updateScore()
        {
            scoreUpdated.Invoke();
        }
        
        public static bool isSafeMove(Coordinates c1, Coordinates c2)
        {
            Board board = Board.instance;
            //save original situation
            Piece p1 = board.of(c1).piece;
            Piece p2 = board.of(c2).piece;
            bool initialDangerStatus = Player.currentPlayer.isAttacked;
            Player.currentPlayer.inDanger(false);

            //move pieces virtually
            board.of(c2).setPiece(p1, true);
            board.of(c1).setPiece(null, true);
            //let other player attacks [virtually] to see if the situation is safe or not
            Player.switchPlayer();
            Player.currentPlayer.attack(virtually: true);
            Player.switchPlayer();
            bool currentDangerStatus = Player.currentPlayer.isAttacked;

            //return to the original situation
            board.of(c2).setPiece(p2);
            board.of(c1).setPiece(p1);
            Player.currentPlayer.inDanger(initialDangerStatus);

            //if my king is attacked => false(not safe move) otherwise => true(safe move)
            return !currentDangerStatus;
        }

        public static void reset()
        {
            Board.reset();
            Player.reset();
            History.clear();
        }
        public static void isGameOver()
        {
            Board board = Board.instance;
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    BoardTile tile = board.of(new Coordinates(i, j));
                    if (!tile.isEmpty() && Player.currentPlayer.isMyPiece(tile.piece) && tile.piece.countAvailableTiles() > 0) 
                    {
                        return;
                    }
                }
            }
            if(!Player.currentPlayer.isAttacked)
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
