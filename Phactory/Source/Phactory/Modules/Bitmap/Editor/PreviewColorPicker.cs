using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace CPCBitmap.Editor
{
    public partial class PreviewColorPicker : Form
    {
        Bitmap bitmap = null;
        int pickedColor;

        public PreviewColorPicker()
        {
            InitializeComponent();
        }

        public void SetImage(Image image)
        {
            bitmap = new Bitmap(image);

            PictureBox.Image = image;
        }

        public int GetPickedColor()
        {
            return pickedColor;
        }

        private Color GetMousePickedColor()
        {
            Point mousePos = PictureBox.PointToClient(Cursor.Position);

            int x = (int)( ((float)(mousePos.X)/(float)(PictureBox.Width)) * (float)PictureBox.Image.Width);
            int y = (int)( ((float)(mousePos.Y)/(float)(PictureBox.Height)) * (float)PictureBox.Image.Height);

            if (x < 0)
            {
                return Color.Transparent;
            }
            if (y < 0)
            {
                return Color.Transparent;
            }
            if (x >= PictureBox.Image.Width)
            {
                return Color.Transparent;
            }
            if (y >= PictureBox.Image.Height)
            {
                return Color.Transparent;
            }

            return bitmap.GetPixel(x, y);
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if ( bitmap == null )
            {
                return;
            }

            panel1.BackColor = GetMousePickedColor();
        }

        private void PictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (bitmap == null)
            {
                return;
            }

            Color color = GetMousePickedColor();
            pickedColor = color.ToArgb(); 
            
            this.Close();
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            // remove bilinear
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = SmoothingMode.None;
            
            base.OnPaint(e);
        }
    }
}
