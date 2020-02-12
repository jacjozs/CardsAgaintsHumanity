using System;
using System.Collections.Generic;
using System.Text;

namespace CAHFrame.Packets
{
    public abstract class Packets
    {
        protected Dictionary<byte, Type> ClientPacketsOpcs = new Dictionary<byte, Type>();
        protected Dictionary<Type, byte> ServerPacketsOpcs = new Dictionary<Type, byte>();

        public static List<int> HighPacketsOpcs = new List<int>();

        public static Packets instance;
        public abstract void Initialize();

        public Type GetClientPacketType(byte opcode)
        {
            Type result;
            ClientPacketsOpcs.TryGetValue(opcode, out result);
            return result;
        }

        public bool HasServerOpcode(Type type)
        {
            return ServerPacketsOpcs.ContainsKey(type);
        }

        public byte GetServerPacketOpcode(Type type)
        {
            byte result;
            ServerPacketsOpcs.TryGetValue(type, out result);
            return result;
        }
    }
}
