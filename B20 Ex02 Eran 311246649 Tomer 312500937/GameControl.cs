using System;
using System.Collections.Generic;

namespace B20_Ex02_Eran_311246649_Tomer_312500937
{
    internal class GameControl
    {
        private Board m_GameBoard;
        private int m_NumOfOpenedPairs;

        public GameControl(int i_Rows, int i_Cols)
        {
            m_GameBoard = new Board(i_Rows, i_Cols);
            initialize();
            mix();
        }

        public Board Board
        {
            get
            {
                return m_GameBoard;
            }
        }

        public int OpenedPairs
        {
            get
            {
                return m_NumOfOpenedPairs;
            }
        }

        public void mix()
        {
            Random rand = new Random();
            int randRow;
            int randCol;
            for (int i = 0; i < m_GameBoard.Rows; i++)
            {
                for (int j = 0; j < m_GameBoard.Cols; j++)
                {
                    randRow = rand.Next(0, (int)m_GameBoard.Rows);
                    randCol = rand.Next(0, (int)m_GameBoard.Cols);
                    int temp = m_GameBoard.Matrix[i, j].Key;
                    m_GameBoard.Matrix[i, j].Key = m_GameBoard.Matrix[randRow, randCol].Key;
                    m_GameBoard.Matrix[randRow, randCol].Key = temp;
                }
            }
        }

        private void initialize()
        {
            int currentNum = 0;
            for (int i = 0; i < m_GameBoard.Rows; i++)
            {
                for (int j = 0; j < m_GameBoard.Cols; j++)
                {
                    m_GameBoard.Matrix[i, j].Key = currentNum;
                    if (j % 2 == 1)
                    {
                        currentNum++;
                    }
                }
            }
        }

        public bool Compare(int i_FirstCellRow, int i_FirstCellCol, int i_SecondCellRow, int i_SecondCellCol)
        {
            bool isMatch = m_GameBoard.Matrix[i_FirstCellRow, i_FirstCellCol].Key ==
                m_GameBoard.Matrix[i_SecondCellRow, i_SecondCellCol].Key;

            if (isMatch)
            {
                m_NumOfOpenedPairs++;
            }

            return isMatch;
        }
    }
}
