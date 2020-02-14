using CAHServer.Data;
using CAHServer.Models;
using CAHServer.Packets;
using CAHServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CAHServer.Services
{
    public sealed class GameService
    {
        private static readonly GameService instance = new GameService();
        static GameService() { }
        private GameService() { }
        public static GameService Instance => instance;

        public Dictionary<string, Player> Players = new Dictionary<string, Player>();
        public int SelectedCardCount;
        private string CzarPlayer;
        private int CzarCard;
        public int[] SelectCzarCard;
        public bool ConnectionEnabled = true;
        /// <summary>
        /// Játékos belépés
        /// </summary>
        /// <param name="player"></param>
        public void OnPlayer(Player player)
        {
            Players.Add(player.Nickname, player);
            PacketSendUtility.SendPackBroadcast(player, new SM_PLAYER_INFO(player.Nickname, true));
        }
        /// <summary>
        /// Játékos kilépés
        /// </summary>
        /// <param name="player"></param>
        public void OffPlayer(Player player)
        {
            PacketSendUtility.SendPackBroadcast(player, new SM_PLAYER_INFO(player.Nickname, false), false);
            Players.Remove(player.Nickname);
        }
        /// <summary>
        /// Játék indítás
        /// </summary>
        /// <param name="player">Indító játékos</param>
        public void StartGame(Player player)
        {
            ConnectionEnabled = false;
            player.IsCzar = true;
            CzarPlayer = player.Nickname;
            CardService.StartCardDivision();
            for (int i = 0; i < 10; i++)
            {
                CzarCard = Rng.Next(1, DataStore.BlackCardsById.Count);
                PacketSendUtility.SendPackBroadcast(this.Players.Values, new SM_CZAR_CARD(DataStore.BlackCardsById[CzarCard]));
                foreach (Player playerr in this.Players.Values)
                {
                    playerr.SelectCards = new List<int>();
                }
                SelectedCardCount = 0;
                do
                {
                    Thread.Sleep(200);
                } while (SelectedCardCount != Players.Count - 1);
                SelectedCardCount = 0;
                CardService.SelectedCardsSend(Players[CzarPlayer], CzarCard);
                do
                {
                    Thread.Sleep(200);
                } while (SelectedCardCount != 1);
                CardService.SelectedCardSend()
            }
        }
    }
}
