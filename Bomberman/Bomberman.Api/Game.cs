using System;

namespace Bomberman.Api
{
    public class Game
    {
        private readonly IGameConnector gameConnector;
        private readonly IDecisionMaker decisionMaker;

        public Game(IGameConnector gameConnector, IDecisionMaker decisionMaker)
        {
            this.gameConnector = gameConnector;
            this.decisionMaker = decisionMaker;
        }

        public void Start()
        {
            gameConnector.Connect();
            gameConnector.NewPosition += (sender, args) =>
                {
                    var nextMove = decisionMaker.NextMove(args.Board);
                    Console.Out.WriteLine("nextMove = {0}", nextMove);
                    gameConnector.NextMove(nextMove);
                };
        }
    }
}