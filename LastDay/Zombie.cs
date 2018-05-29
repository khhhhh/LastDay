using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace LastDay
{
    class Zombie : Thing
    {
        public override float Health { get; set; } = 100;

        Color color = Color.White;

        
        public void GetDamage()
        {
            Health -= 20;
            color = Color.Red;
        }

        public Zombie(Texture2D texture) : base(texture)
        {
            Speed = 3;
        }

        Vector2 direction = Vector2.Zero;

        public void Move(Thing thing)
        {
            if (!thing.Rectangle.Intersects(this.Rectangle))
            {
                float x = thing.X + thing.Width / 2;
                float y = thing.Y + thing.Height / 2;
                float norm = Vector2.Distance(Position, thing.Position);
                direction = new Vector2((Position.X - x) / norm, (Position.Y - y) / norm);
                string str = thing.GetType().Name;

                X -= Speed * direction.X;
                Y -= Speed * direction.Y;
            }
            else
            {
                    thing.Health -= 0.3f;
            }
        }

        public void Cross(List<Zombie> list)
        {
            foreach (var zombie in list)
                if (Rectangle.Intersects(zombie.Rectangle) && !(zombie == this))
                {
                    zombie.X += Math.Sign(direction.X) * 1;
                    zombie.Y += Math.Sign(direction.Y) * 1;
                
                    X -= Math.Sign(direction.X) * 1;
                    Y -= Math.Sign(direction.Y) * 1;
                }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, color, Rotation,
                 new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 1);
            color = Color.White;
        }
    }
}
