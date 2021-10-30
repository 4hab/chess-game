using System;
using System.Collections.Generic;
using System.Text;

namespace chess
{
    class MoveRecord
    {
        private Coordinates _source, _destination;
        Piece _deadPiece;

        public Piece deadPiece => _deadPiece;
        public Coordinates source => _source;
        public Coordinates destination => _destination;

        public MoveRecord(Coordinates from, Coordinates to, Piece deadPiece = null)
        {
            _source = from;
            _destination = to;
            _deadPiece = deadPiece;
        }

    }
}
