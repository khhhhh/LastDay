using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace LastDay
{
    class Thing
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;

        protected float rotation;

        public virtual float Health { get; set; }
        public float Speed;
        public float Rotation => rotation;
        public Texture2D Texture => texture;
        public Rectangle Rectangle => new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float X
        {
            get { return position.X; }
            set { position.X = value; }
        }
        public float Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public float Width => texture.Width;
        public float Height => texture.Height;
    
        public Thing(Texture2D texture)
        {
            this.texture = texture;
            rectangle = new Rectangle((int)X, (int)Y, (int)(Width * 0.3f), (int)(Height * 0.3f));
        }

        public virtual void CalculateRotation(float x, float y)
        {
            float X = x - this.X;
            float Y = y - this.Y;
            rotation = (float)Math.Atan2(Y, X) + (float) Math.PI / 2;
        }
    }
}
