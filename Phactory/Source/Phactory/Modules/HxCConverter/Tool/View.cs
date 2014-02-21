using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HxCConverter.Tool
{
    public partial class View : Form
    {
        PhactoryHost.Host _host;

        public View()
        {
            InitializeComponent();

            this.Convert.Enabled = false;
        }

        public View(PhactoryHost.Host host)
            : this()
        {
            _host = host;
        }

        private void AddFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter =
                "DSK Image file (*.DSK)|*.DSK|" +
                "All files (*.*)|*.*";

            openFileDialog1.Multiselect = true;
            openFileDialog1.Title = "Select DSK file(s)...";
            openFileDialog1.FileName = String.Empty;
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (String filename in openFileDialog1.FileNames)
                {
                    Files.Items.Add(filename);
                }
            }

            if (Files.Items.Count > 0)
            {
                this.Convert.Enabled = true;
            }
        }

        private void Convert_Click(object sender, EventArgs e)
        {
            int successCount = 0;
            int failCount = 0;

            _host.Log("Converting " + Files.Items.Count + " file(s)...");
            foreach (var item in Files.Items)
            {
                if (ConvertDSK(item.ToString()))
                {
                    successCount++;
                    _host.Log(item.ToString() + " converted");
                }
                else
                {
                    failCount++;
                    _host.Log("Failed to convert " + item.ToString() + " !");
                }
            }

            if (failCount == 0)
            {
                _host.Log("All file(s) successfully converted.");
                MessageBox.Show("File(s) successfully converted", "Conversion result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _host.Log("Error(s) occured while converting to HFE format !");
                MessageBox.Show("Error(s) occured while converting to HFE format !", "Conversion result", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ConvertDSK(string filename)
        {
            var dskFileInfo = new FileInfo(filename);

            string tempDir = "\\HFE_CONVERT";

            Directory.CreateDirectory(tempDir);

            string src = tempDir + "\\" + dskFileInfo.Name;
            if (File.Exists(src))
            {
                File.Delete(src);
            }
            File.Copy(dskFileInfo.FullName, src);

            string hfeConverterFullPath = _host.GetPluginsPath() + "flopimage_convert.exe";

            string arguments = tempDir + " " + tempDir + " -HFE";

            if (_host.IsVerboseOutput())
            {
                _host.Log(hfeConverterFullPath + " " + arguments);
            }

            string dst = "";

            bool isOK = _host.StartProcess(hfeConverterFullPath, arguments, tempDir, true);
            if (isOK)
            {
                string dskhfe = src.Replace(".dsk", "_dsk.hfe");
                dst = dskFileInfo.FullName.Replace(".dsk", ".hfe");

                if (File.Exists(dst))
                {
                    File.Delete(dst);
                }
                File.Copy(dskhfe, dst);

                File.Delete(src);
                File.Delete(dskhfe);
            }

            Directory.Delete(tempDir);

            return File.Exists(dst);
        }
    }
}
