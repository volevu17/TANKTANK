using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace TankTank
{
    public class Controller
    {
        public Enemy covid;
        public Tank myTank;
        public List<Blood> orange = new List<Blood>();
        public List<Tank> tanks = new List<Tank>();
        public List<Enemy> covids = new List<Enemy>();
        public List<Bullet> bullets = new List<Bullet>();
        public List<Explode> explodes = new List<Explode>();
        public int[,] lineMap = new int[40, 30];
        public Tile[,] map;

        private Size tis = Tile.size;

        public static Random s_Random = new Random(Guid.NewGuid().GetHashCode());

        private int gameWidth, gameHeight, mapWidth, mapHeight;
        private bool dirUp, cDirUp;
        private bool dirDown, cDirDown;
        private bool dirLeft, cDirLeft;
        private bool dirRight, cDirRight;
        private Direction dirOld, cDirOld;
        public NetClient nc;

        public Controller(int gameWidth, int gameHeight)
        {
            this.gameWidth = gameWidth;
            this.gameHeight = gameHeight;
            this.mapWidth = gameWidth / tis.Width;
            this.mapHeight = gameHeight / tis.Height;
            map = new Tile[mapWidth, mapHeight];
        }

        private void LoadMap()
        {
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    var tileType = (TileType)lineMap[i, j];
                    if (tileType != TileType.None)
                    {
                        map[i, j] = new Tile(i * tis.Width, j * tis.Height, tileType);
                    }
                }
            }
        }

public bool NetClientConnect(string ip, int port, float[] tankColor, string name)
{
    int randomId = GenerateRandomId();
    InitializeGameObjects(name, randomId, tankColor);
    nc = new NetClient(this);
    if (nc.Connect(ip, port))
    {
        LoadMap();
        return true;
    }
    else
    {
        return false;
    }
}

private int GenerateRandomId()
{
    Random random = new Random();
    return random.Next(100, 999);
}

