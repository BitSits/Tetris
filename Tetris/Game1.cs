using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Tetris
{   
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Texture Variables
        Texture2D gameOver,keysControl,pause;
       
        Song song;           
        Level level;
        KeyboardState currentKb,previousKb;
        bool isPause = false;
        BloomComponent bloom;
       
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            Window.Title = "TETRIS....";
            IsMouseVisible = true;
            bloom = new BloomComponent(this);

            //Components.Add(bloom);
            bloom.Settings = BloomSettings.PresetSettings[5];
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameOver = Content.Load<Texture2D>("gameOver");
            keysControl = Content.Load<Texture2D>("KeysControl");
            pause = Content.Load<Texture2D>("pause");
            MediaPlayer.IsRepeating = true;
            song = Content.Load<Song>("Audio/good day - Triplefox");
            MediaPlayer.Volume = 1.0f;
            LoadGame();
        }
       
        protected override void Update(GameTime gameTime)
        {
            isPause = false;
            currentKb=Keyboard.GetState();
            if (currentKb.IsKeyDown(Keys.Escape))
                this.Exit();
            if (!IsActive)
            {
                isPause = true; return;
            }

            HandleInput();
            if (!level.isGameOver)
            {

                level.Update(gameTime);
            }
            previousKb = currentKb;
            base.Update(gameTime);
        }

        public void HandleInput()
        {
            if (level.isGameOver)
            {
                MediaPlayer.Pause();
            }
            if (currentKb.IsKeyDown(Keys.Enter))
            {
                if (level.isGameOver==true)
                {
                    LoadGame();
                }
            }
        }

        public void LoadGame()
        {
            MediaPlayer.Play(song);
            level = new Level(Services);           
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            level.Draw(spriteBatch);
            if (level.isGameOver)
            {
                spriteBatch.Draw(gameOver, Vector2.Zero, Color.White);
                
            }
            if (!level.isGameOver)
            {
                spriteBatch.Draw(keysControl, Vector2.Zero, Color.White);
            }
            if (isPause==true)
            {
                spriteBatch.Draw(pause, Vector2.Zero, Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
