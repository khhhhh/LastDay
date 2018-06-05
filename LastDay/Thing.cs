using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace LastDay
{
    abstract class Thing
    {
        Texture2D texture;
        Vector2 position;

        protected float rotation;

        public virtual float Health { get; set; }
        public float Speed;
        public float Rotation => rotation;
        public Texture2D Texture => texture;
        public Rectangle Rectangle {
            get
            {
                if(Rotation >= Math.PI / 2)
                {
                }
                return new Rectangle((int)(X - Width / 2), (int)(Y - Height / 2), (int)Width, (int)Height);
            }
        }
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
        }

        public virtual void CalculateRotation(float x, float y)
        {
            float X = x - this.X;
            float Y = y - this.Y;
            rotation = (float)Math.Atan2(Y, X);
        }
    }
}
