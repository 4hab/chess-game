using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace chess
{
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
            return multiStepsMoving(dx, dy, countOnly);
        }
        public override void attack()
        {
            multiStepsAttack(dx, dy);
        }
    }
}
