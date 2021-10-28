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
        protected Colors _color;
        protected Image _image;

        public Piece(Coordinates coordinates, Colors color)
        {
            _coordinates = coordinates;
            _color = color;
        }
        public Colors color => _color;
        public Image image => _image;

        public string key => _type + (color == Colors.white ? "_white" : "_black");

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

        public virtual void moveTo(Coordinates coordinates)
        {
            _coordinates = coordinates;
        }
        protected bool inRange(int x, int y)
        {
            return x >= 0 && x < 8 && y >= 0 && y < 8;
        }
        protected void multiStepsMoving(int[] dx, int[] dy)
        {

            for (int i = 0; i < dx.Length; i++) 
            {
                int x = coordinates.x() + dx[i];
                int y = coordinates.y() + dy[i];
                while (true)
                {
                    BoardTile[,] board = GameObserver.board;
                    if (inRange(x, y) && board[x, y].isEmpty())
                    {
                        board[x, y].switchMark();
                    }
                    else if (inRange(x, y) && GameObserver.isEnemy(board[x, y]))
                    {
                        board[x, y].switchMark();
                        break;
                    }
                    else
                        break;
                    x += dx[i];
                    y += dy[i];
                }
            }
        }
    }
    class Pawn : Piece
    {
        private bool _firstMove = true;

        public bool firstMove => _firstMove;
        public Pawn(Coordinates coordinates, Colors color) : base(coordinates, color)
        {
            _type = "pawn";
            _val = 1;
            _image = Image.FromFile(color == Colors.black ? Images.blackPawn : Images.whitePawn);
        }

        public override void moveTo(Coordinates coordinates)
        {
            _firstMove = false;
            base.moveTo(coordinates);
        }
        protected override void lookForAvailableCells()
        {
            int dx = color == Colors.white ? -1 : 1;
            int x = _coordinates.x(), y = _coordinates.y();
            BoardTile[,] board = GameObserver.board;
            if (inRange(x + dx, y) && board[x + dx, y].isEmpty())
            {
                board[x + dx, y].switchMark();
            }
            //attacking
            if (inRange(x + dx, y + 1) && !board[x + dx, y + 1].isEmpty() && GameObserver.isEnemy(board[x + dx, y + 1]))
            {
                board[x + dx, y + 1].switchMark();
            }
            if (inRange(x + dx, y - 1) && !board[x + dx, y - 1].isEmpty() && GameObserver.isEnemy(board[x + dx, y - 1]))
            {
                board[x + dx, y - 1].switchMark();
            }
            if (this.firstMove && board[x + dx, y].isEmpty())
            {
                dx *= 2;
                board[x + dx, y] = board[x + dx, y];
                if (board[x + dx, y].isEmpty())
                {
                    board[x + dx, y].switchMark();
                }
            }
        }
    }
    class Rook : Piece
    {
        private int[] dx = { 0, 0, 1, -1 };
        private int[] dy = { 1, -1, 0, 0 };
        public Rook(Coordinates coordinates, Colors color) : base(coordinates, color)
        {
            _type = "rook";
            _val = 5;
            _image = Image.FromFile(color == Colors.black ? Images.blackRook : Images.whiteRook);
        }
        protected override void lookForAvailableCells()
        {
            multiStepsMoving(dx, dy);
        }
    }
    class Knight : Piece
    {
        private int[] dx = { 1, 1, -1, -1, 2, 2, -2, -2 };
        private int[] dy = { 2, -2, 2, -2, 1, -1, 1, -1 };
        public Knight(Coordinates coordinates, Colors color) : base(coordinates, color)
        {
            _type = "knight";
            _val = 3;
            _image = Image.FromFile(color == Colors.black ? Images.blackKnight : Images.whiteKnight);
        }

        protected override void lookForAvailableCells()
        {
            for (int i = 0; i < 8; i++)
            {
                int x = coordinates.x() + dx[i], y = coordinates.y() + dy[i];
                BoardTile[,] board = GameObserver.board;
                if (inRange(x, y) && board[x, y].isEmpty())
                {
                    board[x, y].switchMark();
                }
                else if (inRange(x, y) && GameObserver.isEnemy(board[x, y]))
                {
                    board[x, y].switchMark();
                }
            }
        }
    }
    class Bishop : Piece
    {
        private int[] dx = { 1, 1, -1, -1 };
        private int[] dy = { 1, -1, 1, -1 };
        public Bishop(Coordinates coordinates, Colors color) : base(coordinates, color)
        {
            _type = "bishop";
            _val = 3;
            _image = Image.FromFile(color == Colors.black ? Images.blackBishop : Images.whiteBishop);
        }
        protected override void lookForAvailableCells()
        {
            multiStepsMoving(dx, dy);
        }
    }
    class King : Piece
    {
        private int[] dx = { 0, 0, 1, 1, 1, -1, -1, -1 };
        private int[] dy = { 1, -1, 1, -1, 0, 1, -1, 0 };
        public King(Coordinates coordinates, Colors color) : base(coordinates, color)
        {
            _type = "king";
            _val = 1;
            _image = Image.FromFile(color == Colors.black ? Images.blackKing : Images.whiteKing);
        }
        protected override void lookForAvailableCells()
        {
            for (int i = 0; i < 8; i++)
            {
                int x = coordinates.x() + dx[i], y = coordinates.y() + dy[i];
                BoardTile[,] board = GameObserver.board;
                if (inRange(x, y) && board[x, y].isEmpty())
                {
                    board[x, y].switchMark();
                }
                else if (inRange(x, y) && GameObserver.isEnemy(board[x, y]))
                {
                    board[x, y].switchMark();
                }
            }
        }
    }
    class Queen : Piece
    {
        private int[] dx = { 0, 0, 1, 1, 1, - 1, -1, -1 };
        private int[] dy = { 1, -1, 1, -1, 0, 1, -1, 0 };
        public Queen(Coordinates coordinates, Colors color) : base(coordinates, color)
        {
            _type = "queen";
            _val = 9;
            _image = Image.FromFile(color == Colors.black ? Images.blackQueen : Images.whiteQueen);
        }
        protected override void lookForAvailableCells()
        {
            multiStepsMoving(dx, dy);
        }
    }

}
