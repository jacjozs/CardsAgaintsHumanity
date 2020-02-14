using CAHLib.Packets;
using CAHServer.Services;
using System.Collections.Generic;

namespace CAHServer.Models
{
    public class Player
    {
        /// <summary>
        /// Fehér kártyák id-e
        /// </summary>
        private List<int> whileCards;
        /// <summary>
        /// Cár e a játékos
        /// </summary>
        private bool isCzar;
        /// <summary>
        /// Kiválasztott fehér kártyák id-e
        /// </summary>
        private List<int> selectCards;
        /// <summary>
        /// Becenév
        /// </summary>
        private readonly string nickname;
        /// <summary>
        /// Kliens kapcsolat
        /// </summary>
        private GameConnection connection;

        public List<int> WhileCards { get => whileCards; set => whileCards = value; }
        public List<int> SelectCards { get => selectCards; set => selectCards = value; }
        public bool IsCzar { get => isCzar; set => isCzar = value; }
        public string Nickname => nickname;
        public GameConnection Connection { get => connection; set => connection = value; }

        public Player(GameConnection connection, string nickname)
        {
            this.whileCards = new List<int>();
            this.isCzar = false;
            this.nickname = nickname;
            this.connection = connection;
        }
        public void SendPacket(ServerPacket pkt)
        {
            Connection.SendPacket(pkt);
        }
    }
}
