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
    public class TestThis
    {
        [Fact(Skip="integration stuff")]
        public  void doit()
        {
            var list = new List<string>();
            var game = new GameConnector();
            game.NewPosition += (sender, args) => list.Add(args.Board);
            game.Connect();
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
            string board, string nextMove, Game game)
        {
            maker.NextMove(board).Returns(nextMove);
            game.Start();
            
            gameConnector.NewPosition += Raise.EventWith(new Object(), new NewPositionArgs(board));

            gameConnector.Received().NextMove(nextMove);

        }
    }
}