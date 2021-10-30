using System;
using System.Collections.Generic;
using System.Text;

namespace chess
{
    class ScoreBusinessLogic
    {
        public event Notify scoreUpdated;

        public void updateScore()
        {
            scoreUpdated.Invoke();
        }

    }
}
