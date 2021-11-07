using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace chess
{
    class Pawn : Piece
    {
        private Coordinates initialCoordinates;
        public bool firstMove => coordinates == initialCoordinates;
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
            Board.instance.of(coordinates).switchMark(countOnly);
            return _attack(countOnly) + _move(countOnly);
        }

        public override void attack()
        {
            Coordinates c1 = new Coordinates(coordinates.x + dx, coordinates.y + 1);
            Coordinates c2 = new Coordinates(coordinates.x + dx, coordinates.y - 1);
            if (inRange(c1))
            {
                BoardTile tile = Board.instance.of(c1);
                tile.markDanger(true);
            }
            if (inRange(c2))
            {
                BoardTile tile = Board.instance.of(c2);
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
                BoardTile tile = Board.instance.of(c1);
                if (!tile.isEmpty() && tile.piece.isEnemy() && Board.instance.isSafeMove(coordinates, c1))
                {
                    tile.switchMark(countOnly);
                    ret++;
                }
            }
            if (inRange(c2))
            {
                BoardTile tile = Board.instance.of(c2);
                if (!tile.isEmpty() && tile.piece.isEnemy() && Board.instance.isSafeMove(coordinates, c2))
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
            if (inRange(c1))
            {
                BoardTile tile = Board.instance.of(c1);
                if (tile.isEmpty() && Board.instance.isSafeMove(coordinates, c1))
                {
                    //mark c1
                    tile.switchMark(countOnly);
                    ret++;
                    tile = Board.instance.of(c2);
                    if (firstMove && tile.isEmpty() && Board.instance.isSafeMove(coordinates, c2))
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
}
