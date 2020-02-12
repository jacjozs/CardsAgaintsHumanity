using CAHServer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAHServer.Services
{
    public sealed class GameService
    {
        private static readonly GameService instance = new GameService();
        static GameService() { }
        private GameService() { }
        public static GameService Instance => instance;

        public Dictionary<string, Player> Players = new Dictionary<string, Player>();
        public string CzarPlayer;

    }
}
