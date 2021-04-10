using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IconTextureToByteArrayConverter
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void convertImagebtn_Click(object sender, EventArgs e)
        {
            fileOpener.ShowDialog();
        }

        private void fileOpener_FileOk(object sender, CancelEventArgs e)
        {
            string Code = "";
            const float ConversionValue = 1f / (255*3);
            using(Stream fs = fileOpener.OpenFile())
            {
                using (Bitmap img = new Bitmap(fs))
                {
                    if(img.Width != 16 || img.Height != 16)
                    {
                        codeOutputBox.Text = "Image must be 16x16!";
                        return;
                    }
                    bool FirstY = true;
                    for (int y = 0; y < img.Height; y++)
                    {
                        if (!FirstY)
                            Code += ",";
                        else
                            FirstY = false;
                        Code += "\n\t{";
                        bool FirstX = true;
                        for (int x = 0; x < img.Width; x++)
                        {
                            Color color = img.GetPixel(x, y);
                            byte ImageByte = (byte)(9 - (color.R + color.G + color.B) * ConversionValue * 9f);//(byte)(255 - ((color.R + color.G + color.B) * ConversionValue));
                            if (color.A == 0)
                                ImageByte = 0;
                            else if (ImageByte == 0)
                                ImageByte = 1;
                            if (!FirstX)
                                Code += ",";
                            else
                                FirstX = false;
                            Code += ImageByte;
                        }
                        Code += "}";
                    }
                }            
            }
            Code = "new byte[,]{" + Code + "\n};";
            codeOutputBox.Text = Code;
        }
    }
}
