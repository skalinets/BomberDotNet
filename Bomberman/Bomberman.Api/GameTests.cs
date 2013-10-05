using System;
using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace Bomberman.Api
{
    public class GameTests
    {
        [Fact(Skip="integration stuff")]
        public  void doit()
        {
            var list = new List<string>();
            var gameConnector = new GameConnector();
            gameConnector.NewPosition += (sender, args) => list.Add(args.Board);
            gameConnector.Connect();
            Thread.Sleep(1000);
            list.Should().NotBeEmpty(); 
        }

        [Theory, NSubData]
        public void game_should_start_listening_to_server(
            [Frozen] IGameConnector gameConnector,
            Game game)
        {
            game.Start();

            gameConnector.Received().Connect();
        }

        [Theory, NSubData]
        public void TestGameStart(
            [Frozen] IGameConnector gameConnector, 
            [Frozen] IDecisionMaker maker, 
            string board, Moves nextMove, Game game)
        {
            maker.NextMove(board).Returns(nextMove);
            game.Start();
            
            gameConnector.NewPosition += Raise.EventWith(new Object(), new NewPositionArgs(board));

            gameConnector.Received().NextMove(nextMove.ToString());

        }
    }
}