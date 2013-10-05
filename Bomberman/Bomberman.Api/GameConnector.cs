using System;
using System.Threading.Tasks;
using WebSocket4Net;

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
}