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

        int zombieValue = 20;
        float zombieSpeed = 4;

        Player player;
        Texture2D zombieTexture;
        List<Zombie> zombies = new List<Zombie>();
        Weapon weapon;
        SpriteFont font;

        Texture2D barFullHP;
        Texture2D barCurrentHP;

        Background background;
        Camera camera;

        int time;
        bool createZombies = false;

        bool GameOver = false;

        MouseState mouse;
        KeyboardState keyboard;

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
            graphics.IsFullScreen = true;

            graphics.ApplyChanges();

            camera = new Camera(GraphicsDevice.Viewport);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            player = new Player(Content.Load<Texture2D>("Player"));
            weapon = new Weapon(Content.Load<Texture2D>("Bullet"));
            zombieTexture = Content.Load<Texture2D>("Zombie");
            barFullHP = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            barFullHP.SetData(new Color[] { Color.Gray });

            barCurrentHP = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            barCurrentHP.SetData(new Color[] { Color.Red });

            font = Content.Load<SpriteFont>("font");
            player.weapon = weapon;
            player.Position = new Vector2(
              (displaySize.X / 2),
              (displaySize.Y / 2));


            background = new Background(Content.Load<Texture2D>("background"));


            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }


        void CreateZombies()
        {
            zombieSpeed += 0.4f;
            Random rand = new Random();
            for (int i = 0; i < zombieValue; i++)
            {
                Zombie zombie = new Zombie(zombieTexture);
                zombie.Speed = zombieSpeed;
                zombie.X = rand.Next((int)player.X - 1500, (int)player.X + 1500);
                zombie.Y = rand.Next((int)player.Y - 1500, (int)player.Y + 1500);
                if (Math.Abs(zombie.X - player.X) < displaySize.X / 2 && Math.Abs(zombie.Y - player.Y) < displaySize.Y / 2)
                {
                    zombie.X += displaySize.X;
                    zombie.Y += displaySize.Y;
                }
                zombies.Add(zombie);
            }
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


        protected override void Update(GameTime gameTime)
        {
            mouse = Mouse.GetState();
            keyboard = Keyboard.GetState();

            if (!GameOver)
            {
                camera.Update(gameTime, player);

                if (zombies.Count == 0 && !createZombies)
                {
                    time = gameTime.TotalGameTime.Seconds;
                    createZombies = true;
                }

                if (gameTime.TotalGameTime.Seconds - time > 3 && createZombies)
                {
                    zombieValue += 10;
                    CreateZombies();
                    createZombies = false;
                }

                List<Zombie> list = new List<Zombie>((IEnumerable<Zombie>)zombies);
                for (int i = 0; i < zombies.Count; i++)
                {
                    zombies[i].Move(player);
                    zombies[i].Cross(list);
                    zombies[i].CalculateRotation(player.X + (player.Width / 2), player.Y + (player.Height / 2));
                    background.Cross(zombies[i]);
                }


                for (int i = 0; i < player.weapon.bullets.Count; i++)
                {
                    player.weapon.bullets[i].Cross(zombies);
                }

                background.Cross(player);

                UpdateZombies();

                player.UpdateBullets();
                player.CalculateRotation(mouse.X, mouse.Y);
                player.Move(keyboard);


                if (mouse.LeftButton == ButtonState.Pressed && gameTime.TotalGameTime.Milliseconds % 100 == 0)
                {
                    player.Shoot();
                }


                if(player.Health  < 0)
                {
                    GameOver = true;
                }
            }

            if (keyboard.IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);

        }




        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                null, null, null, null,
                camera.transform);

            background.Draw(spriteBatch);

            foreach (var bullet in player.weapon.bullets)
                bullet.Draw(spriteBatch);

            spriteBatch.Draw(player.Texture, player.Position, null, Color.White, player.Rotation,
                new Vector2(player.Width / 2, player.Height / 2), 1f, SpriteEffects.None, 1);



            foreach (var zombie in zombies)
                zombie.Draw(spriteBatch);

            Rectangle rec = new Rectangle((int)(player.X - displaySize.X / 2) + 70, (int)(player.Y - displaySize.Y / 2) + 50, 300, 30);
            spriteBatch.Draw(barFullHP, rec, Color.White);
            rec.Width = (int)(300 * (player.Health / 100));
            spriteBatch.Draw(barCurrentHP, rec, Color.White);

            if (GameOver)
            {
                spriteBatch.DrawString(font, "Game Over", new Vector2(player.X - 150, player.Y - 40), Color.Red, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
