using System;
using System.Collections.Generic;
using System.Text;

namespace CAHClient.Packets
{
    public class ClientServerPackets : CAHFrame.Packets.Packets
    {
        public override void Initialize()
        {
            ClientPacketsOpcs.Add(0x01, typeof(CM_PING));

            ServerPacketsOpcs.Add(typeof(SM_PONG), 0x01);
        }
    }
}
