using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastDay
{
    class Camera
    {
        public Matrix transform;
        Viewport viewport;
        Vector2 center;

        public Camera(Viewport viewport) => this.viewport = viewport;

        public void Update(GameTime gameTime, Player player)
        {
            center = new Vector2(player.X + (player.Width / 2) - Game1.displaySize.X / 2, player.Y + (player.Height / 2) - Game1.displaySize.Y / 2 );
            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) * Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));
        }

    }
}
