using CAHLib.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAHServer.Packets
{
    public class SM_PONG : ServerPacket
    {
        public override void WritePacket()
        {
            WriteByte(1);
        }
    }
}
