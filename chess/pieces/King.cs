﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace chess
{
    class King : Piece
    {
        private int[] dx = { 0, 0, 1, 1, 1, -1, -1, -1 };
        private int[] dy = { 1, -1, 1, -1, 0, 1, -1, 0 };

        public override void moveTo(Coordinates c)
        {
            GameObserver.instance.currentPlayer.moveKing(c);
            base.moveTo(c);
        }
        public King(Coordinates coordinates, PieceColor color) : base(coordinates, color)
        {
            _type = "king";
            _val = 1;
            _image = Image.FromFile(color == PieceColor.black ? Images.blackKing : Images.whiteKing);
        }
        protected override int lookForAvailableCells(bool countOnly)
        {
            Board board = Board.instance;
            int ret = 0;
            board.of(coordinates).switchMark(countOnly);
            for (int i = 0; i < 8; i++)
            {
                Coordinates c = new Coordinates(coordinates.x + dx[i], coordinates.y + dy[i]);
                if (inRange(c))
                {
                    BoardTile tile = board.of(c);
                    if (tile.isEmpty() && !tile.danger && Board.instance.isSafeMove(coordinates, c))
                    {
                        tile.switchMark(countOnly);
                        ret++;
                    }
                    else if (!tile.isEmpty() && tile.piece.isEnemy() && !tile.danger && Board.instance.isSafeMove(coordinates, c))
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
