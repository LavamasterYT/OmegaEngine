using System;
using DiscordRPC;
using DiscordRPC.Logging;

namespace OmegaEngine
{
    public class DiscordRPC : IDisposable
    {
        private DiscordRpcClient _client;

        public DiscordRPC()
        {
            _client = new DiscordRpcClient("780976341993390120");
			_client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };
			_client.Initialize();
        }

        public void Invoke()
        {

        }

        public void SetPresence()
        {
            _client.SetPresence(new RichPresence()
            {
                State = "Playing Frogger",
                Details = "What's this?",
                Party = new Party() { ID = "11", Max = 8, Size = 1 }
            });
        }

        public void Dispose()
        {
            _client.Dispose();

            GC.SuppressFinalize(this);
        }

        ~DiscordRPC()
        {
            Dispose();
        }
    }
}