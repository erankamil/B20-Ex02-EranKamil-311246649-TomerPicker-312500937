using System;
using System.Collections.Generic;

namespace B20_Ex02_Eran_311246649_Tomer_312500937
{
    internal class ComputerPlayer
    {
        private int m_Score;
        private string m_Name;
        private Random m_Rand;

        public ComputerPlayer(string i_Name)
        {
            m_Name = i_Name;
            m_Rand = new Random();
        }

        public string Name
        {
            get
            {
                return m_Name;
            }

            set
            {
                m_Name = value;
            }
        }

        public int Score
        {
            get
            {
                return m_Score;
            }

            set
            {
                m_Score = value;
            }
        }

        public string RandMove(int i_Row, int i_Col)
        {
            int row = m_Rand.Next(1, i_Row);
            char col = Convert.ToChar(m_Rand.Next(1, i_Col) + 'A');
            string choice = string.Format("{0}{1}", col, row);
            return choice;
        }
    }
}