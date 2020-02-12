using CAHServer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAHServer.Services
{
    public class PlayerService
    {
        public static void OnPlayer(ref GameConnection connection, string nickname)
        {
            connection.Player = new Player(connection, nickname);
            GameService.Instance.Players.Add(nickname, connection.Player);
        }
    }
}
