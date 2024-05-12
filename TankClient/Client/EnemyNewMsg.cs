using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace TankTank
{
    class EnemyNewMsg : Msg
    {
        private MsgType msgType = MsgType.EnemyNew;
        private Enemy c;
        private Controller ctrl;

        public EnemyNewMsg(Enemy c)
        {
            this.c = c;
        }

        public EnemyNewMsg(Controller ctrl)
        {
            this.ctrl = ctrl;
        }

        public void Send(UdpClient uc, string ip, int udpPort)
        {
            uc.Connect(ip, udpPort);

            var msgContent = (int)msgType + "|" + c.Name + "|" + c.X + "|" + c.Y + "|" + (int)c.Dir;
            var encodedMsg = Encoding.UTF32.GetBytes(msgContent);
            uc.Send(encodedMsg, encodedMsg.Length);
        }

        public void Parse(byte[] b)
        {
            var str = Encoding.UTF32.GetString(b);
            var strs = str.Split('|');
            var parsedName = strs[1];

            if (parsedName == ctrl.covid.Name)
                return;

            var parsedX = Convert.ToInt32(strs[2]);
            var parsedY = Convert.ToInt32(strs[3]);
            var parsedDir = (Direction)Convert.ToInt32(strs[4]);

            // Kiểm tra xem có tồn tại đối tượng với tên này hay không
            bool exists = ctrl.covids.Any(t => t.Name == parsedName);

            // Nếu không tồn tại, tạo một đối tượng Enemy mới và thêm vào danh sách
            if (!exists)
            {
                var newMsg = new EnemyNewMsg(ctrl.covid);
                ctrl.nc.Send(newMsg);

                var newEnemy = new Enemy(parsedName, parsedX, parsedY, parsedDir);
                ctrl.covids.Add(newEnemy);
            }
        }
    }
}
