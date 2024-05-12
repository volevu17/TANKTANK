using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace TankTank
{
    class TankMoveMsg : Msg
    {
        private MsgType msgType = MsgType.TankMove;
        private string tankName;
        private int posX, posY;
        private Direction movementDirection;
        private Controller gameController;

        public TankMoveMsg(string tankName, int posX, int posY, Direction movementDirection)
        {
            this.tankName = tankName;
            this.posX = posX;
            this.posY = posY;
            this.movementDirection = movementDirection;
        }

        public TankMoveMsg(Controller gameController)
        {
            this.gameController = gameController;
        }

        public void Send(UdpClient udpClient, string ipAddress, int portNumber)
        {
            udpClient.Connect(ipAddress, portNumber);
            string message = $"{(int)msgType}|{tankName}|{posX}|{posY}|{(int)movementDirection}";
            byte[] messageBytes = Encoding.UTF32.GetBytes(message);
            udpClient.Send(messageBytes, messageBytes.Length);
        }

        public void Parse(byte[] byteArray)
        {
            string decodedMessage = Encoding.UTF32.GetString(byteArray);
            string[] messageParts = decodedMessage.Split('|');
            string incomingTankName = messageParts[1];

            if (incomingTankName == gameController.myTank.Name)
            {
                return;
            }

            int incomingPosX = Convert.ToInt32(messageParts[2]);
            int incomingPosY = Convert.ToInt32(messageParts[3]);
            Direction incomingDirection = (Direction)Convert.ToInt32(messageParts[4]);

            foreach (Tank tank in gameController.tanks)
            {
                if (tank.Name == incomingTankName)
                {
                    tank.Dir = incomingDirection;
                    tank.X = incomingPosX;
                    tank.Y = incomingPosY;
                    break;
                }
            }
        }
    }
}