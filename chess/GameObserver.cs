using System;
using System.Collections.Generic;
using System.Text;

namespace chess
{
    public delegate void Notify();
    class GameObserver
    {

        public static BoardTile selectedTile = null;
        public static Notify scoreUpdated;
        public static Notify check;

        public static int getScore()
        {
            return 0;
        }



    }


}
