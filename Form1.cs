using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
//using Microsoft.WindowsAPICodePack.Shell;
//using Microsoft.WindowsAPICodePack.ShellExtensions;
//using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System.Reflection;
using System.Drawing.Imaging;

namespace RFAThumbnailCreator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SearchOption so = SearchOption.TopDirectoryOnly;
            if (checkBox1.Checked)
            {
                so = SearchOption.AllDirectories;
            }
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string dir = dialog.SelectedPath;

                if (!Directory.Exists(Path.Combine(dir, "_Images")))
                {
                    Directory.CreateDirectory(Path.Combine(dir, "_Images"));
                }

                DirectoryInfo di = new DirectoryInfo(dir);
                var directories = di.GetFiles("*.rfa",so);                

                //progress bar
                int i = 0;
                progressBar1.Value = 0;
                progressBar1.Maximum = directories.Count();
                progressBar1.Step = 1;

                // Loop
                foreach (FileInfo fi in directories)
                {                                   
                    try
                    {
                        Storage storage = new Storage(fi.FullName);
                        Image b = storage.ThumbnailImage.GetPreviewAsImage();

                        Bitmap bm = new Bitmap(b.Width, b.Height, PixelFormat.Format24bppRgb);
                        Graphics g = Graphics.FromImage(bm);
                        g.DrawImage(b, new Point(0, 0));
                        g.Dispose();
                        
                        bm.Save(Path.Combine(dir, "_Images", fi.Name.Remove(fi.Name.Length - 4) + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                        i++;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    
                    progressBar1.PerformStep();
                }
                
                MessageBox.Show("Image Creation Complete!");

            }


        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
