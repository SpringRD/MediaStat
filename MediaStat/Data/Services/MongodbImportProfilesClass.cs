using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.JSInterop;
using System.Globalization;
using LinqToTwitter;

namespace MediaStat.Data.Services
{
    public class MongodbImportProfilesClass
    {

        private static string _lastId;
        private static DataTable dtAccounts = new DataTable();

        public static async Task<string> ImportProfilesfromMongodb()
        {
            // TODO: Add your code here

            int lineNumber = 0;
            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
            SqlConnection myADONETConnection = null;
            int counter = 0;

            try
            {

                //Declare Variables
                string query;

                DataTable dtDates = new DataTable();
                SqlCommand myCommand1;


                myADONETConnection = new SqlConnection(MyAppData.MyConnectionString);
                myADONETConnection.Open();

                query = "SELECT [AccountId],[ScreenName],[SpecialAccountId] FROM [MediaStat].[dbo].[Accounts]";
                myCommand1 = new SqlCommand(query, myADONETConnection);
                SqlDataReader drAccounts = myCommand1.ExecuteReader(CommandBehavior.CloseConnection);
                dtAccounts.Load(drAccounts);


                //var documents = GetTweetsfromCollection();

                List<BsonDocument> documents;
                //_lastId = GetLastMongoImportedId(myADONETConnection);
                //if (string.IsNullOrEmpty(_lastId))
                //{
                documents = GetProfilesfromCollection(true);
                //}
                //else
                //{
                //    documents = GetProfilesfromCollection(false);
                //}

                foreach (var doc in documents)
                {


                    if (doc[3] != null)
                    {
                        var findAccount = (from myRow in dtAccounts.AsEnumerable()
                                           where string.Compare(myRow.Field<string>("ScreenName"), doc[3].ToString()) == 0
                                           select myRow).ToList();
                        if (findAccount.Count == 0)
                        {
                            InsertAccountByScreenName(myADONETConnection, doc);
                        }
                        else
                        {
                            UpdateAccountByScreenName(myADONETConnection, doc);
                        }
                    }

                //    var urlArray = (BsonArray)doc[11];


                //    if (doc[5] != null)
                //    {


                //        //DateTime dt = DateTime.ParseExact(doc[5].ToString(), "yyyy-MM-dd HH:mm:ss", null);
                //        string strDt = doc[5].ToString();
                //        if (strDt.Contains("+0000")) strDt = strDt.Remove(strDt.IndexOf("+"), 6);
                //        DateTime dt = DateTime.ParseExact(strDt, "ddd MMM dd HH:mm:ss yyyy", CultureInfo.InvariantCulture);
                //        string s = dt.ToString("yyyy-MM-dd", null);
                //        DateTime dtTweetDate = DateTime.ParseExact(s, "yyyy-MM-dd", null);

                //        var tweetDate = (from myRow in dtDates.AsEnumerable()
                //                         where myRow.Field<DateTime>("DayDate") == dtTweetDate
                //                         select myRow).ToList();
                //        if (tweetDate.Count != 0)
                //        {
                //            //dateId = tweetDate[0].Field<int>("Id");
                //        }
                //    }



                //    int? AccountId = null;
                //    string FullText = null;
                //    string SpecialText = null;
                //    string TweetSpecialId = null;
                //    int? LikesCount = null;
                //    int? RetweetCount = null;
                //    int? CommentsCount = null;

                //    if (doc[2] != null)
                //    {
                //        var findAccount = (from myRow in dtAccounts.AsEnumerable()
                //                           where string.Compare(myRow.Field<string>("SpecialAccountId"), doc[2].ToString()) == 0
                //                           select myRow).ToList();
                //        if (findAccount.Count != 0)
                //        {
                //            AccountId = findAccount[0].Field<int>("AccountId");
                //        }
                //        else
                //        {
                //            //string screenName = await UpdateProfileData(doc[2].ToString(), "");
                //            //AccountId = GetAccountByScreenName(myADONETConnection, screenName);
                //            RefreshAccounts(myADONETConnection);
                //        }
                //    }

                //    if (doc[4] != null)
                //    {
                //        FullText = doc[4].ToString();
                //        SpecialText = doc[4].ToString();
                //    }


                //    if (doc[1] != null)
                //    {
                //        TweetSpecialId = doc[1].ToString();
                //    }

                //    if (doc[1] != null)
                //    {
                //        LikesCount = int.Parse(doc[7].ToString());
                //    }
                //    if (doc[1] != null)
                //    {
                //        RetweetCount = int.Parse(doc[6].ToString());
                //    }
                //    if (doc[1] != null)
                //    {
                //        CommentsCount = int.Parse(doc[8].ToString());
                //    }


                //    //
                //    string insertTweetQuery = "INSERT INTO [dbo].[Accounts] ([ScreenName],[ProfileName],[Joined],[DateOfBirth],[LocationDescription],[AccountUrl],[Followers],[Following],[ProfileImageURL],[SpecialAccountId]) " +
                //        "VALUES (@ScreenName,@ProfileName,@Joined,@DateOfBirth,@LocationDescription,@AccountUrl,@Followers,@Following,@ProfileImageURL,@SpecialAccountId)";

                //    using (SqlCommand cmd = new SqlCommand(insertTweetQuery, myADONETConnection))
                //    {
                //        cmd.Parameters.AddWithValue("@AccountId", (AccountId != null) ? AccountId : (object)DBNull.Value);
                //        cmd.Parameters.AddWithValue("@FullText", (FullText != null) ? FullText : (object)DBNull.Value);
                //        cmd.Parameters.AddWithValue("@SpecialText", (SpecialText != null) ? SpecialText : (object)DBNull.Value);
                //        cmd.Parameters.AddWithValue("@TweetSpecialId", (TweetSpecialId != null) ? TweetSpecialId : (object)DBNull.Value);
                //        cmd.Parameters.AddWithValue("@LikesCount", (LikesCount != null) ? LikesCount : (object)DBNull.Value);
                //        cmd.Parameters.AddWithValue("@RetweetCount", (RetweetCount != null) ? RetweetCount : (object)DBNull.Value);
                //        cmd.Parameters.AddWithValue("@CommentsCount", (CommentsCount != null) ? CommentsCount : (object)DBNull.Value);
                //        if (myADONETConnection.State == System.Data.ConnectionState.Closed)
                //            myADONETConnection.Open();

                //        cmd.ExecuteNonQuery();

                //        if (myADONETConnection.State == System.Data.ConnectionState.Open)
                //            myADONETConnection.Close();

                //    }
                //    _lastId = doc[2].ToString();
                //    counter++;
                }

            }
            catch (Exception exception)
            {
                int c = counter;
            }
            finally
            {
                UpdateLastMongoImportedId(myADONETConnection);
                if (myADONETConnection.State == ConnectionState.Open) myADONETConnection.Close();
            }
            return "Finished";

        }

