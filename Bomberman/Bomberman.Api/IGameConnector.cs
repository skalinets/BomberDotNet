using System;

namespace Bomberman.Api
{
    public interface IGameConnector
    {
        void Connect();
        event EventHandler<NewPositionArgs> NewPosition;
        void NextMove(string nextMove);
    }
}