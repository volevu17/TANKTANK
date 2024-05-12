using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace TankTank
{
    public class GameImages
    {
        // Static image resources
        public static List<Bitmap>[] tankPic = new List<Bitmap>[16];
        public static Bitmap coronaPic;
        public static Bitmap orangePic;
        public static List<Bitmap> bulletPic = new List<Bitmap>();
        public static Bitmap explodePic;
        public static List<Bitmap> tilePic = new List<Bitmap>();

        // Image sizes
        private Size tankSize = Tank.bmpSize;
        private Size coronaSize = Enemy.bmpSize;
        private Size bulletSize = Bullet.bmpSize;
        private Size explodeSize = Explode.bmpSize;
        private Size tileSize = Tile.bmpSize;

        // Resource path
        private string picPath = Application.StartupPath + "//Res//";

        // Constructor for initializing game images
        public GameImages()
        {
            InitializeTankImages();
            InitializeEnemyImage();
            InitializeBloodImage();
            InitializeBulletImages();
            InitializeExplodeImage();
            InitializeTileImages();
        }

        // Initialize the tank images
        private void InitializeTankImages()
        {
            Bitmap bmpTank1 = new Bitmap(picPath + "tank1.bmp");
            bmpTank1.MakeTransparent(Color.Black);
            PixelFormat format = bmpTank1.PixelFormat;

            // Initialize lists for all tanks
            for (int i = 0; i < 16; i++)
            {
                tankPic[i] = new List<Bitmap>();
            }

            // Slice and store tank images
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Rectangle tmpRec = new Rectangle(i * tankSize.Width, j * tankSize.Height, tankSize.Width, tankSize.Height);
                    Bitmap tmpBmp = bmpTank1.Clone(tmpRec, format);
                    tankPic[i].Add(tmpBmp);
                }
            }
        }

        // Initialize the corona image
        private void InitializeEnemyImage()
        {
            coronaPic = new Bitmap(picPath + "covid.bmp");
            coronaPic.MakeTransparent(Color.Black);
        }

        // Initialize the orange image
        private void InitializeBloodImage()
        {
            orangePic = new Bitmap(picPath + "orange.bmp");
            orangePic.MakeTransparent(Color.Black);
        }

        // Initialize the bullet images
        private void InitializeBulletImages()
        {
            Bitmap bmpBullet = new Bitmap(picPath + "bullet.bmp");
            bmpBullet.MakeTransparent(Color.Black);
            PixelFormat format = bmpBullet.PixelFormat;

            for (int i = 0; i < 4; i++)
            {
                Rectangle tmpRec = new Rectangle(i * bulletSize.Width, 0, bulletSize.Width, bulletSize.Height);
                Bitmap tmpBmp = bmpBullet.Clone(tmpRec, format);
                bulletPic.Add(tmpBmp);
            }
        }

        // Initialize the explode image
        private void InitializeExplodeImage()
        {
            explodePic = new Bitmap(picPath + "bomb1.bmp");
            explodePic.MakeTransparent(Color.Black);
        }

        // Initialize the tile images
        private void InitializeTileImages()
        {
            Bitmap bmpTile = new Bitmap(picPath + "tile.bmp");
            bmpTile.MakeTransparent(Color.Black);
            PixelFormat format = bmpTile.PixelFormat;

            for (int i = 0; i < 4; i++)
            {
                Rectangle tmpRec = new Rectangle(i * tileSize.Width * 2, 0, tileSize.Width, tileSize.Height);
                Bitmap tmpBmp = bmpTile.Clone(tmpRec, format);
                tilePic.Add(tmpBmp);
            }
        }

        // Apply a color matrix to a list of bitmaps
        public static List<Bitmap> Recoloring(List<Bitmap> bmps, float[] color)
        {
            List<Bitmap> imgs = new List<Bitmap>();
            float[][] colorMatrixElements = {
                new float[] {1, 0, 0, 0, 0},
                new float[] {0, 1, 0, 0, 0},
                new float[] {0, 0, 1, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {color[0], color[1], color[2], 0, 0}
            };

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(
                colorMatrix,
                ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap
            );

            foreach (Bitmap bmp in bmps)
            {
                Bitmap tmp = new Bitmap(bmp.Width, bmp.Height);
                using (Graphics bmpGraphics = Graphics.FromImage(tmp))
                {
                    Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                    bmpGraphics.DrawImage(bmp, rect, 0, 0, rect.Width, rect.Height, GraphicsUnit.Pixel, imageAttributes);
                }
                imgs.Add(tmp);
            }
            return imgs;
        }

        // Apply a color matrix to a single bitmap
        public static Bitmap Recoloring(Bitmap bmp, float[] color)
        {
            Bitmap img = new Bitmap(bmp.Width, bmp.Height);
            float[][] colorMatrixElements = {
                new float[] {1, 0, 0, 0, 0},
                new float[] {0, 1, 0, 0, 0},
                new float[] {0, 0, 1, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {color[0], color[1], color[2], 0, 0}
            };

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(
                colorMatrix,
                ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap
            );

            using (Graphics bmpGraphics = Graphics.FromImage(img))
            {
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                bmpGraphics.DrawImage(bmp, rect, 0, 0, rect.Width, rect.Height, GraphicsUnit.Pixel, imageAttributes);
            }

            return img;
        }
    }
}
