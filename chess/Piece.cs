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
        public virtual int markAvailableCells() 
        {
            return lookForAvailableCells(false);
        }
        public virtual int unmarkAvailableCells()
        {
            return lookForAvailableCells(false);
        }
        protected virtual int lookForAvailableCells(bool countOnly) { return 0; }
        public void moveTo(Coordinates coordinates)
        {
            _coordinates = coordinates;
        }

        public int countAvailableTiles()
        {
            return lookForAvailableCells(true);
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
        protected int multiStepsMoving(int[] dx, int[] dy,bool countOnly)
        {
            int ret = 0;
            Board.of(coordinates).switchMark(countOnly);
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
                            if (GameObserver.isSafeMove(coordinates, c))
                            {
                                tile.switchMark(countOnly);
                                ret++;
                            }
                        }
                        else if (tile.piece.isEnemy())
                        {
                            if(GameObserver.isSafeMove(coordinates, c))
                            {
                                tile.switchMark(countOnly);
                                ret++;
                            }
                            break;
                        }
                        else break;
                        c = new Coordinates(c.x + dx[i], c.y + dy[i]);
                    }
                    else break;
                }
            }
            return ret;
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
        protected override int lookForAvailableCells(bool countOnly)
        {
            Board.of(coordinates).switchMark(countOnly);
            return _attack(countOnly)+_move(countOnly);
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
        private int _attack(bool countOnly)
        {
            int ret = 0;
            Coordinates c1 = new Coordinates(coordinates.x + dx, coordinates.y + 1);
            Coordinates c2 = new Coordinates(coordinates.x + dx, coordinates.y - 1);
            if (inRange(c1))
            {
                BoardTile tile = Board.of(c1);
                if (!tile.isEmpty() && tile.piece.isEnemy() && GameObserver.isSafeMove(coordinates, c1))
                {
                    tile.switchMark(countOnly);
                    ret++;
                }
            }
            if (inRange(c2))
            {
                BoardTile tile = Board.of(c2);
                if (!tile.isEmpty() && tile.piece.isEnemy() && GameObserver.isSafeMove(coordinates, c2))
                {
                    tile.switchMark(countOnly);
                    ret++;
                }
            }
            return ret;
        }
        private int _move(bool countOnly)
        {
            int ret = 0;
            Coordinates c1 = new Coordinates(coordinates.x + dx, coordinates.y);
            Coordinates c2 = new Coordinates(coordinates.x + dx + dx, coordinates.y);
            if(inRange(c1))
            {
                BoardTile tile = Board.of(c1);
                if (tile.isEmpty() && GameObserver.isSafeMove(coordinates, c1)) 
                {
                    //mark c1
                    tile.switchMark(countOnly);
                    ret++;
                    tile = Board.of(c2);
                    if (firstMove && tile.isEmpty() && GameObserver.isSafeMove(coordinates, c2)) 
                    {
                        //mark c2
                        tile.switchMark(countOnly);
                        ret++;
                    }
                }
            }
            return ret;
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
        protected override int lookForAvailableCells(bool countOnly)
        {
            return multiStepsMoving(dx, dy,countOnly);
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

        protected override int lookForAvailableCells(bool countOnly)
        {
            int ret = 0;
            Board.of(coordinates).switchMark(countOnly);
            for (int i = 0; i < 8; i++)
            {
                Coordinates c = new Coordinates(coordinates.x + dx[i], coordinates.y + dy[i]);
                if (inRange(c))
                {
                    BoardTile tile = Board.of(c);
                    if (tile.isEmpty() && GameObserver.isSafeMove(coordinates, c))
                    {
                        tile.switchMark(countOnly);
                        ret++;
                    }
                    else if (!tile.isEmpty() && tile.piece.isEnemy() && GameObserver.isSafeMove(coordinates, c)) 
                    {
                        tile.switchMark(countOnly);
                        ret++;
                    }
                }
            }
            return ret;
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
        protected override int lookForAvailableCells(bool countOnly)
        {
            return multiStepsMoving(dx, dy,countOnly);
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
        protected override int lookForAvailableCells(bool countOnly)
        {
            int ret = 0;
            Board.of(coordinates).switchMark(countOnly);
            for (int i = 0; i < 8; i++)
            {
                Coordinates c = new Coordinates(coordinates.x + dx[i], coordinates.y + dy[i]);
                if (inRange(c))
                {
                    BoardTile tile = Board.of(c);
                    if (tile.isEmpty() && !tile.danger && GameObserver.isSafeMove(coordinates, c))
                    {
                        tile.switchMark(countOnly);
                        ret++;
                    }
                    else if (!tile.isEmpty() && tile.piece.isEnemy() && !tile.danger && GameObserver.isSafeMove(coordinates, c)) 
                    {
                        tile.switchMark(countOnly);
                        ret++;
                    }

                }
            }
            return ret;
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
        protected override int lookForAvailableCells(bool countOnly)
        {
            return multiStepsMoving(dx, dy, countOnly);
        }
        public override void attack()
        {
            multiStepsAttack(dx, dy);
        }
    }

}
