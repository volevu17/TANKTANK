using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Threading;
using System.Diagnostics;


namespace TankTank
{
    public class NetClient
    {
        private readonly int udpPort;
        private readonly UdpClient senderUdpClient;
        private readonly UdpClient receiverUdpClient;
        private readonly Random randomizer = new Random();
        private readonly Controller controller;
        private string serverIp;

        public NetClient(Controller controller)
        {
            udpPort = randomizer.Next(9000, 10000);
            receiverUdpClient = new UdpClient(udpPort);
            senderUdpClient = new UdpClient();
            this.controller = controller;
        }

        public bool Connect(string serverIp, int serverPort)
        {
            this.serverIp = serverIp;

            using (var client = new TcpClient())
            {
                client.Connect(serverIp, serverPort);
                using (var networkStream = client.GetStream())
                using (var writer = new BinaryWriter(networkStream))
                using (var reader = new BinaryReader(networkStream))
                {
                    writer.Write(udpPort);
                    writer.Flush();

                    string mapData = reader.ReadString();
                    LoadMap(mapData);
                }
            }

            Send(new TankNewMsg(controller.myTank));
            Send(new EnemyNewMsg(controller.covid));

            var udpThread = new Thread(ReceiveUdpMessages) { IsBackground = true };
            udpThread.Start();

            return true;
        }

        public void Send(Msg message)
        {
            message.Send(senderUdpClient, serverIp, 7777);
        }

        private void ReceiveUdpMessages()
        {
            while (true)
            {
                IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] receivedBytes = receiverUdpClient.Receive(ref remoteEndpoint);
                ParseMessage(receivedBytes);
            }
        }

        private void ParseMessage(byte[] data)
        {
            var messageContent = Encoding.UTF32.GetString(data);
            var messageParts = messageContent.Split('|');
            var messageType = (MsgType)Convert.ToInt32(messageParts[0]);

            Msg message = null;
            switch (messageType)
            {
                case MsgType.TankNew:
                    message = new TankNewMsg(controller);
                    break;
                case MsgType.TankMove:
                    message = new TankMoveMsg(controller);
                    break;
                case MsgType.BulletNew:
                    message = new BulletNewMsg(controller);
                    break;
                case MsgType.EnemyNew:
                    message = new EnemyNewMsg(controller);
                    break;
                case MsgType.EnemyMove:
                    message = new EnemyMoveMsg(controller);
                    break;
                case MsgType.HealNew:
                    message = new BloodNewMsg(controller);
                    break;
                default:
                    break;
            }

            if (message != null)
            {
                message.Parse(data);
            }
        }


        private void LoadMap(string mapData)
        {
            var mapRows = mapData.Split('|');
            for (int i = 0; i < mapRows.Length; i++)
            {
                ParseMapRow(mapRows[i], i);
            }
        }

        private void ParseMapRow(string rowData, int rowIndex)
        {
            try
            {
                var rowValues = rowData.Split('n');
                for (int colIndex = 0; colIndex < rowValues.Length; colIndex++)
                {
                    controller.lineMap[rowIndex, colIndex] = Convert.ToInt32(rowValues[colIndex]);
                }
            }
            catch
            {
                // Handle exceptions silently to avoid crashes due to map parsing errors.
            }
        }
    }
}