        private static void InsertAccountByScreenName(SqlConnection con, BsonDocument doc)
        {
            string imageProfile = doc[10].ToString().Replace("_normal", "_400x400");

            //using (SqlCommand cmd = new SqlCommand("INSERT INTO Accounts(ScreenName,ProfileName,LocationDescription,Description,Followers,Following,ProfileImageURL,SpecialAccountId) VALUES(@ScreenName,@ProfileName,@LocationDescription,@Description,@Followers,@Following,@ProfileImageURL,@SpecialAccountId)", con))
            using (SqlCommand cmd = new SqlCommand("INSERT INTO Accounts(ScreenName,ProfileName,LocationDescription,Description,Followers,Following,SpecialAccountId,ProfileImageURL) VALUES(@ScreenName,@ProfileName,@LocationDescription,@Description,@Followers,@Following,@SpecialAccountId,@ProfileImageURL)", con))
            {
                cmd.Parameters.AddWithValue("@ScreenName", doc[3].ToString());
                cmd.Parameters.AddWithValue("@ProfileName", doc[2].ToString());
                cmd.Parameters.AddWithValue("@LocationDescription", doc[4].ToString());
                cmd.Parameters.AddWithValue("@Description", doc[5].ToString());
                cmd.Parameters.AddWithValue("@Followers", int.Parse(doc[6].ToString()));
                cmd.Parameters.AddWithValue("@Following", int.Parse(doc[7].ToString()));
                cmd.Parameters.AddWithValue("@ProfileImageURL", imageProfile);
                cmd.Parameters.AddWithValue("@SpecialAccountId", doc[1].ToString());
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                cmd.ExecuteNonQuery();

                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();


            }
        }

