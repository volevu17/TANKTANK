using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TankTank
{
    public enum TileType
    {
        None, Brick, Iron, Grass, Water
    }

    public class Tile
    {
        public int Life { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public TileType Type { get; set; }

        private Bitmap bmpTile;
        private List<Bitmap> bmpTileAll = GameImages.tilePic;

        public static Size size = new Size(16, 16);
        public static Size bmpSize = new Size(16, 16);

        // Constructor for initializing tile properties and assigning the correct tile image
        public Tile(int x, int y, TileType type)
        {
            this.Life = 1;
            this.X = x;
            this.Y = y;
            this.Type = type;

            // Choose the correct image based on tile type
            switch (type)
            {
                case TileType.Brick:
                    bmpTile = bmpTileAll[0];
                    break;
                case TileType.Iron:
                    bmpTile = bmpTileAll[1];
                    break;
                case TileType.Grass:
                    bmpTile = bmpTileAll[2];
                    break;
                case TileType.Water:
                    bmpTile = bmpTileAll[3];
                    break;
            }
        }

        // Draws the tile image on the screen at the specified coordinates
        public void Draw(Graphics g)
        {
            g.DrawImage(bmpTile, X, Y, size.Width, size.Height);
        }

        // Returns a rectangle that represents the tile's position and size
        public Rectangle GetRectangle()
        {
            // Return an empty rectangle for non-blocking tiles
            if (Type == TileType.None || Type == TileType.Grass)
            {
                return new Rectangle(X, Y, 0, 0);
            }
            // Return a full-size rectangle for other tiles
            else
            {
                return new Rectangle(new Point(X, Y), size);
            }
        }
    }
}