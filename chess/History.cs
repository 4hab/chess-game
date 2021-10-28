using System;
using System.Collections.Generic;
using System.Text;

namespace chess
{
    class History
    {
        private Stack<MoveRecord> records;
        private Stack<MoveRecord> temp;

        public History()
        {
            records = new Stack<MoveRecord>();
            temp = new Stack<MoveRecord>();
        }
        public void add(MoveRecord record)
        {
            records.Push(record);
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
