using System;

namespace B20_Ex02_Eran_311246649_Tomer_312500937
{
    internal class Board
    {
        private Cell[,] m_Matrix;
        private int m_Rows;
        private int m_Cols;

        public Board(int i_Rows, int i_Cols)
        {
            m_Rows = i_Rows;
            m_Cols = i_Cols;
            m_Matrix = new Cell[m_Rows, m_Cols];
            for (int i = 0; i < i_Rows; i++)
            {
                for (int j = 0; j < i_Cols; j++)
                {
                    m_Matrix[i, j] = new Cell();
                }
            }
        }

        public int Rows
        {
            get
            {
                return m_Rows;
            }

            set
            {
                m_Rows = value;
            }
        }

        public int Cols
        {
            get
            {
                return m_Cols;
            }

            set
            {
                m_Cols = value;
            }
        }

        public Cell[,] Matrix
        {
            get
            {
                return m_Matrix;
            }

            set
            {
                m_Matrix = value;
            }
        }
    }
}
