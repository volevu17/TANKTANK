using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TankTank
{
    public class Blood
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        private Bitmap bmpBlood = GameImages.orangePic;

        public static Size size = new Size(28, 28);
        public static Size bmpSize = new Size(28, 28);

        public Blood(string name, int x, int y)
        {
            this.Name = name;
            this.X = x;
            this.Y = y;
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(bmpBlood, X, Y, size.Width, size.Height);
        }

        public Rectangle GetRectangle()
        {
            Rectangle rec = new Rectangle(new Point(X, Y), size);
            return rec;
        }
    }
}
