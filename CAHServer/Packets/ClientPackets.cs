using CAHLib.Packets;
using CAHServer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAHServer.Packets
{
    public class CM_PING : ClientPacket
    {
        public override void ProcessPacket()
        {
            Connection.SendPacket(new SM_PONG());
        }
    }

    public class CM_LOGIN : ClientPacket
    {
        public override void ProcessPacket()
        {
            if(GameService.Instance.ConnectionEnabled)
            {
                GameConnection gc = (GameConnection)Connection;
                PlayerService.OnPlayer(ref gc, ReadString());
            }
        }
    }

    public class CM_START_GAME : ClientPacket
    {
        public override void ProcessPacket()
        {
            GameConnection gc = (GameConnection)Connection;
            GameService.Instance.StartGame(gc.Player);
        }
    }
    public class CM_SELECT_CARDS : ClientPacket
    {
        public override void ProcessPacket()
        {
            GameConnection gc = (GameConnection)Connection;
            short count = ReadShort();
            for (int i = 0; i < count; i++)
            {
                gc.Player.SelectCards.Add(ReadInt());
            }
            GameService.Instance.SelectedCardCount++;
        }
    }
    public class CM_SELECT_CZAR_CARD : ClientPacket
    {
        public override void ProcessPacket()
        {
            GameConnection gc = (GameConnection)Connection;
            int count = ReadShort();
            GameService.Instance.SelectCzarCard = new int[ReadInt()];
            for (int i = 0; i < count; i++)
            {

            }
            GameService.Instance.SelectedCardCount++;
        }
    }
}
