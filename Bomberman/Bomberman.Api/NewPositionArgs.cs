using System;

namespace Bomberman.Api
{
    public class NewPositionArgs : EventArgs
    {
        public NewPositionArgs(string board)
        {
            this.Board = board;
        }

        public string Board { get; set; }
    }
}