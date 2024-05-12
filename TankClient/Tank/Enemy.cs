using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TankTank
{
    public class Enemy
    {
        // Static fields
        public static Random s_Random = new Random(Guid.NewGuid().GetHashCode());
        public static Size size = new Size(28, 28); // actual display size
        public static Size bmpSize = new Size(28, 28); // size of image

        // Properties
        public string Name { get; set; }
        public int Life { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int XOld { get; set; }
        public int YOld { get; set; }
        public Direction Dir { get; set; }
        public Direction DirOld { get; set; }
        public int Speed { get; set; }
        public int Level { get; set; }

        // Private fields
        private Bitmap bmpEnemyNow = GameImages.coronaPic;
        private int bulletId = 0;

        // Constructor
        public Enemy(string name, int x, int y, Direction dir)
        {
            Name = name;
            X = x;
            Y = y;
            Life = s_Random.Next(1, 5);
            Dir = dir;
            DirOld = dir;
            Speed = s_Random.Next(1, 5);
            Level = 0;
        }

        // Method to move the Enemy object
        public void Move()
        {
            XOld = X;
            YOld = Y;

            if (Dir == Direction.Up)
                Y -= Speed;
            else if (Dir == Direction.Down)
                Y += Speed;
            else if (Dir == Direction.Left)
                X -= Speed;
            else if (Dir == Direction.Right)
                X += Speed;
        }

        // Method to fire a bullet in the current or last direction
        public Bullet Fire()
        {
            Direction bulletDir = Dir == Direction.Stop ? DirOld : Dir;

            // Calculate the bullet starting position centered on the Enemy object
            int bulletX = X + size.Width / 2 - Bullet.size.Width / 2;
            int bulletY = Y + size.Height / 2 - Bullet.size.Height / 2;

            return new Bullet(Name, bulletId++, bulletX, bulletY, bulletDir);
        }

        // Method to draw the Enemy object
        public void Draw(Graphics g)
        {
            g.DrawImage(bmpEnemyNow, X, Y, size.Width, size.Height);
        }

        // Method to get the rectangular boundary of the Enemy object
        public Rectangle GetRectangle()
        {
            return new Rectangle(new Point(X, Y), size);
        }
    }
}