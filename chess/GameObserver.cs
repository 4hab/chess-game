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
        public static Notify scoreUpdated;
        public static int score=> Player.whitePlayer.score - Player.blackPlayer.score;

        public static void updateScore()
        {
            scoreUpdated.Invoke();
        }
        
        public static bool isSafeMove(Coordinates c1, Coordinates c2)
        {
            //save original situation
            Piece p1 = Board.of(c1).piece;
            Piece p2 = Board.of(c2).piece;
            bool initialDangerStatus = Player.currentPlayer.isAttacked;
            Player.currentPlayer.inDanger(false);

            //move pieces virtually
            Board.of(c2).setPiece(p1, true);
            Board.of(c1).setPiece(null, true);

            //let other player attacks [virtually] to see if the situation is safe or not
            Player.switchPlayer();
            Player.currentPlayer.setVirtualAttack(true);
            Player.currentPlayer.attack();
            Player.currentPlayer.setVirtualAttack(false);
            Player.switchPlayer();
            bool currentDangerStatus = Player.currentPlayer.isAttacked;

            //return to the original situation
            Board.of(c2).setPiece(p2);
            Board.of(c1).setPiece(p1);
            Player.currentPlayer.inDanger(initialDangerStatus);

            //if my king is attacked => false(not safe move) otherwise => true(safe move)
            return !currentDangerStatus;
        }

        public static void reDoMove()
        {
            if (Board.selectedTile != null)
            {
                Board.selectedTile.piece.unmarkAvailableCells();
                Board.select(null);
            }
            MoveRecord record = History.reDo();
            if (record == null)
                return;
            BoardTile sourceTile = Board.of(record.source);
            BoardTile destinationTile = Board.of(record.destination);

            //update score
            Piece p = destinationTile.piece;
            if (p != null)
            {
                Player.otherPlayer.minusScore(p.val);
            }

            sourceTile.piece.moveTo(destinationTile.coordinates);
            destinationTile.setPiece(sourceTile.piece);
            sourceTile.setPiece(null);
            Board.select(null);
            Player.switchPlayer();
        }
        public static void unDoMove()
        {
            if (Board.selectedTile != null)
            {
                Board.selectedTile.piece.unmarkAvailableCells();
                Board.select(null);
            }
            MoveRecord record = History.unDo();
            if (record == null)
                return;
            BoardTile destinationTile = Board.of(record.destination);
            BoardTile sourceTile = Board.of(record.source);

            //update score
            if (record.deadPiece != null)
            {
                Player.currentPlayer.minusScore(record.deadPiece.val * -1);
            }

            destinationTile.piece.moveTo(record.source);

            sourceTile.setPiece(destinationTile.piece);
            destinationTile.setPiece(record.deadPiece);
            Board.select(null);
            Player.switchPlayer();
        }
        public static void isGameOver()
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    BoardTile tile = Board.of(new Coordinates(i, j));
                    if (!tile.isEmpty() && Player.currentPlayer.isMyPiece(tile.piece) && tile.piece.countAvailableTiles() > 0) 
                    {
                        return;
                    }
                }
            }
            gameOver.Invoke();
            return;
        }
    }


}
