using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PixelCalculator.Tool
{
    public partial class View : Form
    {
        private PhactoryHost.Host _host;

        public View()
        {
            InitializeComponent();
        }

        public View(PhactoryHost.Host host) : this()
        {
            _host = host;

            Mode0LeftPixel.Text = "0";
            Mode0RightPixel.Text = "0";

            Mode1Pixel0.Text = "0";
            Mode1Pixel1.Text = "0";
            Mode1Pixel2.Text = "0";
            Mode1Pixel3.Text = "0";
        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private byte ConvertPixelCPC_Mode0(byte p0, byte p1)
        {
            byte v;
            v = (byte)(
                    (((p0) & 1) << 7) + (((p1) & 1) << 6)
                + (((p0 >> 2) & 1) << 5) + (((p1 >> 2) & 1) << 4)
                + (((p0 >> 1) & 1) << 3) + (((p1 >> 1) & 1) << 2)
                + (((p0 >> 3) & 1) << 1) + (((p1 >> 3) & 1)));

            return v;
        }

        private byte ConvertPixelCPC_Mode1(byte p0, byte p1, byte p2, byte p3)
        {
            byte v;
            v = (byte)(
                    (((p0) & 1) << 7) + (((p1) & 1) << 6)
                + (((p2) & 1) << 5) + (((p3) & 1) << 4)
                + (((p0 >> 1) & 1) << 3) + (((p1 >> 1) & 1) << 2)
                + (((p2 >> 1) & 1) << 1) + (((p3 >> 1) & 1)));

            return v;
        }

        private void Mode0LeftPixel_TextChanged(object sender, EventArgs e)
        {
            Mode0UpdateResult();
        }

        private void Mode0RightPixel_TextChanged(object sender, EventArgs e)
        {
            Mode0UpdateResult();
        }

        private void Mode0UpdateResult()
        {
            byte leftPixel = 0;
            byte rightPixel = 0;

            try
            {
                leftPixel = Convert.ToByte(Mode0LeftPixel.Text.ToString());
            }
            catch
            {            	
            }

            try
            {
                rightPixel = Convert.ToByte(Mode0RightPixel.Text.ToString());
            }
            catch
            {
            }

            var cpcByte = ConvertPixelCPC_Mode0(leftPixel, rightPixel);

            Mode0Result.Text = "" + cpcByte + " (0x" + String.Format("{0:X2}", cpcByte) + ")";
        }

        private void Mode1Pixel0_TextChanged(object sender, EventArgs e)
        {
            Mode1UpdateResult();
        }

        private void Mode1Pixel1_TextChanged(object sender, EventArgs e)
        {
            Mode1UpdateResult();
        }

        private void Mode1Pixel2_TextChanged(object sender, EventArgs e)
        {
            Mode1UpdateResult();
        }

        private void Mode1Pixel3_TextChanged(object sender, EventArgs e)
        {
            Mode1UpdateResult();
        }

        private void Mode1UpdateResult()
        {
            byte p0 = 0;
            byte p1 = 0;
            byte p2 = 0;
            byte p3 = 0;

            try
            {
                p0 = Convert.ToByte(Mode1Pixel0.Text.ToString());
            }
            catch
            {
            }
            try
            {
                p1 = Convert.ToByte(Mode1Pixel1.Text.ToString());
            }
            catch
            {
            }
            try
            {
                p2 = Convert.ToByte(Mode1Pixel2.Text.ToString());
            }
            catch
            {
            }
            try
            {
                p3 = Convert.ToByte(Mode1Pixel3.Text.ToString());
            }
            catch
            {
            }

            var cpcByte = ConvertPixelCPC_Mode1(p0, p1, p2, p3);

            Mode1Result.Text = "" + cpcByte + " (0x" + String.Format("{0:X2}", cpcByte) + ")";
        }

        private void Close_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
