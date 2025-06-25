using System;
using System.Drawing;
using System.Windows.Forms;

namespace TicTacToeGame
{
    public class TicTacToeForm : Form
    {
        private bool isPlayerX = true;
        private int moveCount = 0;
        private Label statusLabel;
        private Button resetButton;
        private SmallBoard[,] boards = new SmallBoard[3, 3];
        private string[,] bigBoardWinners = new string[3, 3]; // buat ngetrack nanti pemenang nya siapa
        private int activeRow = -1;
        private int activeCol = -1;
        private bool gameOver = false;

        public TicTacToeForm()
        {
            InitializeForm();
            CreateControls();
        }

        private void InitializeForm()
        {
            this.Text = "Nested Tic Tac Toe";
            this.Size = new Size(440, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.WhiteSmoke;
        }

        private void CreateControls()
        {
            statusLabel = new Label
            {
                Text = "Player X Turn - Click any square to start",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Size = new Size(400, 30),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            this.Controls.Add(statusLabel);

            CreateGameBoard();

            resetButton = new Button
            {
                Text = "Play Again",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Size = new Size(100, 40),
                Location = new Point(160, 450),
                BackColor = Color.LightBlue,
                UseVisualStyleBackColor = false
            };
            resetButton.Click += ResetGame;
            this.Controls.Add(resetButton);
        }

        private void CreateGameBoard()
        {
            int cellSize = 40;
            int spacing = 10;
            int startX = 20;
            int startY = 60;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    SmallBoard board = new SmallBoard(cellSize, OnCellClick);
                    boards[row, col] = board;
                    bigBoardWinners[row, col] = "";

                    int posX = startX + col * (cellSize * 3 + spacing);
                    int posY = startY + row * (cellSize * 3 + spacing);

                    board.panel.Location = new Point(posX, posY);
                    board.BoardRow = row;
                    board.BoardCol = col;
                    this.Controls.Add(board.panel);
                }
            }
        }

        private void OnCellClick(object sender, EventArgs e)
        {
            if (gameOver) return;

            Button btn = sender as Button;
            if (btn == null || btn.Text != "") return;

            var cellPos = (Tuple<int, int>)btn.Tag;
            int cellRow = cellPos.Item1;
            int cellCol = cellPos.Item2;

            SmallBoard clickedBoard = null;
            int boardRow = -1, boardCol = -1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (boards[i, j].Cells[cellRow, cellCol] == btn)
                    {
                        clickedBoard = boards[i, j];
                        boardRow = i;
                        boardCol = j;
                        break;
                    }
                }
                if (clickedBoard != null) break;
            }

            if (activeRow != -1 && activeCol != -1)
            {
                if (boardRow != activeRow || boardCol != activeCol)
                {
                    return;
                }
            }

            btn.Text = isPlayerX ? "X" : "O";
            btn.ForeColor = isPlayerX ? Color.Red : Color.Blue;
            btn.Enabled = false;
            btn.BackColor = Color.LightGray;

            moveCount++;

            string winner = CheckSmallBoardWinner(clickedBoard);
            if (winner != "")
            {
                bigBoardWinners[boardRow, boardCol] = winner;
                clickedBoard.SetWinner(winner);
            }

            string bigWinner = CheckBigBoardWinner();
            if (bigWinner != "")
            {
                statusLabel.Text = $"Player {bigWinner} Wins the Game!";
                statusLabel.ForeColor = Color.Green;
                gameOver = true;
                DisableAllBoards();
                return;
            }

            if (moveCount >= 81 || IsBigBoardFull())
            {
                statusLabel.Text = "It's a Tie!";
                statusLabel.ForeColor = Color.Orange;
                gameOver = true;
                return;
            }

            // MAIN CORE DARI NESTED TTT
            if (bigBoardWinners[cellRow, cellCol] == "")
            {
                // harus main di board yang sama
                activeRow = cellRow;
                activeCol = cellCol;
                UpdateBoardHighlights();
            }
            else
            {
                // mainin buat board lain yang kosong, yang skrng udah menang soalnya
                activeRow = -1;
                activeCol = -1;
                UpdateBoardHighlights();
            }

