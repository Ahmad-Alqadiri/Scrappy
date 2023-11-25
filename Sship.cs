using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrappy
{
    internal class Sship
    {
        
        public bool IsPlayer;
        public int Health = 4;
        public int X;
        public int Y;
        public Direction Direction = Direction.Down;
        public Bullet Bullet;
        public bool IsShooting;
        public int ExplodingFrame;
        public bool IsExploding => ExplodingFrame > 0;

    }
}
