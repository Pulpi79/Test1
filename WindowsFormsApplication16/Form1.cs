﻿using SHDocVw;
using Shell32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WindowsFormsApplication16
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static int CalculateCheckNumber(string text)
        {
            int checkNumber = 0;

            for (int i = 0; i < text.Length; i++)
            {
                int digit = int.Parse(text[i].ToString(), CultureInfo.InvariantCulture);
                int mul = (i % 2 == 0 ? 1 : 3);
                int add = digit * mul;
                checkNumber = checkNumber + add;
            }

            checkNumber = (10 - checkNumber % 10) % 10;

            return checkNumber;
        }

        public static string IntToJournalSatzNummer(int i)
        {
            return i.ToString("D4", CultureInfo.InvariantCulture);
        }

        public static string IntToJournalNummer(int i)
        {
            return i.ToString("D5", CultureInfo.InvariantCulture);
        }

        public static string DateToDayMonthYear(DateTime dateTime)
        {
            return dateTime.ToString("ddMMyy", CultureInfo.InvariantCulture);
        }

        public static string IntToString(int i)
        {
            return i.ToString(CultureInfo.InvariantCulture);
        }

        public static string GetEanCode19(string cashBoxNumber, DateTime dateTime, int journalsatzNummer)
        {
            string actualDate = DateToDayMonthYear(dateTime);
            string paddedJournalsatzNummer = IntToJournalSatzNummer(journalsatzNummer);
            string eanCode19 = cashBoxNumber + actualDate + paddedJournalsatzNummer;
                    //220216      401198

            int checkNumber = CalculateCheckNumber(eanCode19);

            eanCode19 += IntToString(checkNumber);

            return eanCode19;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime a = Convert.ToDateTime("22.02.16");
            string eanCode =GetEanCode19("70000104",a, 174);
            int length = eanCode.Length;
            string ab = "70000104220216401198";
           //string a= IntToJournalSatzNummer(8);
        /*
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        saveFileDialog1.Filter = "Rar Files (.rar)|*.rar|All Files (*.*)|*.*";
        saveFileDialog1.Title = "Bitte Speichern";
        saveFileDialog1.ShowDialog();

        // If the file name is not an empty string open it for saving.
        if (saveFileDialog1.FileName != "")
        {

            File.Copy(saveFileDialog1.FileName, Path.Combine(@"C:\", saveFileDialog1.FileName));
            // Saves the Image via a FileStream created by the OpenFile method.
            /*   System.IO.FileStream fs =
                  (System.IO.FileStream)saveFileDialog1.OpenFile();
               // Saves the Image in the appropriate ImageFormat based upon the
               // File type selected in the dialog box.
               // NOTE that the FilterIndex property is one-based.
               switch (saveFileDialog1.FilterIndex)
               {
                   case 1:
                       this.button1.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Jpeg);
                       break;

                   case 2:
                       this.button1.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Bmp);
                       break;

                   case 3:
                       this.button1.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Gif);
                       break;
               }

               fs.Close();*/
        //}
    }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "Rar Files (.rar)|*.rar|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Multiselect = true;

            // Call the ShowDialog method to show the dialog box.
            //   openFileDialog1.ShowDialog();

            // Process input if the user clicked OK.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string a = openFileDialog1.FileName;
                File.Copy(openFileDialog1.FileName, Path.Combine(@"C:\", openFileDialog1.SafeFileName));
                File.Copy(openFileDialog1.FileName, Path.Combine(@"C:\", openFileDialog1.SafeFileName));
                // Open the selected file to read.
                //System.IO.Stream fileStream = openFileDialog1.File.OpenRead();

                //using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                //{
                //    // Read the first line from the file and write it the textbox.
                //    tbResults.Text = reader.ReadLine();
                //}
                //fileStream.Close()
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] a = File.ReadAllLines("as.txt");
            List<string> list = new List<string>(a);
            List<string> indexList = new List<string>();
            Dictionary<string, List<string>> buchungen = new Dictionary<string, List<string>>();
            int i = 0;


            foreach (var entry in list)
            {

                if (!Regex.IsMatch(entry, @"^\d{2}(?:\.\d{2})\.\d{2}(?:\.\d{2})"))
                {
                    indexList.Add(entry);
                }
                else
                {
                    i++;
                    indexList.Add(entry);
                    string[] array = new string[indexList.Count];
                    indexList.CopyTo(array);
                    buchungen.Add("Buchung " + i.ToString(), array.ToList());
                    indexList.Clear();
                }
            }

            PrepareStrings(buchungen);
        }

        private void PrepareStrings(Dictionary<string,List<string>> dictionary)
        {
            var betreff = string.Empty;
            List<string> listToParse = new List<string>();
          
            Buchung buchung = new Buchung();
            Buchungen buchungen = new Buchungen();

            foreach(var entry in dictionary)
            {
                dictionary.TryGetValue(entry.Key, out listToParse);

                buchung.Vorzeichen = listToParse[0].Substring(0, 1);
                buchung.BuchungsTyp = Regex.Match(listToParse[0], @"([A-Z])\w+").Value;
                buchung.Betrag = Convert.ToDouble(Regex.Match(listToParse[0], @"\d+\,\d+").Value);

                buchung.Datum = Convert.ToDateTime(Regex.Match(listToParse[7], @"\d{2}\.\d{2}").Value);

                for (int i = 1; i <= 6; i++)
                {
                    betreff += listToParse[i].ToString();
                }
                buchung.Betreff = betreff;
                buchungen.Add(buchung);
                betreff = string.Empty;
            }

            //string firstChar = listToParse[0].ToString().Substring(0, 1);
            //string getTextFromFirstLine = Regex.Match(listToParse[0], @"([A-Z])\w+").Value;
            //var resultString = Regex.Match(listToParse[0], @"\d+\,\d+").Value;

            //var datum = Regex.Match(listToParse[7], @"\d{2}\.\d{2}").Value;

            //for (int i = 1; i <= 6; i++)
            //{
            //    betreff += listToParse[i].ToString();
            //}

           
            

        }
    }
}

