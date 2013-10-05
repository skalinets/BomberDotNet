using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using Ninject;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit;
using WebSocket4Net;
using Xunit;
using FluentAssertions;
using Xunit.Extensions;
using Ninject.Extensions.Conventions;

namespace Bomberman.Api
{
    public class GameConnector: IGameConnector
    {
        private WebSocket websocket;

        public event EventHandler<NewPositionArgs> NewPosition = (sender, args) => { };
        public void NextMove(string nextMove)
        {
            websocket.Send(nextMove);
        }

        public void Connect()
        {
            var t = "codenjoy.com";
            const string address = "ws://10.42.211.51:8080/codenjoy-contest/ws";
            const string uri = address + "?user=thejoirc";
            websocket = new WebSocket(uri);

            
            websocket.MessageReceived += (sender, args) =>  
                {
                    var lastMessage = ToBoard(args.Message);
                    Console.Out.WriteLine("{0}", lastMessage); 
                    NewPosition(this, new NewPositionArgs(lastMessage));
                };
            InnerConnect().Wait();
        }

        private Task InnerConnect()
        {
            var tcs = new TaskCompletionSource<bool>();
            websocket.Opened += (sender, args) => tcs.SetResult(true);
            websocket.Error += (sender, args) => tcs.SetException(args.Exception);
            websocket.Open();
            return tcs.Task;
        }

        public string ToBoard(string lastMessage)
        {
            if (!lastMessage.StartsWith("board"))
            {
                return "<empty>";
            }
            var pos = Convert.ToInt32(Math.Sqrt(lastMessage.Length));
            var i = 1;
            var s = lastMessage.Substring("board=".Length);

            while (pos*i+i*2 < s.Length+1)
            {
                s = s.Insert(pos * i + i * 2-2, Environment.NewLine);
                i++;
            }

            return s;
        }
    }

    public class NewPositionArgs : EventArgs
    {
        public NewPositionArgs(string board)
        {
            this.Board = board;
        }

        public string Board { get; set; }
    }

    public class TestThis
    {
//        [Fact]
//        public void marker
        [Fact]
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

    public interface IGameConnector
    {
        void Connect();
        event EventHandler<NewPositionArgs> NewPosition;

        void NextMove(string nextMove);
    }

    public class NSubDataAttribute : AutoDataAttribute
    {
        public NSubDataAttribute() : base(new Fixture().Customize(new AutoNSubstituteCustomization()))
        {

        }
    }
     public class BootstrapperTests
     {
         [Fact]
         public void should_create_game()
         {
             new Bootstrapper().GetGame().Should().NotBeNull();

         }
     }

    public class DecisionMakerTests
    {
        [Theory, NSubDataAttribute]
        public void should_respond_in_allowed_way(DecisionMaker decisionMaker, string board)
        {
            decisionMaker.NextMove(board).Should().BeOneOf("up", "down", "left", "right", "act");
        }
    }

    public class Bootstrapper
    {
        public Game GetGame()
        {
            var kernel = new StandardKernel();
            kernel.Bind(x =>
                                      x.FromThisAssembly()
                                       .SelectAllClasses()
                                       .BindDefaultInterface());
            return kernel.Get<Game>();
        }
    }
}