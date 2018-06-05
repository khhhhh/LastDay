using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace LastDay
{
    class Player : Thing
    {
        //Bullets

        public Weapon weapon;

        public override float Health { get; set; } = 100;

        public Texture2D BarFullHP;
        public Texture2D BarCurrentHP;

        public Player(Texture2D playerTexture) : base(playerTexture) { Speed = 5;  }

        public override void CalculateRotation(float x, float y)
        {
            float X = x - Game1.displaySize.X / 2 + Width / 2;
            float Y = y - Game1.displaySize.Y / 2 + Height / 2;
            rotation = (float)Math.Atan2(Y, X) /*+ (float)Math.PI / 2 */;
        }

        public void UpdateBullets()
        {
            weapon.UpdateBullets(this);
        }

        public void Shoot()
        {
            weapon.Shoot(this);
        }

        public void Move(KeyboardState keyboard)
        {
            Keys[] keys = keyboard.GetPressedKeys();

            float speed = this.Speed;

            if (keys.GetLength(0) > 1) { speed *= 0.7f; }

            if (keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A)) { X -= speed; }
            if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D)) { X += speed; }
            if (keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W)) { Y -= speed; }
            if (keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S)) { Y += speed; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, Rotation,
              new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0);

            Rectangle rec = new Rectangle((int)(X - Game1.displaySize.X / 2) + 70, (int)(Y - Game1.displaySize.Y / 2) + 50, 300, 30);
            spriteBatch.Draw(BarFullHP, rec, Color.White);
            rec.Width = (int)(300 * (Health / 100));
            spriteBatch.Draw(BarCurrentHP, rec, Color.White);
        }
    }
}
