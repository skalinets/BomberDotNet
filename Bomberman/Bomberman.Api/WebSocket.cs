using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ploeh.AutoFixture.Xunit;
using WebSocket4Net;
using Xunit;
using FluentAssertions;
using Xunit.Extensions;

namespace Bomberman.Api
{
    public class GameConnector
    {
        private WebSocket websocket;

        public EventHandler<NewPositionArgs> NewPosition = (sender, args) => { };

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
            websocket.Send("left");
            Thread.Sleep(1000);
            websocket.Send("right");
            Thread.Sleep(1000);
            websocket.Send("up");
            Thread.Sleep(1000);   
            websocket.Send("down");
            Thread.Sleep(1000);
            websocket.Send("act");
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

    public class NewPositionArgs
    {
        public NewPositionArgs(string board)
        {
            this.Board = board;
        }

        public string Board { get; set; }
    }

    public class TestThis
    {
        [Theory, AutoData]
        public  void doit(string s)
        {
            var list = new List<string>();
            var game = new GameConnector();
            game.NewPosition += (sender, args) => list.Add(args.Board);
            game.Connect();
            Thread.Sleep(1000);
            list.Should().NotBeEmpty(); 
        }

        [Fact]
        public void TestGameStart()
        {
            
        }
    }
}