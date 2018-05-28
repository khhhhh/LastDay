using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace LastDay
{
    public class Game1 : Game
    {
        public static readonly Vector2 displaySize;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int numOfZombies = 20;

        Player player;
        Texture2D zombieTexture;
        List<Zombie> zombies = new List<Zombie>();
        Weapon weapon;


        Background background;
        Camera camera;

        static Game1()
        {
            int w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            displaySize = new Vector2(w, h);
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {

            Window.IsBorderless = false;
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = (int)displaySize.X;
            graphics.PreferredBackBufferHeight = (int)displaySize.Y;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();



            camera = new Camera(GraphicsDevice.Viewport);

            base.Initialize();
        }


        void CreateZombies()
        {
            Random rand = new Random();
            for (int i = 0; i < numOfZombies; i++)
            {
                Zombie zombie = new Zombie(zombieTexture);
                zombie.X = rand.Next((int)player.X - 1500, (int)player.X + 1500);
                zombie.Y = rand.Next((int)player.Y - 1500, (int)player.Y + 1500);
                zombies.Add(zombie);
            }
        }

        protected override void LoadContent()
        {
            player = new Player(Content.Load<Texture2D>("Player"));
            weapon = new Weapon(Content.Load<Texture2D>("Bullet"));
            zombieTexture = Content.Load<Texture2D>("Zombie");
            player.weapon = weapon;
            player.Position = new Vector2(
              (displaySize.X / 2),
              (displaySize.Y / 2));

            CreateZombies();

            background = new Background(Content.Load<Texture2D>("background"));


            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        int time;
        bool createZombies = false;

        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();

            camera.Update(gameTime, player);

            if(zombies.Count == 0 && !createZombies)
            {
                time = gameTime.TotalGameTime.Seconds;
                createZombies = true;
            }

            if (gameTime.TotalGameTime.Seconds - time > 3 && createZombies)
            {
                numOfZombies += 10;
                CreateZombies();
                createZombies = false;
            }

            foreach (var zombie in zombies)
            {
                zombie.Move(player);
               // zombie.Cross(zombies);
            }
            player.CalculateRotation(mouse.X, mouse.Y);

            foreach (var zombie in zombies)
                zombie.CalculateRotation(player.X + (player.Width / 2), player.Y + (player.Height / 2));

            foreach (var bullet in player.weapon.bullets)
            {
                foreach (var zombie in zombies)
                {
                    if (bullet.Rectangle.Intersects(zombie.Rectangle))
                    {
                        bullet.isVisible = false;
                        zombie.Health -= 20;
                    }
                }
            }

            if (mouse.LeftButton == ButtonState.Pressed && gameTime.TotalGameTime.Milliseconds % 100 == 0)
            {
                player.Shoot();
            }

            player.UpdateBullets();
            UpdateZombies();

            player.Move(keyboard);


            if (keyboard.IsKeyDown(Keys.Escape))
                Exit();


            base.Update(gameTime);
        }

        void UpdateZombies()
        {
            for (int i = 0; i < zombies.Count; i++)
            {
                if (zombies[i].Health == 0)
                {
                    zombies.RemoveAt(i);
                }
            }
        }


        float sizeOfTexture = 1f;

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                null, null, null, null,
                camera.transform);

            background.Draw(spriteBatch);

            foreach (var bullet in player.weapon.bullets)
            {
                bullet.Draw(spriteBatch);
            }

            spriteBatch.Draw(player.Texture, player.Position, null, Color.White, player.Rotation,
                new Vector2(player.Width / 2, player.Height / 2), sizeOfTexture, SpriteEffects.None, 1);

            spriteBatch.DrawString(Content.Load<SpriteFont>("font"), player.Health.ToString(), new Vector2(player.X , player.Y), Color.White);

            foreach (var zombie in zombies)
                spriteBatch.Draw(zombie.Texture, zombie.Position, null, Color.White, zombie.Rotation,
                  new Vector2(zombie.Width / 2, zombie.Width / 2), sizeOfTexture, SpriteEffects.None, 1);


            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
