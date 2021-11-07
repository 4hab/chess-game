using System;
using System.Collections.Generic;
using System.Text;

namespace chess
{
    class History
    {
        private static History _instance;
        public static History instance=> _instance = _instance==null? new History():_instance;

        private Stack<MoveRecord> records = new Stack<MoveRecord>();
        private Stack<MoveRecord> temp = new Stack<MoveRecord>();

        private History()
        {

        }

        public void clear()
        {
            records.Clear();
            temp.Clear();
        }
        public void add(Coordinates from,Coordinates to,Piece deadPiece=null)
        {
            records.Push(new MoveRecord(from, to, deadPiece));
            temp.Clear();
        }
        public MoveRecord unDo()
        {
            if (records.Count == 0)
                return null;
            MoveRecord mr = records.Pop();
            temp.Push(mr);
            return mr;
        }
        public MoveRecord reDo()
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