private void InitializeGameObjects(string baseName, int id, float[] color)
{
    string uniqueName = $"{baseName}{id}";
    myTank = new Tank(uniqueName, s_Random.Next(5, gameWidth), s_Random.Next(5, gameHeight), Direction.Up, color);
    covid = new Enemy("corona" + id.ToString(), s_Random.Next(15, gameWidth), s_Random.Next(15, gameHeight), Direction.Right);
}


        public void Paint(Graphics g)
        {
            if (myTank != null)
            {
                g.DrawString(myTank.Name + " Life:  " + myTank.Life, new Font("Arial", 8), new SolidBrush(Color.White), 20, 20);
                g.DrawString("EnemyTanks  Count: " + tanks.Count, new Font("Arial", 8), new SolidBrush(Color.White), 20, 50);
            }
            //bullet
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].Life > 0)
                {
                    bullets[i].Draw(g);
                    BulletMove(bullets[i]);
                }
            }
            //orange
            if (orange.Count > 0)
            {
                for (int i = 0; i < orange.Count; i++)
                {
                    orange[i].Draw(g);
                }
            }
            //opponents's tanks
            for (int i = 0; i < tanks.Count; i++)
            {
                if (tanks[i].Life > 0)
                {
                    tanks[i].Draw(g);
                    tanks[i].Move();
                }
            }
            //covids
            for (int i = 0; i < covids.Count; i++)
            {
                if (covids[i].Life > 0)
                {
                    covids[i].Draw(g);
                    covids[i].Move();
                }
            }

            if (covid != null && covid.Life > 0)
            {
                covid.Draw(g);
                EnemyMove(covid);
            }

            //my tank
            if (myTank != null && myTank.Life > 0)
            {
                myTank.Draw(g);
                TankMove(myTank);
            }
            //Blocks
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    if (map[i, j] != null)
                    {
                        map[i, j].Draw(g);
                    }
                }
            }
            //Explosion effect
            for (int i = 0; i < explodes.Count; i++)
            {
                if (explodes[i].Live == true)
                {
                    explodes[i].Draw(g);
                }
                else
                {
                    explodes.Remove(explodes[i]);
                    i--;
                }
            }
        }

        private void TankMove(Tank tank)
        {
            dirOld = tank.Dir;
            if (tank.Dir != Direction.Stop)
            {
                tank.DirOld = tank.Dir;
            }

            if (dirUp)
            {
                tank.Dir = Direction.Up;
                tank.Move();
            }
            if (dirDown)
            {
                tank.Dir = Direction.Down;
                tank.Move();
            }
            if (dirLeft)
            {
                tank.Dir = Direction.Left;
                tank.Move();
            }
            if (dirRight)
            {
                tank.Dir = Direction.Right;
                tank.Move();
            }
            if (!dirUp && !dirDown && !dirLeft && !dirRight)
            {
                tank.Dir = Direction.Stop;
            }
            //Collision detection with boundaries
            if (tank.X < 0 || tank.Y < 0 || tank.X > gameWidth - Tank.size.Width || tank.Y > gameHeight - Tank.size.Height)
            {
                myTank.X = myTank.XOld;
                myTank.Y = myTank.YOld;
                tank.Dir = Direction.Stop;
            }
            //Collision detection with tanks
            for (int i = 0; i < tanks.Count; i++)
            {
                if (myTank != null && CollisionDetection(tanks[i].GetRectangle(), myTank.GetRectangle()))
                {
                    myTank.X = myTank.XOld;
                    myTank.Y = myTank.YOld;
                    tank.Dir = Direction.Stop;
                }
            }
            //collision detection with covids
            for (int i = 0; i < covids.Count; i++)
            {
                if (myTank != null && CollisionDetection(covids[i].GetRectangle(), myTank.GetRectangle()))
                {
                    myTank.X = myTank.XOld;
                    myTank.Y = myTank.YOld;
                    tank.Dir = Direction.Stop;
                    Explode e = new Explode(covids[i].X, covids[i].Y);
                    explodes.Add(e);
                    if (myTank.Life > 0) myTank.Life -= 1;
                }
            }
            if (myTank != null && CollisionDetection(covid.GetRectangle(), myTank.GetRectangle()))
            {
                myTank.X = myTank.XOld;
                myTank.Y = myTank.YOld;
                tank.Dir = Direction.Stop;
                Explode e = new Explode(covid.X, covid.Y);
                explodes.Add(e);
                if (myTank.Life > 0) myTank.Life -= 1;
            }
            //Collision with orange
            for (int i = 0; i < orange.Count; i++)
            {
                if (myTank != null && CollisionDetection(orange[i].GetRectangle(), myTank.GetRectangle()))
                {
                    tank.Dir = Direction.Stop;
                    if (myTank.Life < 3) myTank.Life += 1;
                    orange.Remove(orange[i]);
                }
            }
            for (int i = 0; i < tanks.Count; i++)
            {
                for (int j = 0; j < orange.Count; j++) {
                    if (tanks[i] != null && CollisionDetection(tanks[i].GetRectangle(), orange[j].GetRectangle()))
                    {
                        tanks[i].Dir = Direction.Stop;
                        if (tanks[i].Life < 3) tanks[i].Life += 1;
                        orange.Remove(orange[j]);
                    }
                }
            }
            //Collision detection with map blocks
            int tmpX = myTank.X / tis.Width;
            int tmpY = myTank.Y / tis.Height;
            for (int i = tmpX - 1; i < tmpX + 3; i++)
            {
                for (int j = tmpY - 1; j < tmpY + 3; j++)
                {
                    if (i < 0 || j < 0 || i >= mapWidth || j >= mapHeight)
                    {
                        continue;
                    }
                    if (map[i, j] != null)
                    {
                        if (CollisionDetection(map[i, j].GetRectangle(), myTank.GetRectangle()))
                        {
                            myTank.X = myTank.XOld;
                            myTank.Y = myTank.YOld;
                            tank.Dir = Direction.Stop;
                        }
                    }
                }
            }
            //message send when the direction is changed
            if (nc != null && tank.Dir != dirOld)
            {
                TankMoveMsg msg = new TankMoveMsg(tank.Name, tank.X, tank.Y, tank.Dir);
                nc.Send(msg);
            }
        }

        private void EnemyMove(Enemy enemy)
        {
            UpdateEnemyDirection(enemy);
            PerformEnemyCollisionChecks(enemy);
            SendEnemyDirectionChangeMessage(enemy);
        }

        private void UpdateEnemyDirection(Enemy enemy)
        {
            Direction originalDirection = enemy.Dir;

            if (cDirUp) enemy.Dir = Direction.Up;
            else if (cDirDown) enemy.Dir = Direction.Down;
            else if (cDirLeft) enemy.Dir = Direction.Left;
            else if (cDirRight) enemy.Dir = Direction.Right;
            else enemy.Dir = Direction.Stop;

            if (enemy.Dir != Direction.Stop) enemy.Move();

            if (enemy.Dir != originalDirection && nc != null)
            {
                EnemyMoveMsg msg = new EnemyMoveMsg(enemy.Name, enemy.X, enemy.Y, enemy.Dir);
                nc.Send(msg);
            }
        }

        private void PerformEnemyCollisionChecks(Enemy enemy)
        {
            CheckEnemyBoundaryCollisions(enemy);
            CheckEnemyTankCollisions(enemy);
            CheckEnemyMapCollisions(enemy);
        }

        private void CheckEnemyBoundaryCollisions(Enemy enemy)
        {
            if (enemy.X < 0 || enemy.Y < 0 || enemy.X > gameWidth - Tank.size.Width || enemy.Y > gameHeight - Tank.size.Height)
            {
                ResetEnemyPosition(enemy);
            }
        }

        private void CheckEnemyTankCollisions(Enemy enemy)
        {
            foreach (var tank in tanks)
            {
                if (CollisionDetection(tank.GetRectangle(), enemy.GetRectangle()))
                {
                    ResetEnemyPosition(enemy);
                    HandleTankCollision(tank, enemy);
                }
            }
        }

        private void CheckEnemyMapCollisions(Enemy enemy)
        {
            // Similar to CheckMapCollisions, tailored for enemies
        }

        private void ResetEnemyPosition(Enemy enemy)
        {
            enemy.X = enemy.XOld;
            enemy.Y = enemy.YOld;
            enemy.Dir = Direction.Stop;
        }

        private void HandleTankCollision(Tank tank, Enemy enemy)
        {
            Explode e = new Explode(tank.X, tank.Y);
            explodes.Add(e);
            if (tank.Life > 0) tank.Life -= 1;
            else tanks.Remove(tank);
        }

        private void SendEnemyDirectionChangeMessage(Enemy enemy)
        {
            if (nc != null && enemy.Dir != enemy.DirOld)
            {
                EnemyMoveMsg msg = new EnemyMoveMsg(enemy.Name, enemy.X, enemy.Y, enemy.Dir);
                nc.Send(msg);
            }
        }

        public void BulletMove(Bullet bullet)
        {
            //Bullets collide with tanks
            for (int i = 0; i < tanks.Count; i++)
            {
                if (CollisionDetection(tanks[i].GetRectangle(), bullet.GetRectangle())
                    && tanks[i].Name != bullet.FromName)
                {
                    Explode e = new Explode(tanks[i].X, tanks[i].Y);
                    explodes.Add(e);

                    tanks[i].Life -= 1;
                    if (tanks[i].Life == 0) tanks.Remove(tanks[i]);
                    bullets.Remove(bullet);
                    return;
                }
            }
            if (myTank != null && CollisionDetection(myTank.GetRectangle(), bullet.GetRectangle())
                && myTank.Name != bullet.FromName && myTank.Life > 0)
            {
                Explode e = new Explode(myTank.X, myTank.Y);
                explodes.Add(e);

                myTank.Life -= 1;
                bullets.Remove(bullet);
                return;
            }
            //bullets collide with covids
            for (int i = 0; i < covids.Count; i++)
            {
                if (CollisionDetection(covids[i].GetRectangle(), bullet.GetRectangle())
                    && covids[i].Name != bullet.FromName)
                {
                    Explode e = new Explode(covids[i].X, covids[i].Y);
                    explodes.Add(e);

                    covids[i].Life -= 1;
                    if (covids[i].Life == 0) 
                    {
                        Blood o = new Blood(s_Random.ToString(), covids[i].X, covids[i].Y);
                        orange.Add(o);
                        covids.Remove(covids[i]); 
                    }
                    bullets.Remove(bullet);
                    return;
                }
            }
            if (covid != null && CollisionDetection(covid.GetRectangle(), bullet.GetRectangle())
                && covid.Name != bullet.FromName && covid.Life > 0)
            {
                Explode e = new Explode(covid.X, covid.Y);
                explodes.Add(e);

                covid.Life -= 1;
                if (covid.Life == 0)
                {
                    Blood o = new Blood(s_Random.ToString(), covid.X, covid.Y);
                    orange.Add(o);

                    Random r = new Random();
                    int rNum = r.Next(100, 999);
                    covid = new Enemy("corona" + rNum.ToString(), s_Random.Next(0, gameWidth), s_Random.Next(0, gameHeight), Direction.Up);
                    EnemyNewMsg msg = new EnemyNewMsg(covid);
                    nc.Send(msg);
                }
                bullets.Remove(bullet);
                return;
            }

            //Bullets collide with bullets
            for (int i = 0; i < bullets.Count; i++)
            {
                if (CollisionDetection(bullets[i].GetRectangle(), bullet.GetRectangle()) && bullet.FromName != bullets[i].FromName)
                {
                    bullets[i].Life = 0;
                    bullets.Remove(bullets[i]);
                    bullet.Life = 0;
                    bullets.Remove(bullet);
                    return;
                }
            }

            //Bullets collide with blocks
            int tmpX = bullet.X / tis.Width;
            int tmpY = bullet.Y / tis.Height;
            for (int i = tmpX - 1; i < tmpX + 2; i++)
            {
                for (int j = tmpY - 1; j < tmpY + 2; j++)
                {
                    if (i < 0 || j < 0 || i >= mapWidth || j >= mapHeight)
                    {
                        continue;
                    }
                    if (map[i, j] != null)
                    {
                        if (map[i, j].Type == TileType.Brick || map[i, j].Type == TileType.Iron)
                        {
                            if (CollisionDetection(map[i, j].GetRectangle(), bullet.GetRectangle()))
                            {
                                if(map[i, j].Type == TileType.Brick) map[i, j] = null;

                                Explode e = new Explode(bullet.X - Explode.size.Width / 2 + Bullet.size.Width / 2,
                                    bullet.Y - Explode.size.Height / 2 + Bullet.size.Height / 2);
                                explodes.Add(e);

                                bullet.Life = 0;
                                bullets.Remove(bullet);
                                return;
                            }
                        }
                    }
                }
            }

            //Bullets flew out of the border
            if (bullet.X < 0 || bullet.Y < 0
                || bullet.X > gameWidth - Bullet.size.Width
                || bullet.Y > gameHeight - Bullet.size.Height)
            {
                bullet.Life = 0;
                bullets.Remove(bullet);
            }
        }

        public void KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (Char.ToUpper(e.KeyChar))  // Sử dụng Char.ToUpper để không cần kiểm tra cả ký tự thường và hoa
            {
                case 'W':
                    SetDirection(true, false, false, false);
                    break;
                case 'A':
                    SetDirection(false, false, true, false);
                    break;
                case 'S':
                    SetDirection(false, true, false, false);
                    break;
                case 'D':
                    SetDirection(false, false, false, true);
                    break;
            }
        }

        private void SetDirection(bool up, bool down, bool left, bool right)
        {
            dirUp = up;
            dirDown = down;
            dirLeft = left;
            dirRight = right;
        }
        public void KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                case Keys.S:
                    dirUp = false;
                    dirDown = false;
                    break;
                case Keys.A:
                case Keys.D:
                    dirLeft = false;
                    dirRight = false;
                    break;
                case Keys.Space:
                    HandleFire();
                    break;
            }
        }

        private void HandleFire()
        {
            if (nc != null && myTank.Life > 0)
            {
                Bullet b = myTank.Fire();
                bullets.Add(b);
                BulletNewMsg msg = new BulletNewMsg(b);
                nc.Send(msg);
            }
        }


        public void EnemyMove2(object sender, EventArgs e)
        {
            int direction = s_Random.Next(1, 5);  // Chỉnh sửa phạm vi từ 1 đến 4 thành 1 đến 5 để bao gồm tất cả các hướng
            SetEnemyDirection(direction);
        }

        private void SetEnemyDirection(int direction)
        {
            // Reset all directions to false before setting the new direction
            cDirUp = cDirDown = cDirLeft = cDirRight = false;

            switch (direction)
            {
                case 1:
                    cDirUp = true;
                    break;
                case 2:
                    cDirLeft = true;
                    break;
                case 3:
                    cDirDown = true;
                    break;
                case 4:
                    cDirRight = true;
                    break;
            }
        }

        public void coronaShoot(object sender, EventArgs e)
        {
            if (CanShoot(covid))
            {
                FireBullet(covid);
            }
        }

        private bool CanShoot(Enemy enemy)
        {
            return enemy != null && enemy.Life > 0;
        }

        private void FireBullet(Enemy enemy)
        {
            Bullet bullet = enemy.Fire();
            if (bullet != null)
            {
                bullets.Add(bullet);
                SendBulletMessage(bullet);
            }
        }

        private void SendBulletMessage(Bullet bullet)
        {
            if (nc != null)
            {
                BulletNewMsg msg = new BulletNewMsg(bullet);
                try
                {
                    nc.Send(msg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error sending bullet message: " + ex.Message);
                }
            }
        }

        private bool CollisionDetection(Rectangle rec1, Rectangle rec2)
        {
            // Trả về false ngay lập tức nếu một trong các hình chữ nhật không có kích thước hợp lệ
            if ((rec1.Width == 0 && rec1.Height == 0) || (rec2.Width == 0 && rec2.Height == 0))
            {
                return false;
            }

            // Kiểm tra xem các hình chữ nhật có giao nhau không
            return rec1.IntersectsWith(rec2);
        }
    }
}
