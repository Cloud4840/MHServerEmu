﻿using MHServerEmu.Networking;

namespace MHServerEmu.GameServer.Games
{
    public enum EventEnum
    {
        StartThrowing,
        EndThrowing,
        StartTravel,
        EndTravel
    }

    public class GameEvent
    {
        private readonly DateTime _creationTime;
        private readonly TimeSpan _lifetime;

        public FrontendClient Client { get; }
        public EventEnum Event { get; }
        public bool IsRunning { get; set; }
        public ulong Data { get; set; }

        public GameEvent(FrontendClient client, EventEnum gameEvent, long lifetimeMs, ulong data)
        {
            _creationTime = DateTime.Now;
            _lifetime = TimeSpan.FromMilliseconds(lifetimeMs);

            Client = client;
            Event = gameEvent;
            Data = data;
            IsRunning = true;
        }

        public bool IsExpired()
        {
            return DateTime.Now.Subtract(_creationTime) >= _lifetime;
        }
    }
}
