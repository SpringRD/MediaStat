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
using Serilog;
using System.Text;

namespace MediaStat.Data.Services
{
    public class MongodbImportTweetsClass
    {
        private static string _lastId;
        private static DataTable dtAccounts = new DataTable();

        public static async Task<string> ImportTweetsfromMongodb()
        {
            // TODO: Add your code here

            int lineNumber = 0;
            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
            SqlConnection myADONETConnection = null;
            int counter = 0;

            try
            {

                Log.Information($"START IMPORTING TWEETES FROM MONGO AT: " + DateTime.Now.ToString());

                //Declare Variables
                string FileDelimiter = ""; // Dts.Variables["User::FileDelimiter"].Value.ToString();
                FileDelimiter = FileDelimiter.Replace("\\t", "\t");
                string query;

                DataTable dtDates = new DataTable();
                DataTable dtTimes = new DataTable();
                SqlCommand myCommand1;


                myADONETConnection = new SqlConnection(MyAppData.MyConnectionString);
                myADONETConnection.Open();

                query = "SELECT [AccountId],[ScreenName],[SpecialAccountId] FROM [MediaStat].[dbo].[Accounts]";
                myCommand1 = new SqlCommand(query, myADONETConnection);
                SqlDataReader drAccounts = myCommand1.ExecuteReader(CommandBehavior.CloseConnection);
                dtAccounts = new DataTable();
                dtAccounts.Load(drAccounts);

                query = "SELECT [Id],[DayDate] FROM [MediaStat].[dbo].[TweetDateDim]";
                myADONETConnection.Open();
                myCommand1 = new SqlCommand(query, myADONETConnection);
                SqlDataReader drDates = myCommand1.ExecuteReader(CommandBehavior.CloseConnection);
                dtDates = new DataTable();
                dtDates.Load(drDates);

                query = "SELECT [TimeKey],[Hour30],[MinuteNumber],[SecondNumber] FROM [MediaStat].[dbo].[TweetTimeDim]";
                myADONETConnection.Open();
                myCommand1 = new SqlCommand(query, myADONETConnection);
                SqlDataReader drTimes = myCommand1.ExecuteReader(CommandBehavior.CloseConnection);
                dtTimes = new DataTable();
                dtTimes.Load(drTimes);



                //var documents = GetTweetsfromCollection();

                List<BsonDocument> documents;
                _lastId = GetToStartObjectIdFromMongo();
                //_lastId = GetLastMongoImportedId(myADONETConnection);
                if (string.IsNullOrEmpty(_lastId))
                {
                    documents = GetTweetsfromCollection(true);
                }
                else
                {
                    documents = GetTweetsfromCollection(false);
                }


                foreach (var doc in documents)
                {
                    string link1 = null; //string.Empty;
                    string link2 = null; //string.Empty;
                    string hashtag1 = null; // string.Empty;
                    string hashtag2 = null; // string.Empty;
                    string hashtag3 = null; // string.Empty;
                    string mention1 = null; //string.Empty;
                    string mention2 = null; //string.Empty;


                    if (doc.Count() > 11)
                    {
                        var urlArray = (BsonArray)doc[11];

                        if (urlArray != null && urlArray.Count > 0)
                        {
                            link1 = urlArray[0]["url"].ToString();
                        }
                        if (urlArray != null && urlArray.Count > 1)
                        {
                            link2 = urlArray[1]["url"].ToString();

                        }
                    }


                    if (doc.Count() > 10)
                    {
                        var hashtagArray = (BsonArray)doc[10];
                        if (hashtagArray != null && hashtagArray.Count > 0)
                        {
                            hashtag1 = hashtagArray[0]["text"].ToString();
                        }
                        if (hashtagArray != null && hashtagArray.Count > 1)
                        {
                            hashtag2 = hashtagArray[1]["text"].ToString();
                        }
                        if (hashtagArray != null && hashtagArray.Count > 2)
                        {
                            hashtag3 = hashtagArray[2]["text"].ToString();
                        }
                    }

                    if (doc.Count() > 9)
                    {
                        var mentionArray = (BsonArray)doc[9];
                        if (mentionArray != null && mentionArray.Count > 0)
                        {
                            mention1 = mentionArray[0]["screen_name"].ToString();
                        }
                        if (mentionArray != null && mentionArray.Count > 1)
                        {
                            mention2 = mentionArray[1]["screen_name"].ToString();
                        }
                    }


                    int? mentionId1 = null;
                    int? mentionId2 = null;
                    int? hashtagId1 = null;
                    int? hashtagId2 = null;
                    int? hashtagId3 = null;
                    int? linkId1 = null;
                    int? linkId2 = null;
                    int? dateId = null;
                    int? timeId = null;

                    if (!string.IsNullOrWhiteSpace(mention1))
                    {
                        var results = (from myRow in dtAccounts.AsEnumerable()
                                       where string.Compare(myRow.Field<string>("SpecialAccountId"), mention1) == 0
                                       select myRow).ToList();
                        if (results.Count > 0)
                        {
                            mentionId1 = results[0].Field<int>("AccountId");
                        }
                    }


                    //if (!string.IsNullOrWhiteSpace(mention1))
                    //{
                    //    var results = (from myRow in dtAccounts.AsEnumerable()
                    //                   where string.Compare(myRow.Field<string>("ScreenName").ToLower(), mention1.ToLower()) == 0
                    //                   select myRow).ToList();
                    //    if (results.Count == 0)
                    //    {
                    //        mentionId1 = InsertAccountByScreenName(myADONETConnection, mention1);
                    //        RefreshAccounts(myADONETConnection);
                    //    }
                    //}


                    //if (!string.IsNullOrWhiteSpace(mention2))
                    //{
                    //    var results1 = (from myRow in dtAccounts.AsEnumerable()
                    //                    where string.Compare(myRow.Field<string>("ScreenName").ToLower(), mention2.ToLower()) == 0
                    //                    select myRow).ToList();
                    //    if (results1.Count == 0)
                    //    {
                    //        mentionId2 = InsertAccountByScreenName(myADONETConnection, mention2);
                    //        RefreshAccounts(myADONETConnection);
                    //    }
                    //}


                    if (hashtag1 != null)
                    {
                        hashtagId1 = InsertHashtagDimention(myADONETConnection, hashtag1);
                    }
                    if (hashtag2 != null)
                    {
                        hashtagId2 = InsertHashtagDimention(myADONETConnection, hashtag2);
                    }
                    if (hashtag3 != null)
                    {
                        hashtagId3 = InsertHashtagDimention(myADONETConnection, hashtag3);
                    }


                    if (link1 != null)
                    {
                        linkId1 = InsertLinkDimention(myADONETConnection, link1);
                    }
                    if (link2 != null)
                    {
                        linkId2 = InsertLinkDimention(myADONETConnection, link2);
                    }


                    if (doc[5] != null)
                    {

                        //DateTime dt = DateTime.ParseExact(doc[5].ToString(), "yyyy-MM-dd HH:mm:ss", null);
                        string strDt = doc[5].ToString();


                        //OLDDDDDDDDDDDDDDDDDDDDDDD
                        //if (strDt.Contains("+0000")) strDt = strDt.Remove(strDt.IndexOf("+"), 6);
                        //DateTime dt = DateTime.ParseExact(strDt, "ddd MMM dd HH:mm:ss yyyy", CultureInfo.InvariantCulture);
                        //string s = dt.ToString("yyyy-MM-dd", null);
                        ////string strTweetTime = dt.ToString("yyyy-MM-dd HH:mm:ss", null);
                        //DateTime dtTweetDate = DateTime.ParseExact(s, "yyyy-MM-dd", null);
                        ////DateTime dtTweetTime = DateTime.ParseExact(strTweetTime, "yyyy-MM-dd HH:mm:ss", null);

                        DateTime? dtTweetDate = null;
                        DateTime? dt = null;
                        if (strDt.Contains("T") && strDt.Contains("Z"))
                        {
                            strDt = strDt.Replace("T", " ");
                            strDt = strDt.Replace("Z", "");
                            dt = DateTime.ParseExact(strDt, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            string s = dt.Value.ToString("yyyy-MM-dd", null);
                            dtTweetDate = DateTime.ParseExact(s, "yyyy-MM-dd", null);
                        }
                        else
                        {
                            if (strDt.Contains("+0000")) strDt = strDt.Remove(strDt.IndexOf("+"), 6);
                            dt = DateTime.ParseExact(strDt, "ddd MMM dd HH:mm:ss yyyy", CultureInfo.InvariantCulture);
                            string s = dt.Value.ToString("yyyy-MM-dd", null);
                            dtTweetDate = DateTime.ParseExact(s, "yyyy-MM-dd", null);
                        }

                        var tweetDate = (from myRow in dtDates.AsEnumerable()
                                         where myRow.Field<DateTime>("DayDate") == dtTweetDate
                                         select myRow).ToList();
                        //var tweetTime = (from myRow in dtTimes.AsEnumerable()
                        //                 where myRow.Field<DateTime>("DayDate") == dtTweetDate
                        //                 select myRow).ToList();

                        var tweetTime = dtTimes.AsEnumerable().FirstOrDefault(x => x.Field<byte>("Hour30") == dt.Value.Hour &&
                                        x.Field<byte>("MinuteNumber") == dt.Value.Minute && x.Field<byte>("SecondNumber") == dt.Value.Second);

                        if (tweetDate.Count != 0)
                        {
                            dateId = tweetDate[0].Field<int>("Id");
                        }
                        if (tweetDate != null)
                        {
                            timeId = tweetTime.Field<int>("TimeKey");
                        }
                    }



                    int? AccountId = null;
                    string FullText = null;
                    string SpecialText = null;
                    string TweetSpecialId = null;
                    int? LikesCount = null;
                    int? RetweetCount = null;
                    int? CommentsCount = null;

                    if (doc[2] != null)
                    {
                        var findAccount = (from myRow in dtAccounts.AsEnumerable()
                                           where string.Compare(myRow.Field<string>("SpecialAccountId"), doc[2].ToString()) == 0
                                           select myRow).ToList();
                        if (findAccount.Count != 0)
                        {
                            AccountId = findAccount[0].Field<int>("AccountId");
                        }
                        else
                        {
                            //string screenName = await UpdateProfileData(doc[2].ToString(), "");
                            //AccountId = GetAccountByScreenName(myADONETConnection, screenName);
                            AccountId = await UpdateProfileScreenNameOnly(doc[2].ToString());
                            RefreshAccounts(myADONETConnection);
                        }
                    }

                    if (doc[4] != null)
                    {
                        FullText = doc[4].ToString();
                        SpecialText = doc[4].ToString();
                    }


                    if (doc[1] != null)
                    {
                        TweetSpecialId = doc[1].ToString();
                    }

                    if (doc[1] != null)
                    {
                        LikesCount = int.Parse(doc[7].ToString());
                    }
                    if (doc[1] != null)
                    {
                        RetweetCount = int.Parse(doc[6].ToString());
                    }
                    if (doc[1] != null)
                    {
                        CommentsCount = int.Parse(doc[8].ToString());
                    }


                    //
                    string insertTweetQuery = "INSERT INTO [dbo].[TweetMain] ([AccountId],[FullText],[SpecialText],[TweetSpecialId],[LikesCount],[RetweetCount],[CommentsCount],[DateId],[TimeId],[Hashtag1],[Hashtag2],[Hashtag3],[Mention1],[Mention2],[Link1],[Link2]) VALUES (@AccountId,@FullText,@SpecialText,@TweetSpecialId,@LikesCount,@RetweetCount,@CommentsCount,@DateId,@TimeId,@Hashtag1,@Hashtag2,@Hashtag3,@Mention1,@Mention2,@Link1,@Link2)";

                    using (SqlCommand cmd = new SqlCommand(insertTweetQuery, myADONETConnection))
                    {
                        cmd.Parameters.AddWithValue("@AccountId", (AccountId != null) ? AccountId : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@FullText", (FullText != null) ? FullText : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@SpecialText", (SpecialText != null) ? SpecialText : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@TweetSpecialId", (TweetSpecialId != null) ? TweetSpecialId : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@LikesCount", (LikesCount != null) ? LikesCount : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@RetweetCount", (RetweetCount != null) ? RetweetCount : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CommentsCount", (CommentsCount != null) ? CommentsCount : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@DateId", (dateId != null) ? dateId : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@TimeId", (timeId != null) ? timeId : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Hashtag1", (hashtagId1 != null) ? hashtagId1 : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Hashtag2", (hashtagId2 != null) ? hashtagId2 : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Hashtag3", (hashtagId3 != null) ? hashtagId3 : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Mention1", (mentionId1 != null) ? mentionId1 : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Mention2", (mentionId2 != null) ? mentionId2 : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Link1", (linkId1 != null) ? linkId1 : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Link2", (linkId2 != null) ? linkId2 : (object)DBNull.Value);
                        if (myADONETConnection.State == System.Data.ConnectionState.Closed)
                            myADONETConnection.Open();

                        cmd.ExecuteNonQuery();

                        if (myADONETConnection.State == System.Data.ConnectionState.Open)
                            myADONETConnection.Close();

                    }
                    _lastId = doc[0].ToString();
                    counter++;
                }

            }
            catch (Exception exception)
            {
                int c = counter;
                Log.Information(exception.StackTrace);
            }
            finally
            {
                UpdateLastMongoImportedId(myADONETConnection);
                Log.Information($"The last id imported is ( " + _lastId + ") : " + DateTime.Now.ToString());
                Log.Information($"END IMPORTING TWEETES FROM MONGO AT: " + DateTime.Now.ToString());

                string strQuery = "Update [MediaStat].[dbo].[GeneralConfig] SET [TweetssImportEnded] = 0";
                SqlConnection cnn = new SqlConnection(MyAppData.MyConnectionString);
                SqlCommand cmd = new SqlCommand(strQuery, cnn);
                cmd.CommandType = System.Data.CommandType.Text;
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();

                if (myADONETConnection.State == ConnectionState.Open) myADONETConnection.Close();
            }
            return "Finished";

        }

        private static int InsertAccountByScreenName(SqlConnection con, string sreenName)
        {
            int modified = 0;
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Accounts(ScreenName) output INSERTED.AccountId VALUES(@ScreenName)", con))
                {
                    cmd.Parameters.AddWithValue("@ScreenName", sreenName);
                    if (con.State == System.Data.ConnectionState.Closed)
                        con.Open();

                    modified = (int)cmd.ExecuteScalar();

                    if (con.State == System.Data.ConnectionState.Open)
                        con.Close();
                }
            }

            catch (Exception exception)
            {
                Log.Information(exception.StackTrace);
            }
            return modified;
        }

        private static int GetAccountByScreenName(SqlConnection con, string sreenName)
        {
            int modified = 0;
            try
            {
                using (SqlCommand cmd = new SqlCommand("Select AccountId from Accounts where ScreenName  = '" + sreenName + "'", con))
                {
                    if (con.State == System.Data.ConnectionState.Closed)
                        con.Open();

                    modified = (int)cmd.ExecuteScalar();

                    if (con.State == System.Data.ConnectionState.Open)
                        con.Close();

                }


            }
            catch (Exception exception)
            {
                Log.Information(exception.StackTrace);
            }
            return modified;
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



        private static int InsertHashtagDimention(SqlConnection con, string hashtagText)
        {
            int id = 0;
            //Test if Hashtag exists
            using (SqlCommand cmd = new SqlCommand("select * from [dbo].[TweetHashtagDim] where [HashtagText]=@HashtagText", con))
            {
                cmd.Parameters.AddWithValue("@HashtagText", hashtagText);
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                object obj = cmd.ExecuteScalar();

                if (cmd.ExecuteScalar() != null)
                {
                    int.TryParse(obj.ToString(), out id);
                }
            }

            if (id == 0)
            {
                //INSERT INTO TweetHashtagDim(HashtagText) output INSERTED.Id VALUES(@HashtagText)
                using (SqlCommand cmd = new SqlCommand("INSERT INTO TweetHashtagDim(HashtagText) output INSERTED.Id VALUES(@HashtagText)", con))
                {
                    cmd.Parameters.AddWithValue("@HashtagText", hashtagText);

                    int modified = (int)cmd.ExecuteScalar();

                    return modified;
                }
            }

            if (con.State == System.Data.ConnectionState.Open)
                con.Close();
            return id;
        }

        private static int InsertLinkDimention(SqlConnection con, string link)
        {
            int id = 0;
            //Test if Hashtag exists
            using (SqlCommand cmd = new SqlCommand("select * from [dbo].[TweetLinkDim] where [LinkText]=@LinkText", con))
            {
                cmd.Parameters.AddWithValue("@LinkText", link);
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                object obj = cmd.ExecuteScalar();

                if (cmd.ExecuteScalar() != null)
                {
                    int.TryParse(obj.ToString(), out id);
                }
            }

            if (id == 0)
            {
                //INSERT INTO TweetHashtagDim(HashtagText) output INSERTED.Id VALUES(@HashtagText)
                using (SqlCommand cmd = new SqlCommand("INSERT INTO TweetLinkDim(LinkText) output INSERTED.Id VALUES(@LinkText)", con))
                {
                    cmd.Parameters.AddWithValue("@LinkText", link);

                    int modified = (int)cmd.ExecuteScalar();

                    return modified;
                }
            }

            if (con.State == System.Data.ConnectionState.Open)
                con.Close();
            return id;
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

        private static List<BsonDocument> GetTweetsfromCollection()
        {
            try
            {
                var dbClient = new MongoClient("mongodb://localhost:27017");

                IMongoDatabase db = dbClient.GetDatabase("crawling");

                var tweets = db.GetCollection<BsonDocument>("tweets");
                var documents = tweets.Find(new BsonDocument()).ToList();
                return documents;
            }
            catch (Exception ee)
            {

                //JSRuntime.InvokeAsync<string>("alert", ee.Message);
            }

            return null;
        }

        private static List<BsonDocument> AddImportedFieldToTweetsCollection()
        {
            try
            {
                var dbClient = new MongoClient("mongodb://localhost:27017");

                IMongoDatabase db = dbClient.GetDatabase("crawling");

                var tweets = db.GetCollection<BsonDocument>("tweets");
                //tweets.Update(Query.Null, Update.Set("HomePhoneId", "some value"), UpdateFlags.Multi)

                var documents = tweets.Find(new BsonDocument()).ToList();
                return documents;
            }
            catch (Exception ee)
            {

                //JSRuntime.InvokeAsync<string>("alert", ee.Message);
            }

            return null;
        }

        private static List<BsonDocument> GetTweetsfromCollection(bool blnAll)
        {
            string str = string.Empty;
            try
            {
                List<BsonDocument> documents;
                var dbClient = new MongoClient("mongodb://localhost:27017");
                IMongoDatabase db = dbClient.GetDatabase("crawling");
                var tweets = db.GetCollection<BsonDocument>("tweets");

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

        public static async Task<string> UpdateProfileData(string id, string screenName)
        {
            string _myConnectionString = MyAppData.MyConnectionString;
            string strQuery;
            ulong longID = 0;

            // var auth = DoSingleUserAuth();
            var auth = new ApplicationOnlyAuthorizer
            {
                CredentialStore = new InMemoryCredentialStore()
                {
                    ConsumerKey = "HYMjzOjCUQvDmRxBI8JUmpFdk",
                    ConsumerSecret = "JQxInK1ZSf1eeILr7jLwnAO8FwMWPXuCX22ZXK0S2463LeExGR"
                }
            };

            await auth.AuthorizeAsync();

            //List<string> names = new List<string>() {"naem1","namne2" };
            var twitterCtx = new TwitterContext(auth) { ReadWriteTimeout = 300 };
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    longID = (ulong)long.Parse(id);
                }

                var userResponse =
                 (from user in twitterCtx.User
                  where user.Type == UserType.Show &&
                  user.UserID == longID
                  //|| names.Contains(user.ScreenName)
                  select user).ToArray();

                var firstOrDefault = userResponse.FirstOrDefault();
                //if (firstOrDefault != null) twitterUrl = firstOrDefault.ProfileImageUrl;


                if (firstOrDefault != null & !string.IsNullOrEmpty(firstOrDefault.ScreenNameResponse))
                {
                    //_myConnectionString = "Server=.;Database=MediaStat;Trusted_Connection=True;MultipleActiveResultSets=true;";

                    strQuery = "INSERT INTO [dbo].[Accounts] ([ScreenName],[ProfileName],[Joined],[LocationDescription],[Description],[AccountUrl],[Followers]," +
                        "[Following],[Link],[ProfileImageURL],[SpecialAccountId]) VALUES (@ScreenName,@ProfileName,@Joined,@LocationDescription," +
                        "@Description,@AccountUrl,@Followers,@Following,@Link,@ProfileImageURL,@SpecialAccountId)";
                    //strQuery = "Update [dbo].[Accounts] SET [ScreenName] = @ScreenName ,[ProfileName] = @ProfileName,[Joined] = @Joined,[LocationDescription] = @LocationDescription,[Description] = @Description,[AccountUrl] = @AccountUrl,[Followers] = @Followers,[Following] = @Following,[Link] = @Link,[ProfileImageURL] = @ProfileImageURL,[SpecialAccountId] = @SpecialAccountId WHERE [SpecialAccountId] = '" + id + "'";   //VALUES (@ScreenName,@ProfileName,@Joined,@LocationDescription,@Description,@AccountUrl,@Followers,@Following,@Link,@ProfileImageURL,@SpecialAccountId)
                    //strQuery = "Update [dbo].[Accounts] SET [ScreenName] = @ScreenName ,[ProfileName] = @ProfileName,[Joined] = @Joined,[LocationDescription] = @LocationDescription,[Description] = @Description,[AccountUrl] = @AccountUrl,[Followers] = @Followers,[Following] = @Following,[Link] = @Link,[SpecialAccountId] = @SpecialAccountId,[ProfileImageURL] = @ProfileImageURL, [LastChanged] = @LastChanged WHERE [SpecialAccountId] = '" + id + "' OR [ScreenName] = '" + screenName + "'";   //VALUES (@ScreenName,@ProfileName,@Joined,@LocationDescription,@Description,@AccountUrl,@Followers,@Following,@Link,@ProfileImageURL,@SpecialAccountId)
                    SqlConnection cnn = new SqlConnection(_myConnectionString);
                    SqlCommand cmd = new SqlCommand(strQuery, cnn);
                    cmd.Parameters.AddWithValue("@ScreenName", (firstOrDefault.ScreenNameResponse != null) ? firstOrDefault.ScreenNameResponse.ToString() : string.Empty);
                    cmd.Parameters.AddWithValue("@ProfileName", (firstOrDefault.Name != null) ? firstOrDefault.Name.ToString() : string.Empty);
                    cmd.Parameters.AddWithValue("@Joined", firstOrDefault.CreatedAt);    //firstOrDefault.CreatedAt.ToString("YYYY-mm-DD")
                    cmd.Parameters.AddWithValue("@LocationDescription", (firstOrDefault.Location != null) ? firstOrDefault.Location.ToString() : string.Empty);
                    cmd.Parameters.AddWithValue("@Description", (firstOrDefault.Description != null) ? firstOrDefault.Description.ToString() : string.Empty);
                    cmd.Parameters.AddWithValue("@AccountUrl", (firstOrDefault.Url != null) ? firstOrDefault.Url.ToString() : string.Empty);
                    cmd.Parameters.AddWithValue("@Followers", firstOrDefault.FollowersCount);
                    cmd.Parameters.AddWithValue("@Following", firstOrDefault.FriendsCount);
                    cmd.Parameters.AddWithValue("@Link", (firstOrDefault.Url != null) ? firstOrDefault.Url.ToString() : string.Empty);
                    cmd.Parameters.AddWithValue("@ProfileImageURL", (firstOrDefault.ProfileImageUrl != null) ? firstOrDefault.ProfileImageUrl.ToString().Replace("normal", "400x400") : string.Empty);
                    cmd.Parameters.AddWithValue("@SpecialAccountId", firstOrDefault.UserID.ToString());
                    cmd.Parameters.AddWithValue("@LastChanged", DateTime.Now);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                    //await JsRuntime.InvokeAsync<bool>("alert", "Updated Successfully");
                }




                return firstOrDefault.ScreenNameResponse;




            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }


        public static async Task<int?> UpdateProfileScreenNameOnly(string id)
        {
            string _myConnectionString = MyAppData.MyConnectionString;
            string strQuery;
            int modified = 0;

            try
            {
                //_myConnectionString = "Server=.;Database=MediaStat;Trusted_Connection=True;MultipleActiveResultSets=true;";

                //strQuery = "INSERT INTO [dbo].[Accounts] ([ScreenName]) VALUES (@ScreenName)";
                strQuery = "INSERT INTO Accounts(SpecialAccountId) output INSERTED.AccountId VALUES(@SpecialAccountId)";
                SqlConnection cnn = new SqlConnection(_myConnectionString);
                SqlCommand cmd = new SqlCommand(strQuery, cnn);
                cmd.Parameters.AddWithValue("@SpecialAccountId", id);
                cmd.Parameters.AddWithValue("@LastChanged", DateTime.Now);
                cmd.CommandType = System.Data.CommandType.Text;
                cnn.Open();
                //cmd.ExecuteNonQuery();
                modified = (int)cmd.ExecuteScalar();
                cnn.Close();
            }
            catch (Exception e)
            {
                return modified;
            }
            return modified;
        }

        public static string GetToStartObjectIdFromMongo()
        {
            string strReturn = string.Empty;
            string _myConnectionString = MyAppData.MyConnectionString;
            StringBuilder strQuery = new StringBuilder();

            List<BsonDocument> documents;

            try
            {
                strQuery.Append("declare @maxTweetId int ");
                strQuery.Append("select @maxTweetId = MAX(TweetId) FROM [MediaStat].[dbo].[TweetMain] ");
                strQuery.Append("select [TweetSpecialId] FROM [MediaStat].[dbo].[TweetMain] where TweetId = @maxTweetId");

                SqlConnection cnn = new SqlConnection(_myConnectionString);
                SqlCommand cmd = new SqlCommand(strQuery.ToString(), cnn);
                cmd.CommandType = System.Data.CommandType.Text;
                cnn.Open();
                string strObjectId = (string)cmd.ExecuteScalar();
                cnn.Close();

                if (!string.IsNullOrEmpty(strObjectId))
                {
                    var filter = Builders<BsonDocument>.Filter.Eq("id", strObjectId);
                    var dbClient = new MongoClient("mongodb://localhost:27017");
                    IMongoDatabase db = dbClient.GetDatabase("crawling");
                    var tweets = db.GetCollection<BsonDocument>("tweets");
                    documents = tweets.Find(filter).ToList();
                    if (documents.Count > 0)
                    {
                        strReturn = documents[0][0].ToString();
                    }
                }

            }

            catch (Exception exception)
            {
                Log.Information(exception.StackTrace);
            }
            return strReturn;
        }


    }
}
