using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace CAHServer.Data.Templates
{
    public enum CardType
    {
        BLACK, WHILE
    }

    [XmlRoot("card_template")]
    public class CardTemplate
    {
        [XmlAttribute("id")]
        public int ID;
        [XmlAttribute("type")]
        public CardType Type;
        [XmlAttribute("title")]
        public string Title;
        [XmlAttribute("card_count")]
        public int CardCount;
    }
}
