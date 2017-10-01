using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Bitmap[] origImgPieces;

        private Bitmap[] chopBmp(Bitmap b)
        {
            Bitmap[] bArray = new Bitmap[8];
            for(int i = 0; i < 8; i++)
            {
                Bitmap target = new Bitmap(200,200);

                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(b, new Rectangle(0, 0, target.Width, target.Height), new Rectangle((i%3)*(b.Width/3), (i/3) * (b.Height / 3), 200,200), GraphicsUnit.Pixel);
                    bArray[i] = target;
                }
            }

            return bArray;
        }

        private void loadImgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap img;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Files|*.jpg;*.jpeg;*.png;";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    img = new Bitmap((Bitmap)Image.FromFile(openFileDialog1.FileName), 600,600);
                    origImgPieces = chopBmp(img);//unswapped
                    Random rnd = new Random();
                    Bitmap[] imgPieces = origImgPieces;//.OrderBy(x => rnd.Next()).ToArray();
                    for (int i = 0; i < 9; i++)
                    {
                        PictureBox p = new PictureBox();
                        p.Click += new System.EventHandler(this.imgSquare_Click);
                        this.tableLayoutPanel1.Controls.Add(p);
                        p.SetBounds(0, 0, p.Parent.Width, p.Parent.Height);
                        
                        if (i < 8)
                            p.Image = imgPieces[i];
                    }
                }
                catch
                {
                    MessageBox.Show("Bad File");
                }
            }
            
        }

        private void imgSquare_Click(object sender, EventArgs e)
        {
            PictureBox s = ((PictureBox)sender);
            int srcRow = this.tableLayoutPanel1.GetPositionFromControl(s).Row;
            int srcCol = this.tableLayoutPanel1.GetPositionFromControl(s).Column;
            PictureBox target;
            if((srcRow+1<=tableLayoutPanel1.RowCount-1 && (target = (PictureBox)tableLayoutPanel1.GetControlFromPosition(srcCol, srcRow + 1)).Image == null) || 
                (srcRow - 1 >= 0 && (target = (PictureBox)tableLayoutPanel1.GetControlFromPosition(srcCol, srcRow - 1)).Image == null) ||
                (srcCol + 1 <= tableLayoutPanel1.RowCount - 1 && (target = (PictureBox)tableLayoutPanel1.GetControlFromPosition(srcCol + 1, srcRow)).Image == null) ||
                (srcCol - 1 >= 0 && (target = (PictureBox)tableLayoutPanel1.GetControlFromPosition(srcCol - 1, srcRow)).Image == null))
            {
                Image temp = null;
                temp = s.Image;
                s.Image = target.Image;
                target.Image = temp;
            }
            //check if finished
            bool done = false;
            for(int i = 0; i < 8; i++)
            {
                if (origImgPieces[i] != ((PictureBox)tableLayoutPanel1.GetControlFromPosition(i%3, i/3)).Image)
                {
                    break;
                }
                if (i == 7)
                {
                    MessageBox.Show("Nice job ;]");
                }
            }
        }

        private void clearImgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.tableLayoutPanel1.Controls.Clear();
        }
    }
}
