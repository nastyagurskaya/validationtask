using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataValidationConsole
{
    public partial class Form1 : Form
    {
        /// <summary> 
        ///  Initialize Form
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary> 
        ///  Check button click
        ///  Revealing and comparing hash
        /// </summary>
        private void checkButtonClick(object sender, EventArgs e)
        {
            try { 
                // Cheaking fullness of fields
                if(hashTextBox.Text==""||fileNameTextBox.Text==""||(!radioMD5.Checked && !radioCRC32.Checked))
                {
                    throw new Exception("Not all fields are full");
                }
                String hash="";
                String textHash = hashTextBox.Text;
                String filename = fileNameTextBox.Text;
                //Opening file
                FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
                
                //Crc32 option
                if (radioCRC32.Checked)
                {
                    if (textHash.Length != 8) throw new Exception("Wrong CRC32 format");
                    Crc32 crc = new Crc32();
                    byte[] h = crc.ComputeHash(file);
                    hash = BitConverter.ToString(h);
                    hash = hash.Replace("-", "");
                }
                //MD5 option
                if (radioMD5.Checked)
                {
                    if (textHash.Length != 32) throw new Exception("Wrong MD5 format");
                    StreamReader reader = new StreamReader(file);
                    //Reading file
                    String contents = reader.ReadToEnd();
                    reader.Close();
                    MD5 md = new MD5();
                    md.Value = contents;
                    hash = md.FingerPrint;
                }
                file.Close();
                //Comparing
                if (textHash.CompareTo(hash) == 0)
                    outputLabel.Text = "Success";
                else outputLabel.Text = "Hashes don't match\n";
                //4EB98ECB80B0F639373B9F370785F10A
                //17E3AE0D
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            DialogResult result = of.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                fileNameTextBox.Text = of.FileName;
            }
        }
    }
}
