using System;

namespace B20_Ex02_Eran_311246649_Tomer_312500937
{
    internal class Cell
    {
        private int m_Key;
        private bool m_IsOpened;

        public Cell()
        {
            m_Key = 0;
            m_IsOpened = false;
        }

        public int Key
        {
            get
            {
                return m_Key;
            }

            set
            {
                m_Key = value;
            }
        }

        public bool IsOpen
        {
            get
            {
                return m_IsOpened;
            }

            set
            {
                m_IsOpened = value;
            }
        }
    }
}
