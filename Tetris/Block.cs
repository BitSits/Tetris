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
    class Block
    {
        // Variables for audio.
        private static SoundEffect soundBlockRotate;
        private static SoundEffect soundBlockMoveLeft;
        private static SoundEffect soundBlockMoveRight;
        private static SoundEffect soundGameOver;
        public static SoundEffect soundLineKill;

        //TetrisWell variables
        Vector2 wellPosition;

        Texture2D unit;
        Texture2D nextBoard;
        Color[] shapeColor;
        private int[][][,] shapes = new int[7][][,];
        Vector2 StartPosition, NextPosition = new Vector2(430, 20);
        private int row, col;
        private int currentShapeIndex, nextShapeIndex;
        private int currentRotation, nextRotation, prevRotation;
        private Random random = new Random();

        float pressedLeft = 0.0f;
        float pressedRight = 0.0f;
        float pressedDown = 0.0f;
        KeyboardState currentKb, previousKb;

        Vector2 position, previousPosition;
        int x, y;
        float timeInterval = 1.0f;
        float fallTime = 0.0f;
        Level level;

        public Block(Level level, Vector2 wellPosition)
        {
            this.wellPosition = wellPosition;
            this.level = level;
            unit = level.Content.Load<Texture2D>("unit");
            nextBoard = level.Content.Load<Texture2D>("nextBoard");
            soundBlockRotate = level.Content.Load<SoundEffect>("Audio/blockRotate");
            soundBlockMoveLeft = level.Content.Load<SoundEffect>("Audio/blockMoveLeft");
            soundBlockMoveRight = level.Content.Load<SoundEffect>("Audio/blockMoveRight");
            soundLineKill = level.Content.Load<SoundEffect>("Audio/lineKill");
            soundGameOver = level.Content.Load<SoundEffect>("Audio/gameOver");

            shapeColor = new Color[7];
            StartPosition = wellPosition + new Vector2(3 * unit.Width, 0);
            DefineShape();
            GetNewShape();
        }

        public void DefineShape()
        {
            #region Square
            shapes[0] = new int[1][,];
            shapes[0][0] = new int[5, 5] { {0,0,0,0,0},
                                           {0,1,1,0,0},
                                           {0,1,1,0,0},
                                           {0,0,0,0,0},
                                           {0,0,0,0,0}};
            shapeColor[0] = Color.Red;

            #endregion

            #region iShape
            shapes[1] = new int[2][,];
            shapes[1][0] = new int[5, 5] { {0,0,0,0,0},
                                           {0,0,0,0,0},
                                           {0,1,1,1,1},
                                           {0,0,0,0,0},
                                           {0,0,0,0,0}};

            shapes[1][1] = new int[5, 5] { {0,0,0,0,0},
                                           {0,0,1,0,0},
                                           {0,0,1,0,0},
                                           {0,0,1,0,0},
                                           {0,0,1,0,0}};
            shapeColor[1] = Color.Blue;
            #endregion

            #region nShape
            shapes[2] = new int[2][,];
            shapes[2][0] = new int[5, 5] { {0,0,0,0,0},
                                           {0,1,0,0,0},
                                           {0,1,1,0,0},
                                           {0,0,1,0,0},
                                           {0,0,0,0,0}};

            shapes[2][1] = new int[5, 5] { {0,0,0,0,0},
                                           {0,0,1,1,0},
                                           {0,1,1,0,0},
                                           {0,0,0,0,0},
                                           {0,0,0,0,0}};
            shapeColor[2] = Color.LawnGreen;
            #endregion

            #region jShape

            shapes[3] = new int[4][,];
            shapes[3][0] = new int[5, 5] { {0,0,0,0,0},
                                           {0,1,0,0,0},
                                           {0,1,1,1,0},
                                           {0,0,0,0,0},
                                           {0,0,0,0,0}};

            shapes[3][1] = new int[5, 5] { {0,0,0,0,0},
                                           {0,1,1,0,0},
                                           {0,1,0,0,0},
                                           {0,1,0,0,0},
                                           {0,0,0,0,0}};
            shapes[3][2] = new int[5, 5] { {0,0,0,0,0},
                                           {0,1,1,1,0},
                                           {0,0,0,1,0},
                                           {0,0,0,0,0},
                                           {0,0,0,0,0}};

            shapes[3][3] = new int[5, 5] { {0,0,0,0,0},
                                           {0,0,0,1,0},
                                           {0,0,0,1,0},
                                           {0,0,1,1,0},
                                           {0,0,0,0,0}};
            shapeColor[3] = Color.Orange;
            #endregion

            #region tShape
            shapes[4] = new int[4][,];
            shapes[4][0] = new int[5, 5] { {0,0,0,0,0},
                                           {0,0,1,0,0},
                                           {0,1,1,0,0},
                                           {0,0,1,0,0},
                                           {0,0,0,0,0}};

            shapes[4][1] = new int[5, 5] { {0,0,0,0,0},
                                           {0,0,1,0,0},
                                           {0,1,1,1,0},
                                           {0,0,0,0,0},
                                           {0,0,0,0,0}};

            shapes[4][2] = new int[5, 5] { {0,0,0,0,0},
                                           {0,0,1,0,0},
                                           {0,0,1,1,0},
                                           {0,0,1,0,0},
                                           {0,0,0,0,0}};

            shapes[4][3] = new int[5, 5] { {0,0,0,0,0},
                                           {0,0,0,0,0},
                                           {0,1,1,1,0},
                                           {0,0,1,0,0},
                                           {0,0,0,0,0}};
            shapeColor[4] = Color.BlueViolet;
            #endregion

            #region nMirrorShape
            shapes[5] = new int[2][,];
            shapes[5][0] = new int[5, 5] { {0,0,0,0,0},
                                           {0,0,1,0,0},
                                           {0,1,1,0,0},
                                           {0,1,0,0,0},
                                           {0,0,0,0,0}};

            shapes[5][1] = new int[5, 5] { {0,0,0,0,0},
                                           {1,1,0,0,0},
                                           {0,1,1,0,0},
                                           {0,0,0,0,0},
                                           {0,0,0,0,0}};
            shapeColor[5] = Color.Violet;
            #endregion

            #region lShape
            shapes[6] = new int[4][,];
            shapes[6][0] = new int[5, 5] { {0,0,0,0,0},
                                           {0,0,0,1,0},
                                           {0,1,1,1,0},
                                           {0,0,0,0,0},
                                           {0,0,0,0,0}};

            shapes[6][1] = new int[5, 5] { {0,0,0,0,0},
                                           {0,0,1,1,0},
                                           {0,0,0,1,0},
                                           {0,0,0,1,0},
                                           {0,0,0,0,0}};
            shapes[6][2] = new int[5, 5] { {0,0,0,0,0},
                                           {0,1,1,1,0},
                                           {0,1,0,0,0},
                                           {0,0,0,0,0},
                                           {0,0,0,0,0}};

            shapes[6][3] = new int[5, 5] { {0,0,0,0,0},
                                           {0,1,0,0,0},
                                           {0,1,0,0,0},
                                           {0,1,1,0,0},
                                           {0,0,0,0,0}};
            shapeColor[6] = Color.Yellow;
            #endregion

        }

        public void GetNewShape()
        {
            position = StartPosition;
            x = 4; y =0 ;
            currentShapeIndex = nextShapeIndex;
            currentRotation = nextRotation;
            nextShapeIndex = random.Next(7);
            nextRotation = random.Next(shapes[nextShapeIndex].Length);
        }

        public void Rotation()
        {
            currentRotation += 1;
            if (currentRotation >= shapes[currentShapeIndex].Length) currentRotation = 0;
        }

        public void CheckCollision(GameTime gameTime)
        {
            bool canFix = false;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (shapes[currentShapeIndex][currentRotation][i, j] == 1)   
                    {
                        col=x+j;
                        row=y+i;
                        if (col < 0) x += -(int)col;
                        if (col >level.WellMatrix.GetLength(1) - 1)
                            x += -col + level.WellMatrix.GetLength(1) - 1;
                        if (row > level.WellMatrix.GetLength(0) - 1)
                        {
                            y -= row - (level.WellMatrix.GetLength(0) - 1);
                            canFix = true;
                        }
                        if (row >= 0 && row < level.WellMatrix.GetLength(0) &&
                           col >= 0 && col < level.WellMatrix.GetLength(1))
                        {
                            if (level.WellMatrix[row, col] == 1)
                            {
                                if (previousPosition.X > x) x += 1;
                                else if (previousPosition.X < x) x -= 1;
                                else if (previousPosition.Y < y)
                                {
                                    y -= 1;
                                    canFix = true;
                                }

                            }
                        }

                    }
                }
            }
            CheckOverlap();

            if (canFix) fixIt(gameTime);
            LinesCompletd(gameTime);

            previousPosition.X = x;
            previousPosition.Y = y;

            prevRotation = currentRotation;
          
        }

        private void CheckOverlap()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (shapes[currentShapeIndex][currentRotation][i, j] == 1)
                    {
                        row = y + i;
                        col = x + j;

                        if (col < 0 || col > level.WellMatrix.GetLength(1) - 1 ||
                            level.WellMatrix[row, col] == 1)
                        {
                            x = (int)previousPosition.X;

                            currentRotation -= 1;
                            if (currentRotation < 0)
                                currentRotation = shapes[currentShapeIndex].GetLength(0) - 1;

                            return;
                        }
                    }
                }
            }
        }

        private void fixIt(GameTime gameTime)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (shapes[currentShapeIndex][currentRotation][i, j] == 1)
                    {
                        level.WellMatrix[y + i, x + j] = 1;
                    }
                }
            }
            checkGameOver();
            GetNewShape();
        }

        private bool LinesCompletd(GameTime gametime)
        {
           
            for (int i = level.WellMatrix.GetLength(0) - 1; i >= 4; i--)
            {
                bool IslineCompleted = true;
                for (int j = 0; j < level.WellMatrix.GetLength(1); j++)
                {
                    if (level.WellMatrix[i, j] == 0) IslineCompleted = false;
                }
                if (IslineCompleted == true)
                {

                    level.lines += 1;
                    soundLineKill.Play();
                    level.linesCount += 1;
                    for (int k = i; k >= 4; k--)
                    {
                        for (int j = 0; j < level.WellMatrix.GetLength(1); j++)
                        {
                            level.WellMatrix[k, j] = level.WellMatrix[k - 1, j];
                        }
                    }
                    if (level.linesCount >= 10)
                    {
                        level.levelUp += 1;
                        level.linesCount = 0;
                        timeInterval -= 0.05f;
                    }
                    return true;
                }
            }
            return false;
        }

        private void checkGameOver()
        {
            for (int i = 0; i < level.WellMatrix.GetLength(1); i++)
            {
                if (level.WellMatrix[5, i] == 1) level.isGameOver = true; 
            }

            if (level.isGameOver)
            {
                soundGameOver.Play();
                gameOverScreen();               
            }

        }

        public void gameOverScreen()
        {
            for (int i = 0; i < level.WellMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < level.WellMatrix.GetLength(1); j++)
                    level.WellMatrix[i, j] = 1;
            }            
        }

        public void Update(GameTime gameTime)
        {

            currentKb = Keyboard.GetState();
            fallTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (fallTime>=timeInterval)
            {
                y += 1;
                fallTime = 0.0f;
            }

            pressedLeft += (float)gameTime.ElapsedGameTime.TotalSeconds;
            pressedRight += (float)gameTime.ElapsedGameTime.TotalSeconds;
            pressedDown += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentKb.IsKeyDown(Keys.Left) && previousKb.IsKeyDown(Keys.Left))
            {
                if (pressedLeft > 0.1f)
                {
                    x -= 1;
                    pressedLeft = 0.0f;
                }
            }
    
            else if (currentKb.IsKeyDown(Keys.Right) && previousKb.IsKeyDown(Keys.Right))
            {
                if (pressedRight > 0.1f)
                {
                    x += 1;
                    pressedRight = 0.0f;
                }
            }

            if (currentKb.IsKeyDown(Keys.Down) && previousKb.IsKeyDown(Keys.Down))
            {
                if (pressedDown > 0.1f)
                {
                    y += 1;
                    pressedDown = 0.0f;
                }
            }
            if (currentKb.IsKeyDown(Keys.Left) && previousKb.IsKeyUp(Keys.Left))
            {
                soundBlockMoveLeft.Play();
            }
            if (currentKb.IsKeyDown(Keys.Right) && previousKb.IsKeyUp(Keys.Right))
            {
                soundBlockMoveRight.Play();

            }
            if (currentKb.IsKeyDown(Keys.Up) && previousKb.IsKeyUp(Keys.Up))
            {
                soundBlockRotate.Play();
            }

            if (currentKb.IsKeyDown(Keys.Up) && previousKb.IsKeyUp(Keys.Up))
            {
                Rotation();
            }

            previousKb = currentKb;
            CheckCollision(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (shapes[currentShapeIndex][currentRotation][i, j] == 1)
                        spriteBatch.Draw(unit, wellPosition + new Vector2((x + j) * unit.Width, (y + i) * unit.Height),
                            shapeColor[currentShapeIndex]);
                    if (shapes[nextShapeIndex][nextRotation][i, j] == 1)
                        spriteBatch.Draw(unit, NextPosition + new Vector2(j * unit.Width, i * unit.Height), shapeColor[nextShapeIndex]);        
                }
            }
        }
    }
}
