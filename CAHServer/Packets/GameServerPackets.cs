using System;
using System.Collections.Generic;
using System.Text;

namespace CAHServer.Packets
{
    public class GameServerPackets : CAHLib.Packets.Packets
    {
        public override void Initialize()
        {
            ClientPacketsOpcs.Add(0x01, typeof(CM_PING));
            ClientPacketsOpcs.Add(0x02, typeof(CM_LOGIN));

            ServerPacketsOpcs.Add(typeof(SM_PONG), 0x01);
            ServerPacketsOpcs.Add(typeof(SM_PLAYER_INFO), 0x02);
        }
    }
}