        private static void UpdateAccountByScreenName(SqlConnection con, BsonDocument doc)
        {
            string imageProfile = doc[10].ToString().Replace("_normal", "_400x400");

            //using (SqlCommand cmd = new SqlCommand("INSERT INTO Accounts(ScreenName,ProfileName,LocationDescription,Description,Followers,Following,ProfileImageURL,SpecialAccountId) VALUES(@ScreenName,@ProfileName,@LocationDescription,@Description,@Followers,@Following,@ProfileImageURL,@SpecialAccountId)", con))
            using (SqlCommand cmd = new SqlCommand("Update Accounts SET ProfileName= @ProfileName,LocationDescription= @LocationDescription,Description= @Description,Followers= @Followers,Following= @Following,SpecialAccountId= @SpecialAccountId,ProfileImageURL= @ProfileImageURL  WHERE ScreenName = @ScreenName", con))
            {
                cmd.Parameters.AddWithValue("@ScreenName", doc[3].ToString());
                cmd.Parameters.AddWithValue("@ProfileName", doc[2].ToString());
                cmd.Parameters.AddWithValue("@LocationDescription", doc[4].ToString());
                cmd.Parameters.AddWithValue("@Description", doc[5].ToString());
                cmd.Parameters.AddWithValue("@Followers", int.Parse(doc[6].ToString()));
                cmd.Parameters.AddWithValue("@Following", int.Parse(doc[7].ToString()));
                cmd.Parameters.AddWithValue("@ProfileImageURL", imageProfile);
                cmd.Parameters.AddWithValue("@SpecialAccountId", doc[1].ToString());
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                cmd.ExecuteNonQuery();

                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();


            }
        }


        private static int GetAccountByScreenName(SqlConnection con, string sreenName)
        {

            using (SqlCommand cmd = new SqlCommand("Select AccountId from Accounts where ScreenName  = '" + sreenName + "'", con))
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                int modified = (int)cmd.ExecuteScalar();

                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();

                return modified;
            }
        }

        private static int InsertAccountBySpecialId(SqlConnection con, string specialAccountId)
        {
            using (SqlCommand cmd = new SqlCommand("INSERT INTO Accounts(SpecialAccountId) output INSERTED.AccountId VALUES(@SpecialAccountId)", con))
            {
                cmd.Parameters.AddWithValue("@SpecialAccountId", specialAccountId);
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                int modified = (int)cmd.ExecuteScalar();

                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();

                return modified;
            }
        }


        private static void RefreshAccounts(SqlConnection con)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT [AccountId],[ScreenName],[SpecialAccountId] FROM [MediaStat].[dbo].[Accounts]", con))
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                dtAccounts.Clear();
                SqlDataReader drAccounts = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dtAccounts.Load(drAccounts);
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }


        private static List<BsonDocument> GetProfilesfromCollection(bool blnAll)
        {
            string str = string.Empty;
            try
            {
                List<BsonDocument> documents;
                var dbClient = new MongoClient("mongodb://localhost:27017");
                IMongoDatabase db = dbClient.GetDatabase("crawling");
                var tweets = db.GetCollection<BsonDocument>("users");

                if (blnAll)
                {
                    var sort = Builders<BsonDocument>.Sort.Ascending("_id");
                    documents = tweets.Find(new BsonDocument()).Sort(sort).ToList();
                }
                else
                {
                    ObjectId oid = new ObjectId(_lastId);
                    //documents = tweets.Find(new BsonDocument()).ToList();
                    var filter = Builders<BsonDocument>.Filter.Gt("_id", oid);
                    var sort = Builders<BsonDocument>.Sort.Ascending("_id");
                    documents = tweets.Find(filter).Sort(sort).ToList();
                    //var filter = Builders<BsonDocument>.Filter.Gt(x => x["_id"], oid);
                }

                return documents;
            }
            catch (Exception ee)
            {
                //MessageBox.Show(ee.Message);
                //JSRuntime.InvokeAsync<string>("alert", ee.Message);
            }

            return null;
        }
        private static string GetLastMongoImportedId(SqlConnection con)
        {
            string lastId;
            using (SqlCommand cmd = new SqlCommand("SELECT [LastImportedMongoId] FROM [MediaStat].[dbo].[GeneralConfig]", con))
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                object objReturn = cmd.ExecuteScalar();
                if (objReturn != null)
                {
                    lastId = objReturn.ToString();
                }
                else
                {
                    lastId = string.Empty;
                }

                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();

                return lastId;
            }
        }

        private static void UpdateLastMongoImportedId(SqlConnection con)
        {
            string lastId;
            using (SqlCommand cmd = new SqlCommand("Update [MediaStat].[dbo].[GeneralConfig] SET [LastImportedMongoId] = '" + _lastId + "'", con))
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                cmd.ExecuteNonQuery();

                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();

            }
        }



    }
}
