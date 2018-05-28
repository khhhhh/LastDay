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

        public Player(Texture2D playerTexture) : base(playerTexture) { Speed = 5;  }

        public override void CalculateRotation(float x, float y)
        {
            float X = x - Game1.displaySize.X / 2 + Width / 2;
            float Y = y - Game1.displaySize.Y / 2 + Height / 2;
            rotation = (float)Math.Atan2(Y, X) + (float)Math.PI / 2;
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

    }
}
