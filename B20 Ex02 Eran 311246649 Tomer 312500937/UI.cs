using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02_Eran_311246649_Tomer_312500937
{
    internal class UI
    {
        private GameControl m_Controller;
        private Player m_FirstPlayer;
        private Player m_SecondPlayer;
        private eGameType m_GameType;
        private ComputerPlayer m_ComputerPlayer;
        private Dictionary<int, object> m_Collection;

        public void RunGame()
        {
            StringBuilder sb = new StringBuilder();
            bool anotherGame = true;
            initializePlayers();
            while (anotherGame)
            {
                initializeGameBoard();
                initializeCollection();
                printBoard(sb);
                startGame(sb, ref anotherGame);
            }
        }

        private void initializePlayers()
        {
            getPlayerName();

            string msg = string.Format(
@"Hey {0} please choose you preference,
Press '1' for playing against friend 
Press '2' for playing against computer.
Notice: Any time, for exit the game press 'Q'",
            m_FirstPlayer.Name);
            Console.WriteLine(msg);

            string selectionStr = Console.ReadLine();
            switch (int.Parse(selectionStr))
            {
                case (int)eGameType.againstFriend:
                    m_GameType = eGameType.againstFriend;
                    getPlayerName();
                    break;
                case (int)eGameType.againstComputer:
                    m_GameType = eGameType.againstComputer;
                    m_ComputerPlayer = new ComputerPlayer("computer");
                    break;
            }
        }

        private void getPlayerName()
        {
            Console.WriteLine("Please enter your name: ");
            string firstPlayerName = Console.ReadLine();
            if (m_FirstPlayer != null)
            {
                m_SecondPlayer = new Player(firstPlayerName);
            }
            else
            {
                m_FirstPlayer = new Player(firstPlayerName);
            }
        }

        private void initializeGameBoard()
        {
            string heightStr, widthStr;
            do
            {
                Console.WriteLine("Please enter board height:");
                do
                {
                    heightStr = Console.ReadLine();
                }
                while (!checkValidSize(heightStr));
                Console.WriteLine("Please enter board height:");
                do
                {
                    widthStr = Console.ReadLine();
                }
                while (!checkValidSize(widthStr));
            }
            while (!checkEvenNumOfCells(int.Parse(heightStr), int.Parse(widthStr)));
            m_Controller = new GameControl(int.Parse(heightStr), int.Parse(widthStr));
        }

        private bool checkValidSize(string i_Length)
        {
            int lengthNumeric;
            bool isValid = int.TryParse(i_Length, out lengthNumeric);
            if (isValid == true)
            {
                isValid = lengthNumeric >= 4 && lengthNumeric <= 6;
            }

            if (isValid == false)
            {
                Console.WriteLine("Invalid input: height or length should be 4-6");
            }

            return isValid;
        }

        private bool checkEvenNumOfCells(int i_Height, int i_Width)
        {
            bool isEven = (i_Height * i_Width) % 2 == 0;
            if (isEven == false)
            {
                Console.WriteLine("Invalid input: number of cells must be even.");
            }

            return isEven;
        }

        private void initializeCollection()
        {
            char data = 'A';
            m_Collection = new Dictionary<int, object>();
            int size = m_Controller.Board.Rows * m_Controller.Board.Cols;
            for (int i = 0; i < size; i++)
            {
                m_Collection.Add(i, data);
                data++;
            }
        }

        private void printBoard(StringBuilder i_Sb)
        {
            printColsFrame(i_Sb);
            for (int i = 0; i < m_Controller.Board.Rows; i++)
            {
                printLine(i_Sb);
                i_Sb.Clear();
                i_Sb.Append(i + 1 + " ");
                for (int j = 0; j < m_Controller.Board.Cols; j++)
                {
                    i_Sb.Append("|");
                    if (m_Controller.Board.Matrix[i, j].IsOpen)
                    {
                        int key = m_Controller.Board.Matrix[i, j].Key;
                        object data;
                        m_Collection.TryGetValue(key, out data);
                        i_Sb.Append(" " + data.ToString() + " ");
                    }
                    else
                    {
                        i_Sb.Append("   ");
                    }
                }

                i_Sb.Append("|");
                Console.WriteLine(i_Sb);
            }

            printLine(i_Sb);
        }

        private void printLine(StringBuilder i_Sb)
        {
            i_Sb.Clear();
            i_Sb.Append("  ");
            for (int i = 0; i < m_Controller.Board.Cols; i++)
            {
                i_Sb.Append("====");
            }

            Console.WriteLine(i_Sb);
        }

        private void printColsFrame(StringBuilder i_Sb)
        {
            i_Sb.Clear();
            char colSymbol = 'A';
            i_Sb.Append("    ");
            for (int i = 0; i < m_Controller.Board.Cols; i++)
            {
                i_Sb.Append(colSymbol.ToString());
                i_Sb.Append("   ");
                colSymbol++;
            }

            Console.WriteLine(i_Sb);
        }

        private void strInputToIndexes(string i_InputStr, out int o_Row, out int o_Col)
        {
            o_Col = i_InputStr[0] - 'A';
            o_Row = i_InputStr[1] - '1';
        }

        private void startGame(StringBuilder i_Sd, ref bool io_AnotherTurn)
        {
            int maxOpenPairs = (m_Controller.Board.Rows * m_Controller.Board.Cols) / 2;
            ePlayerType isTurn = ePlayerType.playerOne;
            bool hasMatch = false;
            while (m_Controller.OpenedPairs != maxOpenPairs)
            {
                makeMove(isTurn, ref hasMatch, i_Sd);
                if (!hasMatch)
                {
                    passTurn(ref isTurn);
                }
            }

            announceWinner();
            playAgain(ref io_AnotherTurn);
        }

        private void makeMove(ePlayerType i_PlayerType, ref bool io_HasMatch, StringBuilder i_Sd)
        {
            int firstRow, firstCol, secondRow, secondCol;

            getMove(out firstRow, out firstCol, i_PlayerType);
            showChoosenCellData(firstRow, firstCol, i_Sd);

            getMove(out secondRow, out secondCol, i_PlayerType);
            showChoosenCellData(secondRow, secondCol, i_Sd);

            if (isMatch(firstRow, firstCol, secondRow, secondCol))
            {
                Console.WriteLine("Match! You get another turn.");
                addScore(i_PlayerType);
                io_HasMatch = true;
            }
            else
            {
                clearUnmatchCells(firstRow, firstCol, secondRow, secondCol, i_Sd);
                io_HasMatch = false;
            }
        }

        private void getMove(out int o_Row, out int o_Col, ePlayerType i_PlayerType)
        {
            switch (i_PlayerType)
            {
                case ePlayerType.computer:
                    getComputerMove(out o_Row, out o_Col, i_PlayerType);
                    break;
                default:
                    getPlayerMove(out o_Row, out o_Col, i_PlayerType);
                    break;
            }
        }

        private void showChoosenCellData(int i_Row, int i_Col, StringBuilder i_Sd)
        {
            Ex02.ConsoleUtils.Screen.Clear();
            m_Controller.Board.Matrix[i_Row, i_Col].IsOpen = true;
            printBoard(i_Sd);
        }

        private bool isMatch(int i_FirstRow, int i_FirstCol, int i_SecondRow, int i_SecondCol)
        {
            return m_Controller.Compare(i_FirstRow, i_FirstCol, i_SecondRow, i_SecondCol);
        }

        private void addScore(ePlayerType i_PlayerType)
        {
            switch (i_PlayerType)
            {
                case ePlayerType.playerOne:
                    m_FirstPlayer.Score++;
                    break;
                case ePlayerType.playerTwo:
                    m_SecondPlayer.Score++;
                    break;
                case ePlayerType.computer:
                    m_ComputerPlayer.Score++;
                    break;
            }
        }

        private void clearUnmatchCells(int i_FirstRow, int i_FirstCol, int i_SecondRow, int i_SecondCol, StringBuilder i_Sd)
        {
            System.Threading.Thread.Sleep(2000);
            m_Controller.Board.Matrix[i_FirstRow, i_FirstCol].IsOpen = false;
            m_Controller.Board.Matrix[i_SecondRow, i_SecondCol].IsOpen = false;
            Ex02.ConsoleUtils.Screen.Clear();
            printBoard(i_Sd);
        }

        private void announceWinner()
        {
            StringBuilder winner = new StringBuilder(m_FirstPlayer.Name);
            switch (m_GameType)
            {
                case eGameType.againstComputer:
                    if (m_ComputerPlayer.Score > m_FirstPlayer.Score)
                    {
                        winner.Clear();
                        winner.Append(m_ComputerPlayer.Name);
                    }

                    break;
                default:
                    if (m_SecondPlayer.Score > m_FirstPlayer.Score)
                    {
                        winner.Clear();
                        winner.Append(m_SecondPlayer.Name);
                    }

                    break;
            }

            winner.Insert(0, "Game Over! The winner is: ");
            Console.WriteLine(winner);
        }

        private void getComputerMove(out int o_Row, out int o_Col, ePlayerType i_PlayerType)
        {
            string choiceStr;
            int NumOfRows = m_Controller.Board.Rows;
            int NumOfCols = m_Controller.Board.Cols;
            Console.WriteLine("Computer turn:");
            System.Threading.Thread.Sleep(2000);
            do
            {
                choiceStr = m_ComputerPlayer.RandMove((int)NumOfRows, (int)NumOfCols);
            }
            while (!checkValidInput(choiceStr, i_PlayerType));
            strInputToIndexes(choiceStr, out o_Row, out o_Col);
        }

        private bool checkValidInput(string i_ChoiceStr, ePlayerType i_PlayerType)
        {
            bool isValid = true;
            int chosenRow, chosenCol;
            if (i_ChoiceStr[0] == 'Q' && i_ChoiceStr.Length == 1)
            {
                Environment.Exit(1);
            }

            strInputToIndexes(i_ChoiceStr, out chosenRow, out chosenCol);
            if (i_PlayerType != ePlayerType.computer)
            {
                checkRange(chosenRow, chosenCol, ref isValid);
                if (isValid)
                {
                    if (checkBoardCellState(chosenRow, chosenCol))
                    {
                        Console.WriteLine("This cell is already open! please choose again:");
                        isValid = false;
                    }
                }
            }
            else
            {
                isValid = !checkBoardCellState(chosenRow, chosenCol);
            }

            return isValid;
        }

        private bool checkBoardCellState(int i_Row, int i_Col)
        {
            return m_Controller.Board.Matrix[i_Row, i_Col].IsOpen;
        }

        private void checkRange(int i_Row, int i_Col, ref bool io_IsValid)
        {
            if (i_Row < 0 || i_Row > m_Controller.Board.Rows - 1)
            {
                Console.WriteLine("Invalid Input: please enter row in range of the board.");
                io_IsValid = false;
            }

            if (i_Col < 0 || i_Col > m_Controller.Board.Cols - 1)
            {
                Console.WriteLine("Invalid Input: please enter letter in range of the board.");
                io_IsValid = false;
            }
        }

        private void passTurn(ref ePlayerType i_PlayerType)
        {
            switch (i_PlayerType)
            {
                case ePlayerType.playerOne:
                    if (m_SecondPlayer == null)
                    {
                        i_PlayerType = ePlayerType.computer;
                    }
                    else
                    {
                        i_PlayerType = ePlayerType.playerTwo;
                    }

                    break;
                case ePlayerType.playerTwo:
                    i_PlayerType = ePlayerType.playerOne;
                    break;
                case ePlayerType.computer:
                    i_PlayerType = ePlayerType.playerOne;
                    break;
            }
        }

        private string getCurrentPlayerName(ePlayerType i_PlayerType)
        {
            string playerName;
            switch (i_PlayerType)
            {
                case ePlayerType.playerOne:
                    playerName = m_FirstPlayer.Name;
                    break;
                case ePlayerType.playerTwo:
                    playerName = m_SecondPlayer.Name;
                    break;
                default:
                    playerName = m_ComputerPlayer.Name;
                    break;
            }

            return playerName;
        }

        private void getPlayerMove(out int o_Row, out int o_Col, ePlayerType i_PlayerType)
        {
            StringBuilder msg = new StringBuilder(" ,Which cell whould you like to expose? (etc. A2)");
            msg.Insert(0, getCurrentPlayerName(i_PlayerType));
            string choiceStr;
            Console.WriteLine(msg);
            do
            {
                choiceStr = Console.ReadLine();
            }
            while (!checkValidInput(choiceStr, i_PlayerType));
            strInputToIndexes(choiceStr, out o_Row, out o_Col);
        }

        private void playAgain(ref bool io_AnotherTurn)
        {
            string playerChoice;
            Console.WriteLine(@"Press 1 to restart game
Press Any key to end game");
            playerChoice = Console.ReadLine();
            if (playerChoice[0] == '1' && playerChoice.Length == 1)
            {
                clearPlayersScore();
                io_AnotherTurn = true;
            }
            else
            {
                io_AnotherTurn = false;
            }
        }

        private void clearPlayersScore()
        {
            m_FirstPlayer.Score = 0;
            switch (m_GameType)
            {
                case eGameType.againstComputer:
                    m_ComputerPlayer.Score = 0;
                    break;
                default:
                    m_SecondPlayer.Score = 0;
                    break;
            }
        }

        private enum eGameType
        {
            againstFriend = 1,
            againstComputer = 2
        }

        private enum ePlayerType
        {
            playerOne = 1,
            playerTwo = 2,
            computer = 3
        }
    }
}
