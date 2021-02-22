using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReplaceTextProjectt
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // TODO: Add your code here

            int lineNumber = 0;
            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
            try
            {

                //Declare Variables
                string SourceFolderPath = @"C:\ProfilesDataFolder"; //Dts.Variables["User::SourceFolder"].Value.ToString();
                string FileExtension = ".csv"; // Dts.Variables["User::FileExtension"].Value.ToString();
                string FileDelimiter = "\t"; // Dts.Variables["User::FileDelimiter"].Value.ToString();
                FileDelimiter = FileDelimiter.Replace("\\t", "\t");
                string TableName = ""; // Dts.Variables["User::DestinationTable"].Value.ToString();
                string ArchiveFolder = ""; // Dts.Variables["User::ArchiveFolder"].Value.ToString();


                //string ColumnList = "";

                //Reading file names one by one
                string SourceDirectory = SourceFolderPath;
                string[] fileEntries = Directory.GetFiles(SourceDirectory, "*" + FileExtension);

                SqlConnection myADONETConnection = new SqlConnection();


                //var input = "asdads sdfdsf #burgers, # rabbits dsfsdfds # sdf #dfgdfg";
                //var regex = new Regex(@"#\w+");
                //var matches = regex.Matches(input);
                //foreach (var match in matches)
                //{
                //    Console.WriteLine(match);
                //}


                foreach (string fileName in fileEntries)
                {

                    //Writing Data of File Into Table
                    int counter = 0;
                    string line;
                    string apostrophe = "'";
                    string doubleQoute = "''";
                    string specialCharacter = "﻿";

                    //MessageBox.Show(fileName);

                    System.IO.StreamReader SourceFile =
                        new System.IO.StreamReader(fileName, System.Text.Encoding.UTF8);
                    while ((line = SourceFile.ReadLine()) != null)
                    {
                        lineNumber++;
                        if (counter > 0)
                        {
                            line = line.Replace(apostrophe, doubleQoute);
                            line = line.Replace(specialCharacter, "");

                            int fieldCount = 0;
                            int charNumbers = 0;
                            string newLine = null;

                            foreach (char c in line)
                            {
                                charNumbers++;

                                if (c == '\t')
                                    fieldCount++;
                                if (fieldCount == 4)
                                {
                                    newLine = line.Substring(charNumbers);
                                    break;
                                }
                            }


                            /*                            if (lineNumber == 18)
                                                            lineNumber = lineNumber;*/


                            var regexLinks = new Regex(@"(?<url>(http:|https:[/][/]|www.)([a-z]|[A-Z]|[0-9]|[/.]|[~])*)");
                            var matchesLinks = regexLinks.Matches(newLine);
                            string links = null;
                            string LinkValue = null;
                            foreach (Match m in matchesLinks)
                            {
                                LinkValue = m.Value;
                                if (LinkValue.EndsWith("pic"))
                                    LinkValue = LinkValue.Substring(0, LinkValue.Length - 3);
                                if (LinkValue.EndsWith("http"))
                                    LinkValue = LinkValue.Substring(0, LinkValue.Length - 4);
                                if (LinkValue.EndsWith("https"))
                                    LinkValue = LinkValue.Substring(0, LinkValue.Length - 5);

                                links = links + LinkValue + " ";
                            }

                            if (links != null)
                            {
                                int linksLength = links.Length;
                                if (linksLength > 200)
                                    linksLength = 200;
                                links = links.Substring(0, linksLength);
                                links = links.Trim();
                            }

                            var regexHashtags = new Regex(@"(?<=#)\w+");
                            var matchesHashtags = regexHashtags.Matches(newLine);
                            string hashtags = null;
                            string hashtagValue = null;
                            foreach (Match m in matchesHashtags)
                            {
                                hashtagValue = m.Value;
                                if (hashtagValue.EndsWith("pic"))
                                    hashtagValue = hashtagValue.Substring(0, hashtagValue.Length - 3);
                                if (hashtagValue.EndsWith("http"))
                                    hashtagValue = hashtagValue.Substring(0, hashtagValue.Length - 4);
                                if (hashtagValue.EndsWith("https"))
                                    hashtagValue = hashtagValue.Substring(0, hashtagValue.Length - 5);

                                hashtags = hashtags + "#" + hashtagValue + " ";
                            }

                            var regexHashtagsWithSpace = new Regex(@"(?<=#\s)\w+");
                            var matchesHashtagsWithSpace = regexHashtagsWithSpace.Matches(newLine);
                            foreach (Match m in matchesHashtagsWithSpace)
                            {
                                hashtagValue = m.Value;
                                if (hashtagValue.EndsWith("pic"))
                                    hashtagValue = hashtagValue.Substring(0, hashtagValue.Length - 3);
                                if (hashtagValue.EndsWith("http"))
                                    hashtagValue = hashtagValue.Substring(0, hashtagValue.Length - 4);
                                if (hashtagValue.EndsWith("https"))
                                    hashtagValue = hashtagValue.Substring(0, hashtagValue.Length - 5);

                                hashtags = hashtags + "#" + hashtagValue + " ";
                            }


                            if (hashtags != null)
                            {
                                int hashtagsLength = hashtags.Length;
                                if (hashtagsLength > 100)
                                    hashtagsLength = 100;

                                hashtags = hashtags.Substring(0, hashtagsLength);
                                hashtags = hashtags.Trim();

                            }

                            var regexMentions = new Regex(@"(?<=@)\w+");
                            var matchesMentions = regexMentions.Matches(newLine);
                            string mentions = null;
                            string mentionValue = null;
                            foreach (Match m in matchesMentions)
                            {
                                mentionValue = m.Value;
                                if (mentionValue.EndsWith("pic"))
                                    mentionValue = mentionValue.Substring(0, mentionValue.Length - 3);
                                if (mentionValue.EndsWith("http"))
                                    mentionValue = mentionValue.Substring(0, mentionValue.Length - 4);
                                if (mentionValue.EndsWith("https"))
                                    mentionValue = mentionValue.Substring(0, mentionValue.Length - 5);

                                mentions = mentions + "@" + mentionValue + " ";
                            }

                            var regexMentionsWithSpace = new Regex(@"(?<=@\s)\w+");
                            var matchesMentionsWithSpace = regexMentionsWithSpace.Matches(newLine);

                            foreach (Match m in matchesMentionsWithSpace)
                            {
                                mentionValue = m.Value;
                                if (mentionValue.EndsWith("pic"))
                                    mentionValue = mentionValue.Substring(0, mentionValue.Length - 3);
                                if (mentionValue.EndsWith("https"))
                                    mentionValue = mentionValue.Substring(0, mentionValue.Length - 5);

                                mentions = mentions + "@" + mentionValue + " ";
                            }

                            if (mentions != null)
                            {
                                int mentionsLength = mentions.Length;
                                if (mentionsLength > 100)
                                    mentionsLength = 100;
                                mentions = mentions.Substring(0, mentionsLength);
                                mentions = mentions.Trim();
                            }


                            line = line.Substring(0, charNumbers);
                            line = line + newLine;

                            string query = "Insert into " + TableName + " Values (N'" + line.Replace(FileDelimiter, "',N'") + "',N'" + hashtags + "',N'" + mentions + "',N'" + links + "','" + "')";
                            //MessageBox.Show(query.ToString());
                            SqlCommand myCommand1 = new SqlCommand(query, myADONETConnection);
                            myCommand1.ExecuteNonQuery();
                        }

                        counter++;
                    }

                    SourceFile.Close();
                    //move the file to archive folder after adding datetime to it
                    File.Move(fileName, ArchiveFolder + "\\" + (fileName.Replace(SourceFolderPath, "")).Replace(FileExtension, "") + "_" + datetime + FileExtension);
                   
                }
            }
            catch (Exception exception)
            {
                // Create Log File for Errors
                //using (StreamWriter sw = File.CreateText(Dts.Variables["User::LogFolder"].Value.ToString()
                //    + "\\" + "ErrorLog_" + datetime + ".log"))
                //{
                //    sw.WriteLine(exception.ToString() + " \n My Line Number = " + lineNumber.ToString());
                   
                //}

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string strConString = "Server=.;Database=MediaStat;Trusted_Connection=True;MultipleActiveResultSets=true;";
            SqlConnection con = new SqlConnection(strConString);
            DataTable dtDates = new DataTable();

            using (SqlCommand cmd = new SqlCommand("SELECT [Id],[DayDate] FROM [MediaStat].[dbo].[TweetDateDim]", con))
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                dtDates.Clear();
                SqlDataReader drAccounts = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dtDates.Load(drAccounts);
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }

            string strDate1 = "2020-02-29 00:20:09";
            string strDate2 = "2020-07-04 00:12:48";
            string strDate3 = "2020-08-23 13:45:19";
            string strDate4 = "2020-08-20 20:27:15";
            if (strDate1.Length > 3 && (!string.IsNullOrWhiteSpace(strDate1)))
            {


                DateTime dt = DateTime.ParseExact(strDate3, "yyyy-MM-dd HH:mm:ss", null);
                string s = dt.ToString("yyyy-MM-dd", null);
                DateTime dtTweetDate = DateTime.ParseExact(s, "yyyy-MM-dd", null);

                var tweetDate = (from myRow in dtDates.AsEnumerable()
                                 where myRow.Field<DateTime>("DayDate") == dtTweetDate
                                 select myRow).ToList();
                if (tweetDate.Count != 0)
                {
                    int dateId = tweetDate[0].Field<int>("Id");
                }
            }
        }
    }
}
