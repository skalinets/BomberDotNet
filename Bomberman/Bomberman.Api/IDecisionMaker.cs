using System;

namespace Bomberman.Api
{
    public interface IDecisionMaker
    {
        Moves NextMove(string board);
    }

    public class DecisionMaker : IDecisionMaker
    {
        private string board;

        public Moves NextMove(string board)
        {
            this.board = board;
            Position p = board.GetBomber();

            if (CanMove(p.Row, p.Col - 1))
            {
                return Moves.Left;
            }
            if (CanMove(p.Row, p.Col + 1))
            {
                return Moves.Right;
            }
            if (CanMove(p.Row - 1, p.Col))
            {
                return Moves.Up;
            }
            if (CanMove(p.Row + 1, p.Col))
            {
                return Moves.Down;
            }

            string[] moves = Enum.GetNames(typeof (Moves));
            int index = new Random().Next(moves.Length);
            return (Moves) Enum.Parse(typeof (Moves), moves[index]);
        }

        private bool CanMove(int row, int col)
        {
            var item = board.ItemAt(row, col);
            return item != Items.Empty && item != Items.Wall;
        }
    }
}