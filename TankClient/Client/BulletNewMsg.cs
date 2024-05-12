using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace TankTank
{
    class BulletNewMsg : Msg
    {
        private readonly MsgType msgType = MsgType.BulletNew;
        private Bullet bullet;
        private Controller ctrl;

        // Constructor used for sending packets
        public BulletNewMsg(Bullet bullet)
        {
            this.bullet = bullet;
        }

        public BulletNewMsg(Controller ctrl)
        {
            this.ctrl = ctrl;
        }

        public void Send(UdpClient udpClient, string ipAddress, int port)
        {
            udpClient.Connect(ipAddress, port);
            // Use "|" as the separator for the sent content
            string message = $"{(int)msgType}|{bullet.FromName}|{bullet.Id}|{bullet.X}|{bullet.Y}|{(int)bullet.Dir}";
            byte[] messageBytes = Encoding.UTF32.GetBytes(message);
            udpClient.Send(messageBytes, messageBytes.Length);
        }

        public void Parse(byte[] data)
        {
            string message = Encoding.UTF32.GetString(data);
            string[] messageParts = message.Split('|');

            string fromName = messageParts[1];
            int bulletId = Convert.ToInt32(messageParts[2]);

            // Ignore if the bullet is from the current player's tank
            if (fromName == ctrl.myTank.Name)
                return;

            // Check for duplicate bullets and ignore if already present
            foreach (Bullet tmpBullet in ctrl.bullets)
            {
                if (fromName == tmpBullet.FromName && bulletId == tmpBullet.Id)
                    return;
            }

            int posX = Convert.ToInt32(messageParts[3]);
            int posY = Convert.ToInt32(messageParts[4]);
            Direction direction = (Direction)Convert.ToInt32(messageParts[5]);

            Bullet newBullet = new Bullet(fromName, bulletId, posX, posY, direction);
            ctrl.bullets.Add(newBullet);
        }
    }
}