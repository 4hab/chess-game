using System;
using System.Collections.Generic;
using System.Text;

namespace chess
{
    class History
    {
        private static Stack<MoveRecord> records = new Stack<MoveRecord>();
        private static Stack<MoveRecord> temp = new Stack<MoveRecord>();

        public static void add(Coordinates from,Coordinates to,Piece deadPiece=null)
        {
            records.Push(new MoveRecord(from, to, deadPiece));
            temp.Clear();
        }
        public static MoveRecord unDo()
        {
            if (records.Count == 0)
                return null;
            MoveRecord mr = records.Pop();
            temp.Push(mr);
            return mr;
        }
        public static MoveRecord reDo()
        {
            if (temp.Count == 0)
            {
                return null;
            }
            MoveRecord mr = temp.Pop();
            records.Push(mr);
            return mr;
        }
    }

}
