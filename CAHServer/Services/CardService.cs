using CAHServer.Data;
using CAHServer.Data.Templates;
using CAHServer.Models;
using CAHServer.Packets;
using CAHServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAHServer.Services
{
    public class CardService
    {
        public static void StartCardDivision()
        {
            IEnumerable<Player> players = GameService.Instance.Players.Values;
            List<CardTemplate> cards;
            int id, count = DataStore.WhileCardsById.Count;
            foreach (Player player in players)
            {
                cards = new List<CardTemplate>();
                for (int i = 0; i < 10; i++)
                {
                    id = Rng.Next(1, count);
                    cards.Add(DataStore.WhileCardsById[id]);
                    player.WhileCards.Add(id);
                }
                PacketSendUtility.SendPack(player, new SM_PLAYER_CARD_INFO(cards, true));
            }
        }
        public static void SelectedCardsSend(Player Czar, int CzarCard)
        {
            IEnumerable<Player> players = GameService.Instance.Players.Values;
            CardTemplate czarCard = DataStore.BlackCardsById[CzarCard];
            List<CardTemplate> cards = new List<CardTemplate>();
            foreach (Player player in players)
            {
                foreach (int id in player.SelectCards)
                {
                    cards.Add(DataStore.WhileCardsById[id]);
                }
            }
            PacketSendUtility.SendPack(Czar, new SM_SELECTED_CARDS(cards, czarCard.CardCount));
        }
        public static void SelectedCardSend(int[] CzarCards)
        {
            IEnumerable<Player> players = GameService.Instance.Players.Values;
            List<CardTemplate> cards = new List<CardTemplate>();
            foreach (int id in CzarCards)
            {
                cards.Add(DataStore.BlackCardsById[id]);
            }
            PacketSendUtility.SendPackBroadcast(players, new SM_SELECTED_CARD(cards));
        }
    }
}
