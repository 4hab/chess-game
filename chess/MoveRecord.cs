using System;
using System.Collections.Generic;
using System.Text;

namespace chess
{
    class MoveRecord
    {
        private Coordinates _source, _destination;
        Piece _killedPiece;

        public Piece killedPiece => _killedPiece;
        public Coordinates source => _source;
        public Coordinates destination => _destination;

        public MoveRecord(Coordinates from, Coordinates to, Piece killedPiece = null)
        {
            _source = from;
            _destination = to;
            _killedPiece = killedPiece;
        }

    }
}
