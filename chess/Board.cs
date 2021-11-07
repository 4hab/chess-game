using System;
using System.Collections.Generic;
using System.Text;

namespace chess
{
    class Board
    {
        private BoardTile[,] _board;

        private static Board _instance;
        public static Board instance => _instance = _instance == null ? new Board() : _instance;

        private Board()
        {
            _board = new BoardTile[8, 8];
            _init();

        }
        public static void reset()
        {
            _instance = new Board();
        }
        public BoardTile of(Coordinates c) => _board[c.x, c.y];
        public BoardTile of(int x,int y) => _board[x,y];

        private BoardTile _selectedTile = null;

        public BoardTile selectedTile => _selectedTile;

        public void select(BoardTile tile)
        {
            _selectedTile = tile;
        }


        public void move(Coordinates c1, Coordinates c2)
        {
            //add to history
            History.instance.add(c1, c2, instance.of(c2).piece);
            //change piece coordinates firs
            instance.of(c1).piece.unmarkAvailableCells();
            instance.of(c1).piece.moveTo(c2);
            //change score
            if (!instance.of(c2).isEmpty())
                GameObserver.instance.otherPlayer.minusScore(instance.of(c2).piece.val);
            //move pieces
            instance.of(c2).setPiece(instance.of(c1).piece);
            instance.of(c1).setPiece(null);
            select(null);
            _afterMove();
        }
        public void unDoMove()
        {
            if (instance.selectedTile != null)
            {
                instance.selectedTile.piece.unmarkAvailableCells();
                instance.select(null);
            }
            MoveRecord record = History.instance.unDo();
            if (record == null)
                return;
            Coordinates c1 = record.destination, c2 = record.source;
            GameObserver.instance.switchPlayer();
            instance.of(c1).piece.moveTo(c2);
            GameObserver.instance.switchPlayer();
            if (record.deadPiece != null)
                GameObserver.instance.currentPlayer.minusScore(record.deadPiece.val * -1);
            instance.of(c2).setPiece(instance.of(c1).piece);
            instance.of(c1).setPiece(record.deadPiece);
            select(null);
            _afterMove();
        }
        public void reDoMove()
        {
            if (instance.selectedTile != null)
            {
                instance.selectedTile.piece.unmarkAvailableCells();
                instance.select(null);
            }
            MoveRecord record = History.instance.reDo();
            if (record == null)
                return;

            Coordinates c1 = record.source, c2 = record.destination;
            GameObserver.instance.switchPlayer();
            instance.of(c1).piece.moveTo(c2);
            GameObserver.instance.switchPlayer();
            if (instance.of(c1).piece != null)
            {
                GameObserver.instance.currentPlayer.minusScore(instance.of(c1).piece.val);
            }
            instance.of(c2).setPiece(instance.of(c1).piece);
            instance.of(c1).setPiece(null);
            select(null);
            _afterMove();
        }
        public bool isSafeMove(Coordinates c1, Coordinates c2)
        {
            Board board = Board.instance;
            //save original situation
            Piece p1 = board.of(c1).piece;
            Piece p2 = board.of(c2).piece;
            bool initialDangerStatus = GameObserver.instance.currentPlayer.isAttacked;
            GameObserver.instance.currentPlayer.inDanger(false);

            //move pieces virtually
            board.of(c2).setPiece(p1, true);
            board.of(c1).setPiece(null, true);
            //let other player attacks [virtually] to see if the situation is safe or not
            GameObserver.instance.switchPlayer();
            GameObserver.instance.currentPlayer.attack(virtually: true);
            GameObserver.instance.switchPlayer();
            bool currentDangerStatus = GameObserver.instance.currentPlayer.isAttacked;

            //return to the original situation
            board.of(c2).setPiece(p2);
            board.of(c1).setPiece(p1);
            GameObserver.instance.currentPlayer.inDanger(initialDangerStatus);

            //if my king is attacked => false(not safe move) otherwise => true(safe move)
            return !currentDangerStatus;
        }
        private void _afterMove()
        {
            //mark danger squares
            GameObserver.instance.currentPlayer.attack();
            GameObserver.instance.currentPlayer.inDanger(false);
            //switch player
            GameObserver.instance.switchPlayer();
            GameObserver.instance.isGameOver();
        }
        private void _init()
        {
            _selectedTile = null;
            History.instance.clear();
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
        private void putPawns()
        {
            for (int column = 0; column < 8; column++)
            {
                Piece blackPawn = new Pawn(new Coordinates(1, column), PieceColor.black);
                Piece whitePawn = new Pawn(new Coordinates(6, column), PieceColor.white);

                _board[1, column] = new BoardTile(blackPawn.coordinates, blackPawn);
                _board[6, column] = new BoardTile(whitePawn.coordinates, whitePawn);

            }
        }
        private void putRooks()
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
        private void putKnights()
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
        private void putBishops()
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
        private void putKings()
        {
            int whiteRow = 7, blackRow = 0;

            Piece blackKing = new King(new Coordinates(blackRow, 4), PieceColor.black);

            _board[blackRow, 4] = new BoardTile(blackKing.coordinates, blackKing);


            Piece whiteKing = new King(new Coordinates(whiteRow, 4), PieceColor.white);

            _board[whiteRow, 4] = new BoardTile(whiteKing.coordinates, whiteKing);
        }
        private void putQueens()
        {
            int whiteRow = 7, blackRow = 0;

            Piece blackQueen = new Queen(new Coordinates(blackRow, 3), PieceColor.black);

            _board[blackRow, 3] = new BoardTile(blackQueen.coordinates, blackQueen);


            Piece whiteQueen = new Queen(new Coordinates(whiteRow, 3), PieceColor.white);

            _board[whiteRow, 3] = new BoardTile(whiteQueen.coordinates, whiteQueen);
        }
    }
}
