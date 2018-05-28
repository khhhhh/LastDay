using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LastDay
{
    class Bullet : Thing
    {
        public Vector2 velocity;


        public bool isVisible;

        public Bullet(Texture2D texture) : base(texture) => isVisible = false;

        public void Draw (SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

    }
}
