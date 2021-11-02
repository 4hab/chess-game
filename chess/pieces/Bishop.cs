using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace chess
{
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
            return multiStepsMoving(dx, dy, countOnly);
        }
        public override void attack()
        {
            multiStepsAttack(dx, dy);
        }
    }
}
