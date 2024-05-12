using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace TankTank
{
    class EnemyMoveMsg : Msg
    {
        private MsgType msgType = MsgType.EnemyMove;
        private string name;
        private int x;
        private int y;
        private Direction dir;
        private Controller ctrl;

        public EnemyMoveMsg(string name, int x, int y, Direction dir)
        {
            this.name = name;
            this.dir = dir;
            this.x = x;
            this.y = y;
        }

        public EnemyMoveMsg(Controller ctrl)
        {
            this.ctrl = ctrl;
        }

        public void Send(UdpClient uc, string ip, int udpPort)
        {
            uc.Connect(ip, udpPort);

            var msgContent = (int)msgType + "|" + name + "|" + x + "|" + y + "|" + (int)dir;
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

            foreach (var t in ctrl.covids)
            {
                if (t.Name == parsedName)
                {
                    t.Dir = parsedDir;
                    t.X = parsedX;
                    t.Y = parsedY;
                    break;
                }
            }
        }
    }
}
