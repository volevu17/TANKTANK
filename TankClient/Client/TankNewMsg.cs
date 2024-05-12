using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace TankTank
{
    class TankNewMsg : Msg
    {
        private readonly MsgType msgType = MsgType.TankNew;
        private readonly Tank tank;
        private readonly Controller ctrl;

        // Constructor đầu tiên nhận vào một đối tượng Tank
        public TankNewMsg(Tank tank)
        {
            this.tank = tank;
        }

        // Constructor thứ hai nhận vào đối tượng Controller
        public TankNewMsg(Controller ctrl)
        {
            this.ctrl = ctrl;
        }

        // Gửi thông tin qua UDP
        public void Send(UdpClient udpClient, string ipAddress, int udpPort)
        {
            udpClient.Connect(ipAddress, udpPort);

            // Tạo chuỗi chứa thông tin cần gửi
            string message = $"{(int)msgType}|{tank.Name}|{tank.X}|{tank.Y}|{(int)tank.Dir}|{tank.Color[0]}|{tank.Color[1]}|{tank.Color[2]}";

            // Chuyển đổi chuỗi thành byte và gửi qua UDP
            byte[] data = Encoding.UTF32.GetBytes(message);
            udpClient.Send(data, data.Length);
        }

        // Phân tích thông tin nhận được từ dữ liệu byte
        public void Parse(byte[] data)
        {
            string message = Encoding.UTF32.GetString(data);
            string[] parts = message.Split('|');
            string receivedName = parts[1];

            // Bỏ qua nếu tên tank nhận được trùng với tên tank của chính mình
            if (receivedName == ctrl.myTank.Name)
                return;

            // Trích xuất các giá trị từ thông tin nhận được
            int x = Convert.ToInt32(parts[2]);
            int y = Convert.ToInt32(parts[3]);
            Direction direction = (Direction)Convert.ToInt32(parts[4]);
            float[] color = {
                Convert.ToSingle(parts[5]),
                Convert.ToSingle(parts[6]),
                Convert.ToSingle(parts[7])
            };

            // Kiểm tra tank có tồn tại trong danh sách không
            bool tankExists = ctrl.tanks.Any(t => t.Name == receivedName);

            // Nếu tank không tồn tại, tạo mới và thêm vào danh sách
            if (!tankExists)
            {
                TankNewMsg msg = new TankNewMsg(ctrl.myTank);
                ctrl.nc.Send(msg);
                Tank newTank = new Tank(receivedName, x, y, direction, color)
                {
                    Name = receivedName
                };
                ctrl.tanks.Add(newTank);
            }
        }
    }
}