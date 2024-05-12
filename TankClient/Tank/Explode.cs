using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TankTank
{
    public class Explode
    {
        // Properties
        public bool Live { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        // Static fields
        public static Size size = new Size(28, 28);
        public static Size bmpSize = new Size(28, 28);

        // Private fields
        private readonly Bitmap bmpExplode = GameImages.explodePic;
        private static readonly int span = 5;
        private int currentFrame = 0;

        // Constructor
        public Explode(int x, int y)
        {
            Live = true;
            X = x;
            Y = y;
        }

        // Method to draw the explosion effect
        public void Draw(Graphics g)
        {
            if (currentFrame >= span)
            {
                currentFrame = 0;
                Live = false;
                return;
            }

            // Draw the explosion image
            g.DrawImage(bmpExplode, X, Y, size.Width, size.Height);

            // Increment the frame counter
            currentFrame++;
        }
    }
}