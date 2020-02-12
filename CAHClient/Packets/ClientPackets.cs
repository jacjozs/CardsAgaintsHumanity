using CAHFrame.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAHClient.Packets
{
    public class CM_PING : ClientPacket
    {
        public override void ProcessPacket()
        {
            Connection.SendPacket(new SM_PONG());
        }
    }
}
