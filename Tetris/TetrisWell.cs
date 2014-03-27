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
    class TetrisWell
    {
        public Vector2 WellPosition
        {
            get { return wellPosition; }
        }

        Level level;
        Texture2D well;
        Texture2D unit;
        Vector2 wellPosition = new Vector2(40, -150);
        Vector2 wellOrigin;
        int wellBoundary = 10;
        const int Row = 24;
        const int Col = 12;
        public int[,] wellMatrix = new int[Row, Col];
        
        public TetrisWell(Level level)
        {
            this.level = level;
            well = level.Content.Load<Texture2D>("well");
            unit = level.Content.Load<Texture2D>("unit");
            wellOrigin = new Vector2(wellBoundary, 0);
            ResetWell();
        }

        public void ResetWell()
        {
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    wellMatrix[i, j] = 0;
                }
            }
        }                

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(well,Vector2.Zero,Color.White);
            DrawWell(spriteBatch);
        }

        public void DrawWell(SpriteBatch spriteBatch)
        {
           
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    if (wellMatrix[i, j] == 1) 
                    spriteBatch.Draw(unit, wellPosition + new Vector2(j * unit.Width, i * unit.Height), Color.Gainsboro);                     
                }
            }
        }

    }
}
