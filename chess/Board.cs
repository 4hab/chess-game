using System;
using System.Collections.Generic;
using System.Text;

namespace chess
{
    class Board
    {
        private static BoardTile[,] _board = new BoardTile[8, 8];
        public static BoardTile[,] instance => _board;

        public static BoardTile of(Coordinates c) => _board[c.x, c.y];

        private static BoardTile _selectedTile = null;

        public static BoardTile selectedTile => _selectedTile;

        public static void select(BoardTile tile)
        {
            _selectedTile = tile;
        }

        public static void move(Coordinates c1,Coordinates c2)
        {
            //change piece coordinates firs
            Board.of(c1).piece.unmarkAvailableCells();
            Board.of(c1).piece.moveTo(c2);
            //move pieces
            Board.of(c2).setPiece(Board.of(c1).piece);
            Board.of(c1).setPiece(null);
            select(null);
        }

        public static void init()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    _board[row, column] = new BoardTile(new Coordinates(row, column));
                }
            }
            putPawns();
            putRooks();
            putKnights();
            putBishops();
            putKings();
            putQueens();
        }

        public static bool isSafeMove(Coordinates c1,Coordinates c2)
        {
            //save original situation
            Piece p1 = Board.of(c1).piece;
            Piece p2 = Board.of(c2).piece;
            bool initialDangerStatus = Player.currentPlayer.isAttacked;
            Player.currentPlayer.inDanger(false);

            //move pieces virtually
            Board.of(c2).setPiece(p1,true);
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

            MoveRecord record = History.reDo();
            if (record == null)
                return;
            BoardTile sourceTile = Board.of(record.source);
            BoardTile destinationTile = Board.of(record.destination);
            sourceTile.piece.moveTo(destinationTile.coordinates);
            destinationTile.setPiece(sourceTile.piece);
            sourceTile.setPiece(null);
            Board.select(null);
            Player.switchPlayer();
        }

        public static void unDoMove()
        {

            MoveRecord record = History.unDo();
            if (record == null)
                return;
            BoardTile destinationTile = Board.of(record.destination);
            BoardTile sourceTile = Board.of(record.source);

            destinationTile.piece.moveTo(record.source);

            sourceTile.setPiece(destinationTile.piece);
            destinationTile.setPiece(record.deadPiece);
            Board.select(null);
            Player.switchPlayer();
        }

        private static void putPawns()
        {
            for (int column = 0; column < 8; column++)
            {
                Piece blackPawn = new Pawn(new Coordinates(1, column), PieceColor.black);
                Piece whitePawn = new Pawn(new Coordinates(6, column), PieceColor.white);

                _board[1, column] = new BoardTile(blackPawn.coordinates, blackPawn);
                _board[6, column] = new BoardTile(whitePawn.coordinates, whitePawn);

            }
        }
        private static void putRooks()
        {
            int whiteRow = 7, blackRow = 0;

            Piece blackRook1 = new Rook(new Coordinates(blackRow, 0), PieceColor.black);
            Piece blackRook2 = new Rook(new Coordinates(blackRow, 7), PieceColor.black);

            _board[blackRow, 0] = new BoardTile(blackRook1.coordinates, blackRook1);
            _board[blackRow, 7] = new BoardTile(blackRook2.coordinates, blackRook2);


            Piece whiteRook1 = new Rook(new Coordinates(whiteRow, 0), PieceColor.white);
            Piece whiteRook2 = new Rook(new Coordinates(whiteRow, 7), PieceColor.white);

            _board[whiteRow, 0] = new BoardTile(whiteRook1.coordinates, whiteRook1);
            _board[whiteRow, 7] = new BoardTile(whiteRook2.coordinates, whiteRook2);
        }
        private static void putKnights()
        {
            int whiteRow = 7, blackRow = 0;

            Piece blackKnight1 = new Knight(new Coordinates(blackRow, 1), PieceColor.black);
            Piece blackKnight2 = new Knight(new Coordinates(blackRow, 6), PieceColor.black);

            _board[blackRow, 1] = new BoardTile(blackKnight1.coordinates, blackKnight1);
            _board[blackRow, 6] = new BoardTile(blackKnight2.coordinates, blackKnight2);


            Piece whiteKnight1 = new Knight(new Coordinates(whiteRow, 1), PieceColor.white);
            Piece whiteKnight2 = new Knight(new Coordinates(whiteRow, 6), PieceColor.white);

            _board[whiteRow, 1] = new BoardTile(whiteKnight1.coordinates, whiteKnight1);
            _board[whiteRow, 6] = new BoardTile(whiteKnight2.coordinates, whiteKnight2);
        }
        private static void putBishops()
        {
            int whiteRow = 7, blackRow = 0;

            Piece blackBishop1 = new Bishop(new Coordinates(blackRow, 2), PieceColor.black);
            Piece blackBishop2 = new Bishop(new Coordinates(blackRow, 5), PieceColor.black);

            _board[blackRow, 2] = new BoardTile(blackBishop1.coordinates, blackBishop1);
            _board[blackRow, 5] = new BoardTile(blackBishop2.coordinates, blackBishop2);


            Piece whiteBishop1 = new Bishop(new Coordinates(whiteRow, 2), PieceColor.white);
            Piece whiteBishop2 = new Bishop(new Coordinates(whiteRow, 5), PieceColor.white);

            _board[whiteRow, 2] = new BoardTile(whiteBishop1.coordinates, whiteBishop1);
            _board[whiteRow, 5] = new BoardTile(whiteBishop2.coordinates, whiteBishop2);
        }
        private static void putKings()
        {
            int whiteRow = 7, blackRow = 0;

            Piece blackKing = new King(new Coordinates(blackRow, 4), PieceColor.black);

            _board[blackRow, 4] = new BoardTile(blackKing.coordinates, blackKing);


            Piece whiteKing = new King(new Coordinates(whiteRow, 4), PieceColor.white);

            _board[whiteRow, 4] = new BoardTile(whiteKing.coordinates, whiteKing);
        }
        private static void putQueens()
        {
            int whiteRow = 7, blackRow = 0;

            Piece blackQueen = new Queen(new Coordinates(blackRow, 3), PieceColor.black);

            _board[blackRow, 3] = new BoardTile(blackQueen.coordinates, blackQueen);


            Piece whiteQueen = new Queen(new Coordinates(whiteRow, 3), PieceColor.white);

            _board[whiteRow, 3] = new BoardTile(whiteQueen.coordinates, whiteQueen);
        }
    }
}
