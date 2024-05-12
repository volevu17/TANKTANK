using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace TankTank
{
    public enum MsgType
    {
        None,
        TankNew,
        TankMove,
        BulletNew,
        EnemyNew,
        EnemyMove,
        HealNew
    }

    public interface Msg
    {
        /// <summary>
        /// Gửi tin nhắn.
        /// </summary>
        /// <param name="uc">Đối tượng UdpClient nơi mà gói tin được gửi.</param>
        /// <param name="ip">Địa chỉ IP của server.</param>
        /// <param name="udpPort">Cổng UDP của server.</param>
        void Send(UdpClient uc, string ip, int udpPort);

        /// <summary>
        /// Phân tích tin nhắn.
        /// </summary>
        /// <param name="b">Mảng byte để phân tích.</param>
        void Parse(byte[] b);
    }
}

