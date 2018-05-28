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
        public Background(Texture2D texture) : base(texture)
        {
        }

        public void Draw(SpriteBatch sprite)
        {
            for(int i = -4; i <= 4; i++)
            {
                for(int j = -4; j <= 4; j++)
                {
                    sprite.Draw(Texture, new Vector2(i * Width, j * Height), Color.White);
                }
            }
        }
    }
}
