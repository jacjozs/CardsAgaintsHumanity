using CAHLib.Packets;
using CAHServer.Data;
using CAHServer.Data.Templates;
using System.Collections.Generic;

namespace CAHServer.Packets
{
    public class SM_PONG : ServerPacket
    {
        public override void WritePacket()
        {
            WriteByte(1);
        }
    }

    public class SM_PLAYER_INFO : ServerPacket
    {
        private string Nickname;
        private bool NewRemov = true;//true = new; false = remove

        public SM_PLAYER_INFO(string nickname, bool newRemov)
        {
            Nickname = nickname;
            NewRemov = newRemov;
        }

        public override void WritePacket()
        {
            WriteString(Nickname);
            WriteBool(NewRemov);
        }
    }
    public class SM_PLAYER_CARD_INFO : ServerPacket
    {
        private List<CardTemplate> Cards;
        private bool WhileBlack = true;//true = while; false = black

        public SM_PLAYER_CARD_INFO(List<CardTemplate> cards, bool whileBlack)
        {
            Cards = cards;
            WhileBlack = whileBlack;
        }

        public override void WritePacket()
        {
            foreach (CardTemplate card in Cards)
            {
                WriteInt(card.ID);
                WriteString(card.Title);
            }
            WriteBool(WhileBlack);
        }
    }
    public class SM_CZAR_CARD : ServerPacket
    {
        private CardTemplate Card;

        public SM_CZAR_CARD(CardTemplate card)
        {
            Card = card;
        }

        public override void WritePacket()
        {
            WriteInt(Card.ID);
            WriteString(Card.Title);
            WriteShort(Card.CardCount);
        }
    }
    public class SM_SELECTED_CARDS : ServerPacket
    {
        private List<CardTemplate> Cards;
        private int CardCount;

        public SM_SELECTED_CARDS(List<CardTemplate> cards, int cardCount)
        {
            Cards = cards;
            CardCount = cardCount;
        }

        public override void WritePacket()
        {
            WriteInt(Cards.Count);
            WriteShort(CardCount);
            foreach (CardTemplate card in Cards)
            {
                WriteInt(card.ID);
                WriteString(card.Title);
            }
        }
    }
    public class SM_SELECTED_CARD : ServerPacket
    {
        private List<CardTemplate> Cards;

        public SM_SELECTED_CARD(List<CardTemplate> cards)
        {
            Cards = cards;
        }

        public override void WritePacket()
        {
            WriteShort(Cards.Count);
            foreach (CardTemplate card in Cards)
            {
                WriteInt(card.ID);
                WriteString(card.Title);
            }
        }
    }
}
