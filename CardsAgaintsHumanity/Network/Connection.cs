using CAHLib.Packets;
using CAHLib.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CAHLib.Network
{
    public abstract class Connection
    {
        protected TcpClient Tcp;
        protected byte[] ReceiveBuffer;
        private MemoryStream ReceiveStream;
        private Queue<ServerPacket> PacketSendQueue = new Queue<ServerPacket>();
        private bool WriteInterestEnabled = false;
        protected bool Ready = false;

        protected Connection(TcpClient client)
        {
            this.Tcp = client;
            ReceiveBuffer = new byte[8192];
            ReceiveStream = new MemoryStream(8192 * 4);
        }

        public void SetClient(TcpClient client)
        {
            this.Tcp = client;
        }

        public string GetRemoteIP()
        {
            return ((IPEndPoint)Tcp.Client.RemoteEndPoint).Address.ToString();
        }

        public void Close()
        {
            Log.Debug("ServerClose " + GetRemoteIP());
            OnDisconnect();
            Tcp.Close();
        }

        public virtual void OnConnect()
        {
            BeginRead();
        }

        void BeginRead()
        {
            if (Tcp != null && Tcp.Client != null && Tcp.Connected)
            {
                try
                {
                    Tcp.Client.BeginReceive(ReceiveBuffer, 0, 8192, SocketFlags.None, new AsyncCallback(EndRead), ReceiveBuffer);
                }
                catch (Exception)
                {
                    Close();
                }
            }
        }

        public virtual void OnDisconnect()
        {

        }

        bool ReadInterestEnabled = false;

        void EnableReadInterest()
        {
            if (ReadInterestEnabled)
                return;
            lock (ReceiveStream)
            {
                ReadInterestEnabled = true;

                // Read First Packet Size then go back at initial position.
                ushort size = BitConverter.ToUInt16(new byte[] { (byte)ReceiveStream.ReadByte(), (byte)ReceiveStream.ReadByte() }, 0);
                ReceiveStream.Position -= 2;

                while (size > 0 && size <= (ReceiveStream.Length - ReceiveStream.Position))
                {
                    //Log.Debug("Read: " + size + "-byte packet.");

                    // Read Packet Data including length
                    byte[] data = new byte[size];
                    ReceiveStream.Read(data, 0, size);

                    // Process packet.
                    MemoryStream stream = new MemoryStream(data, 2, data.Length - 2, false);

                    byte opcode = (byte)stream.ReadByte();
                    Type packetType = Packets.Packets.instance.GetClientPacketType(opcode);

                    if (packetType == null)
                    {
                        Log.Warn("Unknown packet received from AION client, opcode: " + string.Format("{0:X}", opcode) + " - size: " + (size - 2));
                    }
                    else
                    {
                        ClientPacket pkt = (ClientPacket)Activator.CreateInstance(packetType);
                        pkt.Write(stream.ToArray(), (int)stream.Position, (int)(stream.Length - stream.Position));
                        pkt.Position = 0;
                        stream.Close();

                        Log.Debug("PktRecv " + GetRemoteIP() + ": " + pkt.GetType().Name + "(" + pkt.Length + ")");
                        pkt.Connection = this;
                        pkt.Opcode = opcode;

                        try
                        {
                            pkt.ProcessPacket();
                        }
                        catch (Exception e)
                        {
                            Log.Error("Cannot process client packet!", e);
                        }

                        pkt.Close();
                    }

                    if (ReceiveStream.Length - ReceiveStream.Position > 2)
                    {
                        size = BitConverter.ToUInt16(new byte[] { (byte)ReceiveStream.ReadByte(), (byte)ReceiveStream.ReadByte() }, 0);
                        ReceiveStream.Position -= 2;
                    }
                    else
                        size = 0;
                }

                int remaining = (int)(ReceiveStream.Length - ReceiveStream.Position);
                if (remaining > 0)
                {
                    Log.Debug("Remaining Bytes : " + remaining);
                    // Read Latest Bytes
                    byte[] remdata = new byte[remaining];
                    ReceiveStream.Read(remdata, 0, remaining);
                    ReceiveStream.Position = 0;
                    ReceiveStream.Write(remdata, 0, remdata.Length);
                }
                else
                    ReceiveStream.Position = 0;

                ReadInterestEnabled = false;
            }
        }

        void EndRead(IAsyncResult result)
        {
            lock (ReceiveBuffer)
            {
                try
                {
                    int received = 0;

                    try
                    {
                        received = Tcp.Client.EndReceive(result);
                    }
                    catch (Exception)
                    {
                        OnDisconnect();
                        return;
                    }

                    if (received <= 0)
                    {
                        OnDisconnect();
                        return;
                    }

                    if (!Ready)
                    {
                        Log.Warn("Received a " + received + "-byte packet while connection not ready to accept packets.");
                        return;
                    }

                    // This is the full set of data.
                    // May contain several packets.
                    byte[] data = (byte[])result.AsyncState;

                    lock (ReceiveStream)
                    {
                        ReceiveStream.Write(data, 0, received);
                    }

                    if (!ReadInterestEnabled)
                    {
                        ReceiveStream.SetLength(ReceiveStream.Position);
                        ReceiveStream.Position = 0;
                        EnableReadInterest();
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Error while processing client data!", e);
                }

                BeginRead();
            }
        }

        public void SendPacket(ServerPacket packet)
        {
            lock (PacketSendQueue)
            {
                PacketSendQueue.Enqueue(packet);
                if (!WriteInterestEnabled)
                    EnableWriteInterest();
            }
        }

        public void SendPacket(ServerPacket packet, bool closeAfterPacket)
        {
            SendPacket(packet);
            if (closeAfterPacket)
                SendPacket(null);
        }

        void EnableWriteInterest()
        {
            WriteInterestEnabled = true;
            while (PacketSendQueue.Count > 0)
            {
                ServerPacket pkt = PacketSendQueue.Dequeue();
                if (pkt == null)
                {
                    Close();
                    return;
                }
                BeginWrite(pkt);
            }
            WriteInterestEnabled = false;
        }

        protected void BeginWrite(ServerPacket packet)
        {
            if (packet == null)
                return;
            try
            {
                // Here, packet is ready to be sent
                packet.Connection = this;
                packet.Position = 0;

                if (!Packets.Packets.instance.HasServerOpcode(packet.GetType()))
                {
                    Log.Warn("No opcode defined for " + packet.GetType().Name);
                    return;
                }

                byte opcode = Packets.Packets.instance.GetServerPacketOpcode(packet.GetType());

                packet.Opcode = opcode;

                packet.WriteShort(0); // Future length

                    packet.WriteByte(opcode); // Opcode

                packet.WritePacket(); // Write contents of packet

                int length = (int)packet.Position;

                // Encrypt packet
                byte[] result;

                    result = new byte[length * 4];

                Array.Copy(packet.ToArray(), 0, result, 0, length);


                packet.Position = 0;
                packet.WriteShort((short)length);
                packet.Write(result, 2, length - 2);

                // Send in socket
                Tcp.Client.BeginSend(packet.ToArray(), 0, length, SocketFlags.None, new AsyncCallback(EndWrite), packet);

                if (!Ready)
                    Ready = true;

            }
            catch (Exception e)
            {
                Log.Error("Error while writing to data sink!", e);
            }
        }

        void EndWrite(IAsyncResult result)
        {
            if (Tcp != null && Tcp.Client != null && Tcp.Client.Connected)
            {
                int sent = Tcp.Client.EndSend(result);
                ServerPacket pkt = (ServerPacket)result.AsyncState;
                Log.Debug("PktSend " + GetRemoteIP() + ": " + pkt.GetType().Name + " (" + pkt.Length + ")");
            }
        }
    }
}
