using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace TankTank
{
    class BloodNewMsg : Msg
    {
        private readonly MsgType _msgType = MsgType.HealNew;
        private readonly Blood _orange;
        private readonly Controller _ctrl;

        // Constructor to send packets using an Blood object
        public BloodNewMsg(Blood o)
        {
            _orange = o;
        }

        // Constructor to initialize with a Controller object
        public BloodNewMsg(Controller ctrl)
        {
            _ctrl = ctrl;
        }

        // Sends a message using UdpClient
        public void Send(UdpClient udpClient, string ipAddress, int udpPort)
        {
            udpClient.Connect(ipAddress, udpPort);
            string messageContent = $"{(int)_msgType}|{_orange.Name}|{_orange.X}|{_orange.Y}";
            byte[] messageBytes = Encoding.UTF32.GetBytes(messageContent);
            udpClient.Send(messageBytes, messageBytes.Length);
        }

        // Parses the received message
        public void Parse(byte[] messageBytes)
        {
            string receivedMessage = Encoding.UTF32.GetString(messageBytes);
            string[] messageParts = receivedMessage.Split('|');
            string receivedName = messageParts[1];

            // Check if the Blood already exists in the controller list
            if (_ctrl.orange.Any(existingBlood => existingBlood.Name == receivedName))
            {
                return;
            }

            // Parse the X and Y coordinates
            int xCoordinate = Convert.ToInt32(messageParts[2]);
            int yCoordinate = Convert.ToInt32(messageParts[3]);

            // Create a new Blood and add it to the controller list
            Blood newBlood = new Blood(receivedName, xCoordinate, yCoordinate);
            _ctrl.orange.Add(newBlood);
        }
    }
}