using CAHLib.Packets;
using CAHServer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAHServer.Services
{
    public class PacketSendUtility
    {
        public static void SendPack(Player player, ServerPacket serverPacket)
        {
            player.Connection.SendPacket(serverPacket);
        }
        public static void SendPackBroadcast(Player player, ServerPacket serverPacket)
        {
            SendPackBroadcast(player, serverPacket, true);
        }
        public static void SendPackBroadcast(Player player, ServerPacket serverPacket, bool Self)
        {
            IEnumerable<Player> players = GameService.Instance.Players.Values;
            foreach (Player playerr in players)
            {
                if (!Self && playerr.Nickname == player.Nickname) continue;
                playerr.Connection.SendPacket(serverPacket);
            }
        }
        public static void SendPackBroadcast(IEnumerable<Player> players, ServerPacket serverPacket)
        {
            foreach (Player player in players)
            {
                player.Connection.SendPacket(serverPacket);
            }
        }
    }
}
