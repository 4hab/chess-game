﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace chess
{
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
