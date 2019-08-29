using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightsOut
{
    public partial class MainForm : Form {

        private const int GridOffset = 25;     // Distance from upper-left side of window
        private int gridLength = 200;           // Size in pixels of grid
        private LightsOutGame game;

        public MainForm()
        {
            InitializeComponent();

            game = new LightsOutGame();
            game.NewGame();

        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int cellLength = gridLength / game.GridSize;

            for (int r = 0; r < game.GridSize; r++)
                for (int c = 0; c < game.GridSize; c++)
                {
                    // Get proper pen and brush for on/off grid section
                    Brush brush;
                    Pen pen;

                    if (game.GetGridValue(r, c))
                    {
                        pen = Pens.Black;
                        brush = Brushes.White;	// On
                    }
                    else
                    {
                        pen = Pens.White;
                        brush = Brushes.Black;	// Off
                    }

                    // Determine (x,y) coord of row and col to draw rectangle                    
                    int x = c * cellLength + GridOffset;
                    int y = r * cellLength + GridOffset;

                    // Draw outline and inner rectangle
                    g.DrawRectangle(pen, x, y, cellLength, cellLength);
                    g.FillRectangle(brush, x + 1, y + 1, cellLength - 1, cellLength - 1);
                }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            int cellLength = gridLength / game.GridSize;

            // Make sure click was inside the grid
            if (e.X < GridOffset || e.X > cellLength * game.GridSize + GridOffset ||
                e.Y < GridOffset || e.Y > cellLength * game.GridSize + GridOffset)
                return;

            // Find row, col of mouse press
            int r = (e.Y - GridOffset) / cellLength;
            int c = (e.X - GridOffset) / cellLength;

            game.Move(r, c);

            // Redraw grid
            this.Invalidate();

            // Check to see if puzzle has been solved
            if (game.IsGameOver())
            {
                // Display winner dialog box just inside window
                MessageBox.Show(this, "Congratulations!  You've won!", "Lights Out!",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void NewGameButton_Click(object sender, EventArgs e)
        {
            game.NewGame();

            // Redraw the grid
            Invalidate();
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGameButton_Click(sender, e);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutBox = new AboutForm();
            aboutBox.ShowDialog(this);
        }
    }
}
