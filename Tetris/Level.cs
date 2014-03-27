using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Tetris
{
    class Level
    {
       
        public int lines=0;
        public int levelUp = 1;
        public int linesCount = 0;
        public Color color=Color.Gainsboro;

        TetrisWell tetrisWell;
        Block block;
        ContentManager content;
        int[,] wellMatrix;
        public bool isGameOver = false;
        SpriteFont font;
        Texture2D credits;

        public ContentManager Content { get { return content; } }
        public int[,] WellMatrix { get { return wellMatrix; } }        
        
        public Level(IServiceProvider serviceProvider)
        {
            content = new ContentManager(serviceProvider, "Content");
            tetrisWell = new TetrisWell(this);
            block = new Block(this, tetrisWell.WellPosition);
            wellMatrix = tetrisWell.wellMatrix;
            LoadContent();
        }

        public void LoadContent()
        {
            font = Content.Load<SpriteFont>("font18");
            credits = Content.Load<Texture2D>("credits");

            font.Spacing = 2;
            font.LineSpacing = 30;
        }

        public void Update(GameTime gameTime)
        {
            block.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            tetrisWell.Draw(spriteBatch);
            block.Draw(spriteBatch);
            spriteBatch.DrawString(font, "Lines = " + lines.ToString(), new Vector2(615,20), Color.White);
            spriteBatch.DrawString(font, "Level = " + levelUp.ToString(), new Vector2(615,70), Color.White);
            spriteBatch.Draw(credits, Vector2.Zero, Color.White);
        }
    }
}
