using CAHLib.Utils;
using CAHServer.Data.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace CAHServer.Data
{
    public class DataStore
    {
        public static Dictionary<int, CardTemplate> WhileCardsById = new Dictionary<int, CardTemplate>();
        public static Dictionary<int, CardTemplate> BlackCardsById = new Dictionary<int, CardTemplate>();

        public static void Initialize()
        {
            Log.Info("===== Loading Data Store ... =====");
            long start = Utils.DateTimeToTimestamp(DateTime.Now);

            DeserializeXML("card_template.xml", typeof(CardTemplates));

            long time = Utils.DateTimeToTimestamp(DateTime.Now) - start;
            Log.Info("===== Loaded Data Store in " + time + " seconds. =====\n");
        }

        static void DeserializeXML(string filename, Type destinationType)
        {
            XmlSerializer ser = new XmlSerializer(destinationType);
            CardTemplates file = (CardTemplates)ser.Deserialize(new StreamReader("Data\\Store\\" + filename));
            int loaded = file.GetSize();
            file.PostDeserialize();
            Log.Info("-> Loaded " + loaded + " " + destinationType.Name);
        }

        [XmlRoot(ElementName = "card_templates")]
        public class CardTemplates
        {
            [XmlElement(ElementName = "card_template")]
            public List<CardTemplate> Templates;

            public int GetSize()
            {
                return Templates.Count;
            }

            public void PostDeserialize()
            {
                foreach (CardTemplate template in Templates)
                {
                    switch (template.Type)
                    {   
                        case CardType.BLACK:
                            DataStore.BlackCardsById.Add(template.ID, template);
                            break;
                        case CardType.WHILE:
                            DataStore.WhileCardsById.Add(template.ID, template);
                            break;
                        default:
                            Log.Warn($"Unknow card type! template id: {template.ID}\n");
                            break;
                    }
                }
                Templates.Clear();
            }
        }
    }
}
