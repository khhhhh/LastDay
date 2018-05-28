using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace LastDay
{
    class Zombie : Thing
    {
        float Xt = 0, Yt = 0;
        public override float Health { get; set; } = 100;

        public Zombie(Texture2D texture) : base(texture)
        {
            Speed = 5;

        }

        public void Move(Thing thing)
        {
            if (!thing.Rectangle.Intersects(this.Rectangle))
            {
                Xt = X;
                Yt = Y;
                float x = thing.X + thing.Width / 2;
                float y = thing.Y + thing.Height / 2;
                float norm = Vector2.Distance(Position, thing.Position);
                Vector2 direction = new Vector2((Position.X - x) / norm, (Position.Y - y) / norm);
                string str = thing.GetType().Name;

                X -= Speed * direction.X;
                Y -= Speed * direction.Y;
            }
            else
            {
                if (thing.GetType().Name == "Player")
                    thing.Health -= 0.3f;

            }
        }


        public void Cross(List<Zombie> list)
        {
            foreach (var zombie in list)
                if (Rectangle.Intersects(zombie.Rectangle))
                {
                    X = Xt;
                    Y = Yt;
                }
        }
    }
}
