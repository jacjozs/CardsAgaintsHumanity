using CAHLib.Network;
using CAHLib.Utils;
using CAHServer.Models;
using CAHServer.Packets;
using System;
using System.Net;
using System.Net.Sockets;

namespace CAHServer.Services
{
    public class GameConnection : Connection
    {
        public Player Player;
        public GameConnection(TcpClient client) : base(client)
        {

        }

        public override void OnConnect()
        {
            base.OnConnect();
            // Send Init Packet.
            Log.Debug("New connection from " + GetRemoteIP());
            BeginWrite(new SM_PONG());
        }

        public override void OnDisconnect()
        {
            base.OnDisconnect();
            PlayerService.OffPlayer(Player);
        }
    }

    public class GameClientListener : TcpListener
    {
        public GameClientListener() : base(IPAddress.Parse("0.0.0.0"), 2106)
        {

        }
    }
    public class NetworkService
    {
        static GameClientListener Listener;
        public static void Start()
        {
            CAHLib.Packets.Packets.instance = new GameServerPackets();
            CAHLib.Packets.Packets.instance.Initialize();

            Listener = new GameClientListener();
            Listener.Start();
            BeginAccept();
        }

        static GameConnection CnxPreAlloc;

        static void BeginAccept()
        {
            CnxPreAlloc = new GameConnection(null);
            Listener.BeginAcceptTcpClient(new AsyncCallback(EndAccept), Listener);
        }

        static void EndAccept(IAsyncResult ar)
        {
            TcpListener lsn = (TcpListener)ar.AsyncState;
            TcpClient client = lsn.EndAcceptTcpClient(ar);
            CnxPreAlloc.SetClient(client);
            CnxPreAlloc.OnConnect();

            BeginAccept();
        }
    }
}
