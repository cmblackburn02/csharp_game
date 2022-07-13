using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Blocks.Constants;

namespace Blocks
{

    public class PlayScene : IScene
    {
        private Engine engine;
// the fun colors of the blocks
        private readonly Color[] palette = new Color[]
        {
            Color.Black,
            Color.Cyan,
            Color.Yellow,
            Color.Purple,
            Color.Orange,
            Color.Blue,
            Color.Green,
            Color.Red,
        };
        
        private double baseDelay;
        private double curDelay;
        private double elapsed;
        private double? timeInLevel;
        int score;
        int level = 1;
        private readonly InputManager input = new();
        private SceneManager sceneManager;
// tells the computer what to do when certain keys are pressed
        private void Reset()
        {
            baseDelay = NORMAL_DELAY;
            curDelay = baseDelay;
            engine = new();
            input.Handle(Keys.Left, () =>
            {
                engine.Left();
            });
            input.Handle(Keys.Right, () =>
            {
                engine.Right();
            });
            input.Handle(Keys.Up, () =>
            {
                engine.Rotate();
            });
            input.Handle(Keys.Down, () =>
            {
                engine.DownMore();
            });
            input.Handle(Keys.Space, () =>
            {
                curDelay = FAST_DELAY;
            });
            input.Handle(Keys.Escape, () =>
            {
                App.Instance.Exit();
            });
            engine.Spawn();
            engine.Dropped = () =>
            {
                curDelay = baseDelay;
            };
            engine.GameOver = () =>
            {
                sceneManager.SwitchScene("gameOver", new GameSummary(score, level));
            };
            engine.LinesRemoved = UpdateScore;
            engine.Down();
        }

        public PlayScene()
        {
            Reset();
        }
// this tells the user what parts of the inner diagram is in the game
//at the bottom, it shows what is on the side of the inner window
        public void Render(SpriteBatch spriteBatch)
        {
            var sx = (VIRTUAL_WIDTH - CELL * BOARD_COLS) / 2;
            var sy = (VIRTUAL_HEIGHT - CELL * (BOARD_ROWS - OVERFLOW)) / 2;
            for (int r = OVERFLOW; r < BOARD_ROWS; r++)
            {
                for (int c = 0; c < BOARD_COLS; c++)
                {
                    var rc = new Rectangle(
                        sx + c * CELL,
                        sy + (r - OVERFLOW) * CELL,
                        CELL,
                        CELL
                    );
                    var v = engine.Board[r, c];
                    spriteBatch.Draw(App.Instance.Pixel, rc, palette[v]);
                }
            }
            var font = App.Instance.Font18;
            font.DrawText(
                spriteBatch,
                $"Level: {level + 1}",
                new Vector2(16, 16),
                Color.White
                );
            font.DrawText(
                spriteBatch,
                $"Score: {score}",
                new Vector2(16, 32),
                Color.White
                );
            font.DrawText(
                spriteBatch,
                "[escape] to exit. :)",
                new Vector2(16, 48),
                Color.White
            );
        }
// this updates and adds speed to how fast the blocks fall
        public void Update(double dt)
        {
            input.Update(dt);
            elapsed += dt;
            if (elapsed >= curDelay)
            {
                elapsed = 0;
                engine.Down();
            }
            if (!timeInLevel.HasValue)
            {
                timeInLevel = dt;
            }
            else
            {
                timeInLevel += dt;
            }
            if (timeInLevel > LEVEL_SECS)
            {
                timeInLevel = null;
                level++;
                baseDelay /= SPEED_BUMP;
            }
        }
// updates the score
        private void UpdateScore(int removed)
        {
            var scoreMultipliers = new[] { 0, 40, 100, 300, 1200 };
            score += scoreMultipliers[removed] * (level + 1);

        }
        public void Enter(SceneManager manager, object _)
        {
            sceneManager = manager;
            score = 0;
            level = 0;
            Reset();
        }
        public readonly InputManager inputManager = new();
        public void Leave()
        {
        }

    }
}