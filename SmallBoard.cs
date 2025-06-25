using System;
using System.Drawing;
using System.Windows.Forms;

namespace TicTacToeGame
{
    public class SmallBoard
    {
        public Button[,] Cells = new Button[3, 3];
        public Panel panel = new Panel();
        public int BoardRow { get; set; }
        public int BoardCol { get; set; }

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

        public void Disable(bool enabled)
        {
            foreach (Button b in Cells)
            {
                if (b.Text == "")
                    b.Enabled = enabled;
            }
        }

        public void SetWinner(string winner)
        {
            panel.BackColor = winner == "X" ? Color.LightCoral : Color.LightBlue;

            // disable tombol di cell ini
            foreach (Button b in Cells)
            {
                b.Enabled = false;
            }

            Label winnerLabel = new Label
            {
                Text = winner,
                Font = new Font("Arial", 36, FontStyle.Bold),
                ForeColor = winner == "X" ? Color.Red : Color.Blue,
                BackColor = Color.Transparent,
                Size = panel.Size,
                Location = new Point(0, 0),
                TextAlign = ContentAlignment.MiddleCenter
            };
            panel.Controls.Add(winnerLabel);
            winnerLabel.BringToFront();
        }

        public void SetDraw()
        {
            panel.BackColor = Color.LightGray;

            // disable
            foreach (Button b in Cells)
            {
                b.Enabled = false;
            }

            Label drawLabel = new Label
            {
                Text = "DRAW",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.DarkGray,
                BackColor = Color.Transparent,
                Size = panel.Size,
                Location = new Point(0, 0),
                TextAlign = ContentAlignment.MiddleCenter
            };
            panel.Controls.Add(drawLabel);
            drawLabel.BringToFront();
        }

        public void Reset()
        {
            for (int i = panel.Controls.Count - 1; i >= 0; i--)
            {
                if (panel.Controls[i] is Label)
                {
                    panel.Controls.RemoveAt(i);
                }
            }

            foreach (Button button in Cells)
            {
                button.Text = "";
                button.BackColor = Color.White;
                button.Enabled = true;
            }

            panel.BackColor = Color.White;
        }
    }
}