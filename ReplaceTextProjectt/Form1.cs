using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReplaceTextProjectt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            //System.IO.StreamReader file = new System.IO.StreamReader(@"F:\Tweet\From Jojo\New\test - Copy\Profiles.csv");
            System.IO.StreamReader file = new System.IO.StreamReader(txtMainFile.Text);
            string lineWithoutQutaionsAtBeginAndEnd;
            string lineReplaced;
            string strAll = string.Empty;

            while ((line = file.ReadLine()) != null)
            {
                if (counter != 0)
                {
                    lineWithoutQutaionsAtBeginAndEnd = line.Substring(1, line.Length - 2);
                    lineReplaced = Regex.Replace(lineWithoutQutaionsAtBeginAndEnd, @"(?<!,)""(?!,)", @"'*&");
                    strAll = strAll + "\"" + lineReplaced + "\"" + Environment.NewLine;
                }
                else
                {
                    strAll += line + Environment.NewLine;
                }

                counter++;
            }

            file.Close();

            //System.IO.File.WriteAllText(@"C:\Users\z2\Desktop\Replaced.csv", string.Empty);
            //System.IO.File.WriteAllText(@"C:\Users\z2\Desktop\Replaced.csv", strAll);

            System.IO.File.WriteAllText(txtReplaced.Text, string.Empty);
            System.IO.File.WriteAllText(txtReplaced.Text, strAll);




            //string input = System.IO.File.ReadAllText(@"C:\Users\z2\Desktop\Replaced.csv");
            //var output = Regex.Replace(input, @"(?<!,)""(?!,)", @"'*&");
            ////var output = Regex.Replace(input, @"(?<!,)""(?!,)(!^{""})", @"'*&");



            //System.IO.File.WriteAllText(@"C:\Users\z2\Desktop\Replaced.csv", string.Empty);
            //System.IO.File.WriteAllText(@"C:\Users\z2\Desktop\Replaced.csv", output);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var oldString = "your\ttext area\n string\r\n for testing";
            var newString = string.Join(" ", Regex.Split(oldString, @"(?:\r)"));
            MessageBox.Show(newString);
        }
    }
}
