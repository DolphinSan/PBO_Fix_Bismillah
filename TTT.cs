using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace TicTacToeGame
{
    public class TicTacToeForm : Form
    {
        private Button[,] buttons = new Button[3, 3];
        private bool isPlayerX = true;
        private int moveCount = 0;
        private Label statusLabel;
        private Button resetButton;

        private SmallBoard[,] boards = new SmallBoard[3, 3];
        public TicTacToeForm()
        {
            InitializeForm();
            CreateControls();
        }

        private void InitializeForm()
        {
            this.Text = "Tic Tac Toe";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.WhiteSmoke;
        }

        private void CreateControls()
        {
            statusLabel = new Label
            {
                Text = "Player X Turn",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Size = new Size(300, 30),
                Location = new Point(50, 20),
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
                Location = new Point(150, 420),
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

                    int posX = startX + col * (cellSize * 3 + spacing);
                    int posY = startY + row * (cellSize * 3 + spacing);

                    board.panel.Location = new Point(posX, posY);
                    this.Controls.Add(board.panel);
                }
            }
        }

        private void OnCellClick(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null || btn.Text != "") return;

            btn.Text = isPlayerX ? "X" : "O";
            btn.ForeColor = isPlayerX ? Color.Red : Color.Blue;
            btn.Enabled = false;
            btn.BackColor = Color.LightGray;

            moveCount++;

            isPlayerX = !isPlayerX;
            statusLabel.Text = $"Player {(isPlayerX ? "X" : "O")} Turn";
            statusLabel.ForeColor = Color.Black;
        }

        private bool CheckForWinner()
        {
            for (int row = 0; row < 3; row++)
            {
                if (IsWinningLine(buttons[row, 0], buttons[row, 1], buttons[row, 2]))
                {
                    HighlightWinningButtons(buttons[row, 0], buttons[row, 1], buttons[row, 2]);
                    return true;
                }
            }

            for (int col = 0; col < 3; col++)
            {
                if (IsWinningLine(buttons[0, col], buttons[1, col], buttons[2, col]))
                {
                    HighlightWinningButtons(buttons[0, col], buttons[1, col], buttons[2, col]);
                    return true;
                }
            }

            if (IsWinningLine(buttons[0, 0], buttons[1, 1], buttons[2, 2]))
            {
                HighlightWinningButtons(buttons[0, 0], buttons[1, 1], buttons[2, 2]);
                return true;
            }

            if (IsWinningLine(buttons[0, 2], buttons[1, 1], buttons[2, 0]))
            {
                HighlightWinningButtons(buttons[0, 2], buttons[1, 1], buttons[2, 0]);
                return true;
            }

            return false;
        }

        private bool IsWinningLine(Button b1, Button b2, Button b3)
        {
            return b1.Text != "" && b1.Text == b2.Text && b2.Text == b3.Text;
        }

        private void HighlightWinningButtons(Button b1, Button b2, Button b3)
        {
            b1.BackColor = Color.LightGreen;
            b2.BackColor = Color.LightGreen;
            b3.BackColor = Color.LightGreen;
        }

        private void DisableAllButtons()
        {
            foreach (Button button in buttons)
            {
                button.Enabled = false;
            }
        }

        private void ResetGame(object sender, EventArgs e)
        {
            foreach (Button button in buttons)
            {
                button.Text = "";
                button.BackColor = Color.White;
                button.Enabled = true;
            }

            isPlayerX = true;
            moveCount = 0;
            statusLabel.Text = "Player X Turn";
            statusLabel.ForeColor = Color.Black;
        }
    }
    public class SmallBoard
    {
        public Button[,] Cells = new Button[3, 3];
        public Panel panel = new Panel();

        public SmallBoard(int CellSize, EventHandler onClick)
        {
            panel.Size = new Size(CellSize * 3, CellSize * 3);
            panel.BorderStyle = BorderStyle.FixedSingle;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Button button = new Button
                    {
                        Size = new Size(CellSize, CellSize),
                        Location = new Point(col * CellSize, row * CellSize),
                        Font = new Font("Arial", 10, FontStyle.Bold),
                        BackColor = Color.White,
                        Text = "",
                        Tag = new Tuple<int, int>(row, col)
                    };
                    button.Click += onClick;
                    panel.Controls.Add(button);
                    Cells[row, col] = button;
                }
            }
        }
        public void Matikan(bool enabled)
        {
            foreach (Button b in Cells)
            {
                if (b.Text == "")
                    b.Enabled = enabled;
            }
        }
    }
}