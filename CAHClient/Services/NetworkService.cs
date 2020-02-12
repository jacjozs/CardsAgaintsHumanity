using CAHClient.Packets;
using CAHFrame.Network;
using CAHFrame.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CAHClient.Services
{
    public class ServerConnection : Connection
    {
        public ServerConnection(TcpClient client) : base(client)
        {

        }

        public override void OnConnect()
        {
            base.OnConnect();
            // Send Init Packet.
            Log.Debug("New connection from " + GetRemoteIP());
            BeginWrite(new SM_PONG());
        }
    }
 
    public class NetworkService
    {
        static TcpClient Client;
        static ServerConnection ServerConnection;
        public static void Start()
        {
            CAHFrame.Packets.Packets.instance = new ClientServerPackets();
            CAHFrame.Packets.Packets.instance.Initialize();
            ServerConnection = new ServerConnection(null);
            Client = new TcpClient();
            Client.BeginConnect(IPAddress.Parse("127.0.0.1"), 2106, new AsyncCallback(StartConnect), Client);
        }
        static void StartConnect(IAsyncResult ar)
        {
            ServerConnection.SetClient((TcpClient)ar.AsyncState);
            ServerConnection.OnConnect();
        }
    }
}
