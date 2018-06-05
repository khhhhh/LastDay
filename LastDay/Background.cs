using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LastDay
{

    class Background : Thing
    {
        Vector2 coordinates = new Vector2(0, 0);

        public Background(Texture2D texture) : base(texture) { }

        public void Draw(SpriteBatch sprite)
        {
            for (int i = -4; i <= 4; i++)
            {
                for (int j = -4; j <= 4; j++)
                {
                    sprite.Draw(Texture, new Vector2(i * Width, j * Height), Color.White);
                }
            }
        }

        public void Cross(Thing thing)
        {
            if (!Rectangle.Intersects(thing.Rectangle))
            {

                if (thing.X <= -4 * Width + thing.Width / 2)
                {
                    thing.X = -4 * Width + thing.Width / 2;
                }
                if (thing.X >= 5 * Width - thing.Width / 2)
                {
                    thing.X = 5 * Width - thing.Width / 2;
                }
                if (thing.Y <= -4 * Height + thing.Height / 2)
                {
                    thing.Y = -4 * Height + thing.Height / 2;
                }
                if (thing.Y >= 5 * Height - thing.Height / 2)
                {
                    thing.Y = 5 * Height - thing.Height / 2;
                }
            }
        }
    }
}
