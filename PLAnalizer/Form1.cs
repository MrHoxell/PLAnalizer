using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PLAnalizer
{
    public partial class Form1 : Form
    {
        /*
            - Not Important
            - Papyrus VM Status (3 lines)
            
        */

        private new string[] BackColor =
        {
            "default" //Error
        };

        private string[] Color =
        {
            "default"
            //"#E52C1B" //Error
        };

        private new int[] Font =
        {
            2
        };



        /// <summary>
        /// Returns choosen type of font by reading wanted position
        /// </summary>
        /// <param name="typeOfFont">0 = regular, 1 = bold, 2 = italic, 3 = underline</param>
        /// <returns>Choosen type of font</returns>
        private FontStyle setFont(int typeOfFont)
        {
            FontStyle output = new FontStyle();
            switch(typeOfFont)
            {
                case 0: output = FontStyle.Regular; break;
                case 1: output = FontStyle.Bold; break;
                case 2: output = FontStyle.Italic; break;
                case 3: output = FontStyle.Underline; break;
                default: output = FontStyle.Regular; break;
            }
            return output;
        }

        public Form1()
        {
            InitializeComponent();
            PapyrusOutput.BackColor = SystemColors.Control;
        }

        private void SyntaxWords()
        {

            //Pattern array
            string[] pattern =
            {
                "Papyrus\\slog\\sopened\\s.{4,7}|Update budget:.*|Memory page:.*", //Basic info (not important)
                @"Error",
            };

            for(int i = 0; i < pattern.Length; i++)
            {
                //Setup a collection of words that should be found
                MatchCollection match = Regex.Matches(PapyrusOutput.Text, pattern[i]);

                //Find all these words
                foreach (Match word in match)
                {
                    //Select matched WORD
                    PapyrusOutput.Select(word.Index, word.Length);

                    //Edit syntax of this word
                    try
                    {
                        PapyrusOutput.SelectionBackColor =
                            (BackColor[i] != "default") ? ColorTranslator.FromHtml(BackColor[i]) : PapyrusOutput.BackColor;
                        PapyrusOutput.SelectionColor =
                            (Color[i] != "default") ? ColorTranslator.FromHtml(Color[i]) : PapyrusOutput.ForeColor;
                        PapyrusOutput.SelectionFont = new Font(PapyrusOutput.Font, setFont(Font[i]));
                    }
                    catch(IndexOutOfRangeException) {
                        //MessageBox.Show("Check if ALL elements in config like: Text color, Text background color & Font style are having the same count of items", "Wrong config settings detected!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog FileDialog = new OpenFileDialog();
            FileDialog.Title = "Load Papyrus log file...";
            FileDialog.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";
            FileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\My Games\Skyrim Special Edition\Logs\Script";

            if(FileDialog.ShowDialog() == DialogResult.OK)
            {
                PapyrusOutput.Text = System.IO.File.ReadAllText(FileDialog.FileName);
                SyntaxWords();
            }

        }
    }
}
