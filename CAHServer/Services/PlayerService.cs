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
            GameService.Instance.OnPlayer(connection.Player);
        }

        public static void OffPlayer(Player player)
        {
            GameService.Instance.OffPlayer(player);
        }
    }
}
