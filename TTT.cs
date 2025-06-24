using System;
using System.Drawing;
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
            int buttonSize = 80;
            int startX = 80;
            int startY = 80;
            int gap = 5;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    buttons[row, col] = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(
                            startX + col * (buttonSize + gap),
                            startY + row * (buttonSize + gap)
                        ),
                        Font = new Font("Arial", 24, FontStyle.Bold),
                        BackColor = Color.White,
                        UseVisualStyleBackColor = false,
                        FlatStyle = FlatStyle.Flat,
                        Text = "",
                        Tag = new Point(row, col)
                    };
                    
                    buttons[row, col].FlatAppearance.BorderSize = 2;
                    buttons[row, col].FlatAppearance.BorderColor = Color.Black;
                    buttons[row, col].Click += OnCellClick;
                    this.Controls.Add(buttons[row, col]);
                }
            }
        }

        private void OnCellClick(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null || button.Text != "")
                return;

            button.Text = isPlayerX ? "X" : "O";
            button.ForeColor = isPlayerX ? Color.Red : Color.Blue;
            button.BackColor = Color.LightGray;
            
            moveCount++;

            if (CheckForWinner())
            {
                string winner = isPlayerX ? "X" : "O";
                statusLabel.Text = $"PLayer {winner} Win";
                statusLabel.ForeColor = Color.Green;
                DisableAllButtons();
                return;
            }

            if (moveCount == 9)
            {
                statusLabel.Text = "Draw";
                statusLabel.ForeColor = Color.Orange;
                return;
            }

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
}