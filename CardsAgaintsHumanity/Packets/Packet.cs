using CAHLib.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CAHLib.Packets
{
    public enum PacketType
    {
        CLIENT, SERVER
    }
    public abstract class Packet : MemoryStream
    {
        public PacketType Type;
        public Connection Connection;
        public byte Opcode;

        public Packet(PacketType direction)
        {
            Type = direction;
        }
    }
}
