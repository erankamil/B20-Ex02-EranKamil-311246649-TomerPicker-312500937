namespace B20_Ex02_Eran_311246649_Tomer_312500937
{
    internal class Player
    {
        private string m_Name;
        private int m_Score;

        public Player(string i_Name)
        {
            m_Name = i_Name;
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
    }
}