            isPlayerX = !isPlayerX;
            UpdateStatusLabel();
        }

        private bool IsSmallBoardFull(SmallBoard board)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board.Cells[row, col].Text == "")
                        return false;
                }
            }
            return true;
        }

        private void UpdateStatusLabel()
        {
            if (activeRow == -1 && activeCol == -1)
            {
                statusLabel.Text = $"Player {(isPlayerX ? "X" : "O")} Turn - Choose any available board";
            }
            else
            {
                statusLabel.Text = $"Player {(isPlayerX ? "X" : "O")} Turn - Must play in highlighted board";
            }
            statusLabel.ForeColor = Color.Black;
        }

        private void UpdateBoardHighlights()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (activeRow == -1 && activeCol == -1)
                    {
                        if (bigBoardWinners[row, col] == "")
                        {
                            boards[row, col].panel.BackColor = Color.LightYellow;
                            boards[row, col].Disable(true);
                        }
                        else
                        {
                            boards[row, col].panel.BackColor = Color.LightGray;
                            boards[row, col].Disable(false);
                        }
                    }
                    else
                    {
                        // buat nge spesifik board
                        if (row == activeRow && col == activeCol && bigBoardWinners[row, col] == "")
                        {
                            boards[row, col].panel.BackColor = Color.LightGreen;
                            boards[row, col].Disable(true);
                        }
                        else
                        {
                            boards[row, col].panel.BackColor = Color.LightGray;
                            boards[row, col].Disable(false);
                        }
                    }
                }
            }
        }

        private string CheckSmallBoardWinner(SmallBoard board)
        {
            Button[,] cells = board.Cells;

            for (int row = 0; row < 3; row++)
            {
                if (IsWinningLine(cells[row, 0], cells[row, 1], cells[row, 2]))
                {
                    return cells[row, 0].Text;
                }
            }

            for (int col = 0; col < 3; col++)
            {
                if (IsWinningLine(cells[0, col], cells[1, col], cells[2, col]))
                {
                    return cells[0, col].Text;
                }
            }

            if (IsWinningLine(cells[0, 0], cells[1, 1], cells[2, 2]))
            {
                return cells[0, 0].Text;
            }

            if (IsWinningLine(cells[0, 2], cells[1, 1], cells[2, 0]))
            {
                return cells[0, 2].Text;
            }

            return "";
        }

        private string CheckBigBoardWinner()
        {
            for (int row = 0; row < 3; row++)
            {
                if (bigBoardWinners[row, 0] != "" &&
                    bigBoardWinners[row, 0] == bigBoardWinners[row, 1] &&
                    bigBoardWinners[row, 1] == bigBoardWinners[row, 2])
                {
                    return bigBoardWinners[row, 0];
                }
            }

            for (int col = 0; col < 3; col++)
            {
                if (bigBoardWinners[0, col] != "" &&
                    bigBoardWinners[0, col] == bigBoardWinners[1, col] &&
                    bigBoardWinners[1, col] == bigBoardWinners[2, col])
                {
                    return bigBoardWinners[0, col];
                }
            }

            if (bigBoardWinners[0, 0] != "" &&
                bigBoardWinners[0, 0] == bigBoardWinners[1, 1] &&
                bigBoardWinners[1, 1] == bigBoardWinners[2, 2])
            {
                return bigBoardWinners[0, 0];
            }

            if (bigBoardWinners[0, 2] != "" &&
                bigBoardWinners[0, 2] == bigBoardWinners[1, 1] &&
                bigBoardWinners[1, 1] == bigBoardWinners[2, 0])
            {
                return bigBoardWinners[0, 2];
            }

            return "";
        }

        private bool IsWinningLine(Button b1, Button b2, Button b3)
        {
            return b1.Text != "" && b1.Text == b2.Text && b2.Text == b3.Text;
        }

        private bool IsBigBoardFull()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (bigBoardWinners[row, col] == "")
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                if (boards[row, col].Cells[i, j].Text == "")
                                    return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        private void DisableAllBoards()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    boards[row, col].Disable(false);
                }
            }
        }

        private void ResetGame(object sender, EventArgs e)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    boards[row, col].Reset();
                    bigBoardWinners[row, col] = "";
                }
            }

            isPlayerX = true;
            moveCount = 0;
            activeRow = -1;
            activeCol = -1;
            gameOver = false;
            statusLabel.Text = "Player X Turn - Click any square to start";
            statusLabel.ForeColor = Color.Black;
            UpdateBoardHighlights();
        }
    }
}