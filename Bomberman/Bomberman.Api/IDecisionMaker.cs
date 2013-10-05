using System;

namespace Bomberman.Api
{
    public interface IDecisionMaker
    {
        string NextMove(string board);
    }

    public class DecisionMaker : IDecisionMaker
    {
        public string NextMove(string board)
        {
            var moves = new[]{"up", "right", "left", "down"};
            var index = new Random().Next(moves.Length);
            return moves[index];
        }
    }
}