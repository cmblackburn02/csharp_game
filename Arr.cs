namespace Blocks
{
    // this class is here to represent a huge array
    public class Arr
    {
        private readonly int[] buf;
        private readonly int rows;
        private readonly int cols;

        public int Rows
        {
            get => rows;
        }

        public int Cols
        {
            get => cols;
        }

        public int this[int r, int c]
        {
            get => buf[cols * r + c];
            set => buf[cols * r + c] = value;
        }

        public Arr(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            buf = new int[this.rows * this.cols];
        }

        public Arr(int[] source, int rows)
        {
            this.rows = rows;
            cols = source.Length / this.rows;
            buf = new int[this.rows * cols];
            source.CopyTo(buf, 0);
        }

// tells the computer if the block can be placed
        public bool CanPlace(Arr piece, int r, int c)
        {
            for (int y = 0; y < piece.Rows; y++)
            {
                for (int x = 0; x < piece.Cols; x++)
                {
                    int p = piece[y, x];
                    if (p == 0)
                    {
                        continue;
                    }
                    int br = r + y;
                    int bc = c + x;
                    if (br < 0 || br >= rows)
                    {
                        return false;
                    }
                    if (bc < 0 || bc >= cols)
                    {
                        return false;
                    }
                    if (this[br, bc] != 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
// this places the block
        public void Place(Arr piece, int r, int c)
        {
            for (int y = 0; y < piece.Rows; y++)
            {
                for (int x = 0; x < piece.Cols; x++)
                {

                    int p = piece[y, x];
                    if (p != 0)
                    {
                        this[r + y, c + x] = p;
                    }

                }
            }
        }

        public Arr Cloned
        {
            get => new(buf, rows);
        }
// decides if we can remove a row
        public void Remove(Arr piece, int r, int c)
        {
            for (int y = 0; y < piece.Rows; y++)
            {
                for (int x = 0; x < piece.Cols; x++)
                {
                    if (piece[y, x] != 0)
                    {
                        this[r + y, c + x] = 0;
                    }
                }
            }
        }

// this makes it so we can rotate the blocks clockwise
        public Arr RotatedClockwise
        {
            get
            {
                var dst = new Arr(cols, rows);
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        int v = this[r, c];
                        int dr = c;
                        int dc = rows - 1 - r;
                        dst[dr, dc] = v;
                    }
                }
                return dst;
            }
        }
// this makes it so we can rotate the blocks counter clockwise
        public Arr RotatedCounterClockwise
        {
            get
            {
                var dst = new Arr(cols, rows);
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        int v = this[r, c];
                        int dr = cols - 1 - c;
                        int dc = r;
                        dst[dr, dc] = v;
                    }
                }
                return dst;
            }
        }
// this determines if the row is full
        private bool IsRowFull(int row)
        {
            for (int c = 0; c < cols; ++c)
            {
                if (this[row, c] == 0)
                {
                    return false;
                }
            }
            return true;
        }
        private void ShiftRow(int row)
        {
            for (int c = 0; c < cols; ++c)
            {
                this[row + 1, c] = this[row, c];
            }
        }
// this clears the row
        private void ClearRow(int row)
        {
            for (int c = 0; c < cols; ++c)
            {
                this[row, c] = 0;
            }
        }
// this removes the row
        private void RemoveRow(int row)
        {
            for (int r = row; r > 0; r--)
            {
                ShiftRow(r - 1);
            }
            ClearRow(0);
        }
//this removes a full row
        public int RemoveFullRows()
        {
            int removed = 0;
            for (int row = rows - 1; row > 0; row--)
            {
                if (IsRowFull(row))
                {
                    RemoveRow(row);
                    row++;
                    removed++;
                }
            }

            return removed;
        }
    }
}