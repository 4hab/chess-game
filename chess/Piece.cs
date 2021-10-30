using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace chess
{
    class Piece
    {
        protected Coordinates _coordinates;
        protected string _type;
        protected int _val;
        protected PieceColor _color;
        protected Image _image;

        public int val => _val;
        public string type => _type;
        public Piece(Coordinates coordinates, PieceColor color)
        {
            _coordinates = coordinates;
            _color = color;
        }
        public PieceColor color => _color;
        public Image image => _image;

        public Coordinates coordinates => _coordinates;
        public virtual void markAvailableCells() 
        {
            lookForAvailableCells();
        }
        public virtual void unmarkAvailableCells()
        {
            lookForAvailableCells();
        }
        protected virtual void lookForAvailableCells() { }
        public void moveTo(Coordinates coordinates)
        {
            _coordinates = coordinates;
        }

        public bool isEnemy()
        {
            return color != Player.currentPlayer.color;
        }
        protected bool inRange(int x, int y)
        {
            return x >= 0 && x < 8 && y >= 0 && y < 8;
        }
        protected bool inRange(Coordinates coordinates)
        {
            return coordinates.x >= 0 && coordinates.x < 8 && coordinates.y >= 0 && coordinates.y < 8;
        }
        protected void multiStepsMoving(int[] dx, int[] dy)
        {
            Board.of(coordinates).switchMark();
            for (int i = 0; i < dx.Length; i++)
            {
                Coordinates c = new Coordinates(coordinates.x + dx[i], coordinates.y + dy[i]);
                while (true)
                {
                    if (inRange(c))
                    {
                        BoardTile tile = Board.of(c);
                        if (tile.isEmpty())
                        {
                            if(Board.isSafeMove(coordinates, c))
                                tile.switchMark();
                        }
                        else if (tile.piece.isEnemy())
                        {
                            if(Board.isSafeMove(coordinates, c))
                                tile.switchMark();
                            break;
                        }
                        else break;
                        c = new Coordinates(c.x + dx[i], c.y + dy[i]);
                    }
                    else break;
                }
            }
        }
        protected void multiStepsAttack(int[] dx, int[] dy)
        {
            for (int i = 0; i < dx.Length; i++)
            {
                Coordinates c = new Coordinates(coordinates.x + dx[i], coordinates.y + dy[i]);
                while (true)
                {
                    if (inRange(c))
                    {
                        BoardTile tile = Board.of(c);
                        if (tile.isEmpty())
                        {
                            tile.markDanger(true);
                        }
                        else
                        {
                            tile.markDanger(true);
                            break;
                        }
                        c = new Coordinates(c.x + dx[i], c.y + dy[i]);
                    }
                    else break;
                }
            }
        }

        public virtual void attack()
        {

        }
    }
    class Pawn : Piece
    {
        private Coordinates initialCoordinates;
        public bool firstMove => coordinates==initialCoordinates;
        public Pawn(Coordinates coordinates, PieceColor color) : base(coordinates, color)
        {
            _type = "pawn";
            _val = 1;
            _image = Image.FromFile(color == PieceColor.black ? Images.blackPawn : Images.whitePawn);
            initialCoordinates = coordinates;
        }

        private int dx => color == PieceColor.white ? -1 : 1;
        protected override void lookForAvailableCells()
        {
            Board.of(coordinates).switchMark();
            _attack();
            _move();
        }

        public override void attack()
        {
            Coordinates c1 = new Coordinates(coordinates.x + dx, coordinates.y + 1);
            Coordinates c2 = new Coordinates(coordinates.x + dx, coordinates.y - 1);
            if (inRange(c1))
            {
                BoardTile tile = Board.of(c1);
                tile.markDanger(true);
            }
            if (inRange(c2))
            {
                BoardTile tile = Board.of(c2);
                tile.markDanger(true);
            }
        }
        private void _attack()
        {
            Coordinates c1 = new Coordinates(coordinates.x + dx, coordinates.y + 1);
            Coordinates c2 = new Coordinates(coordinates.x + dx, coordinates.y - 1);
            if (inRange(c1))
            {
                BoardTile tile = Board.of(c1);
                if (!tile.isEmpty() && tile.piece.isEnemy() && Board.isSafeMove(coordinates, c1))
                {
                    tile.switchMark();
                }
            }
            if (inRange(c2))
            {
                BoardTile tile = Board.of(c2);
                if (!tile.isEmpty() && tile.piece.isEnemy() && Board.isSafeMove(coordinates, c2))
                {
                    tile.switchMark();
                }
            }
        }
        private void _move()
        {
            Coordinates c1 = new Coordinates(coordinates.x + dx, coordinates.y);
            Coordinates c2 = new Coordinates(coordinates.x + dx + dx, coordinates.y);
            if(inRange(c1))
            {
                BoardTile tile = Board.of(c1);
                if (tile.isEmpty() && Board.isSafeMove(coordinates, c1)) 
                {
                    //mark c1
                    tile.switchMark();
                    tile = Board.of(c2);
                    if (firstMove && tile.isEmpty() && Board.isSafeMove(coordinates, c2)) 
                    {
                        //mark c2
                        tile.switchMark();
                    }
                }
            }
        }
    }
    class Rook : Piece
    {
        private int[] dx = { 0, 0, 1, -1 };
        private int[] dy = { 1, -1, 0, 0 };
        public Rook(Coordinates coordinates, PieceColor color) : base(coordinates, color)
        {
            _type = "rook";
            _val = 5;
            _image = Image.FromFile(color == PieceColor.black ? Images.blackRook : Images.whiteRook);
        }
        protected override void lookForAvailableCells()
        {
            multiStepsMoving(dx, dy);
        }
        public override void attack()
        {
            multiStepsAttack(dx,dy);
        }
    }
    class Knight : Piece
    {
        private int[] dx = { 1, 1, -1, -1, 2, 2, -2, -2 };
        private int[] dy = { 2, -2, 2, -2, 1, -1, 1, -1 };
        public Knight(Coordinates coordinates, PieceColor color) : base(coordinates, color)
        {
            _type = "knight";
            _val = 3;
            _image = Image.FromFile(color == PieceColor.black ? Images.blackKnight : Images.whiteKnight);
        }

        protected override void lookForAvailableCells()
        {
            Board.of(coordinates).switchMark();
            for (int i = 0; i < 8; i++)
            {
                Coordinates c = new Coordinates(coordinates.x + dx[i], coordinates.y + dy[i]);
                if (inRange(c))
                {
                    BoardTile tile = Board.of(c);
                    if (tile.isEmpty() && Board.isSafeMove(coordinates, c))
                    {
                        tile.switchMark();
                    }
                    else if (!tile.isEmpty() && tile.piece.isEnemy() && Board.isSafeMove(coordinates, c)) 
                    {
                        tile.switchMark();
                    }
                }
            }
        }

        public override void attack()
        {
            for (int i = 0; i < 8; i++)
            {
                Coordinates c = new Coordinates(coordinates.x + dx[i], coordinates.y + dy[i]);
                if (inRange(c))
                {
                    BoardTile tile = Board.of(c);
                    tile.markDanger(true);
                }
            }
        }
    }
    class Bishop : Piece
    {
        private int[] dx = { 1, 1, -1, -1 };
        private int[] dy = { 1, -1, 1, -1 };
        public Bishop(Coordinates coordinates, PieceColor color) : base(coordinates, color)
        {
            _type = "bishop";
            _val = 3;
            _image = Image.FromFile(color == PieceColor.black ? Images.blackBishop : Images.whiteBishop);
        }
        protected override void lookForAvailableCells()
        {
            multiStepsMoving(dx, dy);
        }
        public override void attack()
        {
            multiStepsAttack(dx, dy);
        }
    }
    class King : Piece
    {
        private int[] dx = { 0, 0, 1, 1, 1, -1, -1, -1 };
        private int[] dy = { 1, -1, 1, -1, 0, 1, -1, 0 };
        public King(Coordinates coordinates, PieceColor color) : base(coordinates, color)
        {
            _type = "king";
            _val = 1;
            _image = Image.FromFile(color == PieceColor.black ? Images.blackKing : Images.whiteKing);
        }
        protected override void lookForAvailableCells()
        {
            Board.of(coordinates).switchMark();
            for (int i = 0; i < 8; i++)
            {
                Coordinates c = new Coordinates(coordinates.x + dx[i], coordinates.y + dy[i]);
                if (inRange(c))
                {
                    BoardTile tile = Board.of(c);
                    if (tile.isEmpty() && !tile.danger && Board.isSafeMove(coordinates, c))
                    {
                        tile.switchMark();
                    }
                    else if (!tile.isEmpty() && tile.piece.isEnemy() && !tile.danger && Board.isSafeMove(coordinates, c)) 
                    {
                        tile.switchMark();
                    }

                }
            }
        }

        public override void attack()
        {
            for (int i = 0; i < 8; i++)
            {
                Coordinates c = new Coordinates(coordinates.x + dx[i], coordinates.y + dy[i]);
                if (inRange(c))
                {
                    BoardTile tile = Board.of(c);
                    tile.markDanger(true);
                }
            }
        }
    }
    class Queen : Piece
    {
        private int[] dx = { 0, 0, 1, 1, 1, - 1, -1, -1 };
        private int[] dy = { 1, -1, 1, -1, 0, 1, -1, 0 };
        public Queen(Coordinates coordinates, PieceColor color) : base(coordinates, color)
        {
            _type = "queen";
            _val = 9;
            _image = Image.FromFile(color == PieceColor.black ? Images.blackQueen : Images.whiteQueen);
        }
        protected override void lookForAvailableCells()
        {
            multiStepsMoving(dx, dy);
        }
        public override void attack()
        {
            multiStepsAttack(dx, dy);
        }
    }

}
