using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastDay
{
    class Weapon : Thing
    {
        public List<Bullet> bullets = new List<Bullet>();

        public Weapon(Texture2D texture) : base(texture) { }

        public void UpdateBullets(Thing thing)
        {
            foreach (var bullet in bullets)
            {
                bullet.Position += bullet.velocity;
                if (Vector2.Distance(bullet.Position, thing.Position) > Game1.displaySize.X / 2 + 50)
                {
                    bullet.isVisible = false;
                }
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].isVisible)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Shoot(Thing thing)
        {
            Bullet bullet = new Bullet(Texture);
            bullet.velocity = new Vector2((float)Math.Cos(thing.Rotation /*- (float)Math.PI / 2 */), (float)Math.Sin(thing.Rotation /*- (float)Math.PI / 2 */) ) * 20f;
            bullet.Position = thing.Position + bullet.velocity;
            bullet.isVisible = true;

            if (bullets.Count < 20)
                bullets.Add(bullet);
        }



    }
}
