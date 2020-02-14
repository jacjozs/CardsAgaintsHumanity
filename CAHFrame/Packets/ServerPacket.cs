using System;
using System.Collections.Generic;
using System.Text;

namespace CAHFrame.Packets
{
    public abstract class ServerPacket : Packet
    {
        public ServerPacket() : base(PacketType.SERVER) { }
        public abstract void WritePacket();
        public void WriteBytes(byte[] bytes)
        {
            Write(bytes, 0, bytes.Length);
        }

        public void WriteInt(int value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        public void WriteShort(short value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        public void WriteUnsignedShort(ushort value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        public void WriteLong(long value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        public void WriteFloat(float value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        public void WriteString(string str)
        {
            foreach (char c in str)
            {
                byte[] tmp = BitConverter.GetBytes(c);
                WriteBytes(tmp);
            }
            WriteShort(0);
        }
        public void WriteBool(bool value)
        {
            WriteByte((byte)(value ? 1 : 0));
        }
    }
}
