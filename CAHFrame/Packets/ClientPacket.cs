using System;
using System.Collections.Generic;
using System.Text;

namespace CAHFrame.Packets
{
    public abstract class ClientPacket : Packet
    {
        public ClientPacket() : base(PacketType.CLIENT) { }
        public abstract void ProcessPacket();
        protected byte[] ReadBytes(int length)
        {
            byte[] result = new byte[length];
            Read(result, 0, length);
            return result;
        }

        protected new byte ReadByte()
        {
            return ReadBytes(1)[0];
        }

        protected int ReadInt()
        {
            return BitConverter.ToInt32(ReadBytes(4), 0);
        }

        protected float ReadFloat()
        {
            return BitConverter.ToSingle(ReadBytes(4), 0);
        }

        protected short ReadShort()
        {
            return BitConverter.ToInt16(ReadBytes(2), 0);
        }

        protected ushort ReadUnsignedShort()
        {
            return BitConverter.ToUInt16(ReadBytes(2), 0);
        }

        protected string ReadString()
        {
            string result = "";
            // loop the stream until end, will be broken in middle if string end found
            while ((Length - Position) > 2)
            {
                char c = BitConverter.ToChar(ReadBytes(2), 0);
                if (c == 0)
                    break;
                else
                    result += c;
            }
            return result;
        }
    }
}
