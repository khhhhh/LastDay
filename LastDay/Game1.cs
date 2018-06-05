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


        int zombieValue = 0;
        float zombieSpeed = 4;
        int day = 0;

        int kills = 0;

        Player player;
        Texture2D zombieTexture;
        List<Zombie> zombies = new List<Zombie>();
        Weapon weapon;
        SpriteFont font;
        SpriteFont font2;

        Background background;
        Camera camera;

        double time;
        bool createZombies = false;

        //Working With Text

        int signTransparency = -3;
        Texture2D blackScreen;
        int textTransparency = 0;
        bool nextLevel = true;
        bool gameOver = false;

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
            player.BarFullHP = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            player.BarFullHP.SetData(new Color[] { Color.Gray });

            blackScreen = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blackScreen.SetData(new Color[] { Color.Black });

            player.BarCurrentHP = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            player.BarCurrentHP.SetData(new Color[] { Color.Red });

            font = Content.Load<SpriteFont>("font");
            font2 = Content.Load<SpriteFont>("font2");
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
            zombieSpeed += 0.1f;
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
                    kills++;
                }
            }
        }


        protected override void Update(GameTime gameTime)
        {
            mouse = Mouse.GetState();
            keyboard = Keyboard.GetState();

            if (!gameOver)
            {
                camera.Update(gameTime, player);

                if (zombies.Count == 0 && !createZombies)
                {
                    time = gameTime.TotalGameTime.TotalSeconds;
                    nextLevel = true;
                    day++;
                    createZombies = true;

                    textTransparency = 0;
                    signTransparency *= -1;
                }

                if (nextLevel)
                {
                    if (textTransparency >= 255)
                        signTransparency *= -1;

                    textTransparency += signTransparency;
                }

                if (gameTime.TotalGameTime.TotalSeconds - time > 3 && createZombies)
                {
                    zombieValue += 5;
                    nextLevel = false;

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
                    gameOver = true;
                    textTransparency = 0;
                }
            }
            else
            {
                textTransparency += 10;
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


            player.Draw(spriteBatch);

            foreach (var zombie in zombies)
                zombie.Draw(spriteBatch);

            Vector2 rec = new Vector2(player.X + displaySize.X / 2 - 70, player.Y - displaySize.Y / 2 + 50);

            spriteBatch.DrawString(font2, $"Kills: {kills}", rec, Color.White);

            if (gameOver)
            {
                spriteBatch.Draw(blackScreen, new Rectangle((int)(player.X - displaySize.X * 2), (int)(player.Y - displaySize.Y * 2), (int)displaySize.X * 3, (int)displaySize.Y * 3), new Color(Color.Black, textTransparency));
                spriteBatch.DrawString(font, "Game Over", new Vector2(player.X - 150, player.Y - 40), new Color(255, 0, 0, textTransparency), 0f, Vector2.Zero, 1f, SpriteEffects.None, 1);
            }

            if (nextLevel)
            {
                spriteBatch.Draw(blackScreen, new Rectangle((int)(player.X - displaySize.X * 2), (int)(player.Y - displaySize.Y * 2), (int)displaySize.X * 3, (int)displaySize.Y * 3), new Color(Color.Black, textTransparency));
                spriteBatch.DrawString(font, $"Day {day}", new Vector2(player.X - 150, player.Y - 40), new Color(Color.White, textTransparency), 0f, Vector2.Zero, 1f, SpriteEffects.None, 1);

            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
