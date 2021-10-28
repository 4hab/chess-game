using System;
using System.Collections.Generic;
using System.Text;

namespace chess
{
    class Coordinates
    {
        private int _x, _y;
        public int x() { return _x; }
        public int y() { return _y; }
        public Coordinates(int x, int y) { _x = x; _y = y; }

        public static bool operator ==(Coordinates c1, Coordinates c2)
        {
            return c1.x() == c2.x() && c1.y() == c2.y();
        }
        public static bool operator !=(Coordinates c1, Coordinates c2)
        {
            return c1.x() != c2.x() || c1.y() != c2.y();
        }
    }
}
