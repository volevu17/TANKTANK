using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TankTank
{
    public class Bullet
    {
        // Properties
        public string FromName { get; set; }
        public int Id { get; set; }
        public int Life { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Direction Dir { get; set; }
        public int Speed { get; set; }
        public int Level { get; set; }

        // Static fields
        public static Size size = new Size(8, 8); // The actual display size
        public static Size bmpSize = new Size(8, 8); // The size of the stock image

        // Private fields
        private Bitmap bmpBulletNow;
        private readonly List<Bitmap> bmpBulletAll = GameImages.bulletPic;

        // Constructor
        public Bullet(string fromName, int id, int x, int y, Direction dir)
        {
            FromName = fromName;
            Id = id;
            X = x;
            Y = y;
            Life = 3;
            Dir = dir;
            Speed = 15;
            Level = 1;
        }

        // Private method for movement
        private void Move()
        {
            if (Dir == Direction.Up)
                Y -= Speed;
            else if (Dir == Direction.Down)
                Y += Speed;
            else if (Dir == Direction.Left)
                X -= Speed;
            else if (Dir == Direction.Right)
                X += Speed;
        }

        // Public method to draw the bullet and then move it
        public void Draw(Graphics g)
        {
            if (Dir == Direction.Up)
                bmpBulletNow = bmpBulletAll[0];
            else if (Dir == Direction.Down)
                bmpBulletNow = bmpBulletAll[2];
            else if (Dir == Direction.Left)
                bmpBulletNow = bmpBulletAll[3];
            else if (Dir == Direction.Right)
                bmpBulletNow = bmpBulletAll[1];

            g.DrawImage(bmpBulletNow, X, Y, size.Width, size.Height);
            Move();
        }

        // Public method to get the bullet's rectangular bounds
        public Rectangle GetRectangle()
        {
            return new Rectangle(new Point(X, Y), size);
        }
    }
}