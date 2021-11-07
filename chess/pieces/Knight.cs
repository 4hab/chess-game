using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace chess
{
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
            Board.instance.of(coordinates).switchMark(countOnly);
            for (int i = 0; i < 8; i++)
            {
                Coordinates c = new Coordinates(coordinates.x + dx[i], coordinates.y + dy[i]);
                if (inRange(c))
                {
                    BoardTile tile = Board.instance.of(c);
                    if (tile.isEmpty() && Board.instance.isSafeMove(coordinates, c))
                    {
                        tile.switchMark(countOnly);
                        ret++;
                    }
                    else if (!tile.isEmpty() && tile.piece.isEnemy() && Board.instance.isSafeMove(coordinates, c))
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
                    BoardTile tile = Board.instance.of(c);
                    tile.markDanger(true);
                }
            }
        }
    }
}
