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
            GameConnection gc = (GameConnection)Connection;
            PlayerService.OnPlayer(ref gc, ReadString());
        }
    }
}
