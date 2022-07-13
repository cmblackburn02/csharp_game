using System;
using static Blocks.Constants;
using static Blocks.Pieces;

namespace Blocks
{

    public class Engine
    {
        private int curRow;
        private int curCol;
        private Arr curPiece;
        private readonly Random rnd =new();
        private Arr board = new(BOARD_ROWS, BOARD_COLS);
        private Action gameOver = () => { };
        private Action dropped = () => { };
        private Action<int> linesRemoved = (rows) => { };

        public Action GameOver
        {
            set => gameOver = value;
        }

        public Action Dropped
        {
            set => dropped = value;
        }

        public Action<int> LinesRemoved
        {
            set => linesRemoved = value;
        }

        public Arr Board
        {
            get => board;
        }
// this spawns the blocks
        public void Spawn()
        {
            int which = rnd.Next(PIECES.Length);
            curCol = 4;
            curRow = 0;
            curPiece = PIECES[which].Cloned;
            if (!board.CanPlace(curPiece, curRow + 1, curCol))
            {
                gameOver();
            }
        }
// this makes it so when you press the spacebar and drops the block
        public void Down()
        {
            var clone = board.Cloned;
            clone.Remove(curPiece, curRow, curCol);
            if (clone.CanPlace(curPiece, curRow + 1, curCol))
            {
                curRow++;
                clone.Place(curPiece, curRow, curCol);
                board = clone;
            }
            else
            {
                dropped();
                var rows = board.RemoveFullRows();
                linesRemoved(rows);
                Spawn();
            }
        }
// this allows you to use the down arrow to move the block down faster if you are impatient
        public void DownMore()
        {
            var clone = board.Cloned;
            clone.Remove(curPiece, curRow, curCol);
            if (clone.CanPlace(curPiece, curRow + 1, curCol))
            {
                curRow++;
                clone.Place(curPiece, curRow, curCol);
                board = clone;
            }
        }
// this is how you can move the block to the right
        public void Right()
        {
            var clone = board.Cloned;
            clone.Remove(curPiece, curRow, curCol);
            if (clone.CanPlace(curPiece, curRow, curCol + 1))
            {
                curCol++;
                clone.Place(curPiece, curRow, curCol);
                board = clone;
            }
        }
// this allows you to move the block to the left
        public void Left()
        {
            var clone = board.Cloned;
            clone.Remove(curPiece, curRow, curCol);
            if (clone.CanPlace(curPiece, curRow, curCol - 1))
            {
                curCol--;
                clone.Place(curPiece, curRow, curCol);
                board = clone;
            }
        }
// this allows you to rotate your blocks
        public void Rotate()
        {
            var clone = board.Cloned;
            var rotated = curPiece.RotatedCounterClockwise;
            clone.Remove(curPiece, curRow, curCol);
            if (clone.CanPlace(rotated, curRow, curCol))
            {
                curPiece = rotated;
                clone.Place(curPiece, curRow, curCol);
                board = clone;
            }

        }
    }

}