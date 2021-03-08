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
                dtAccounts = new DataTable();
                dtAccounts.Load(drAccounts);


                //var documents = GetTweetsfromCollection();

                List<BsonDocument> documents;
                _lastId = GetLastMongoImportedId(myADONETConnection);
                if (string.IsNullOrEmpty(_lastId))
                {
                    documents = GetProfilesfromCollection(true);
                }
                else
                {
                    documents = GetProfilesfromCollection(false);
                }

                foreach (var doc in documents)
                {
                    if (doc["screen_name"] != null)
                    {
                        var findAccountByScreenName = (from myRow in dtAccounts.AsEnumerable()
                                           where string.Compare(myRow.Field<string>("ScreenName"), doc["screen_name"].ToString()) == 0
                                           select myRow).ToList();

                        var findAccountById = (from myRow in dtAccounts.AsEnumerable()
                                               where string.Compare(myRow.Field<string>("SpecialAccountId"), doc["id"].ToString()) == 0
                                               select myRow).ToList();

                        if (findAccountByScreenName.Count == 0 && findAccountById.Count == 0)
                        {
                            InsertAccountByScreenName(myADONETConnection, doc);
                        }
                        else if(findAccountByScreenName.Count > 0)
                        {
                            UpdateAccountByScreenName(myADONETConnection, doc);
                        }
                        else if (findAccountById.Count > 0)
                        {
                            UpdateAccountByAccountId(myADONETConnection, doc);
                        }
                        else
                        {

                        }

                            _lastId = doc[0].ToString();
                    }
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
            string imageProfile = doc["profile_image_url"].ToString().Replace("_normal", "_400x400");

            //using (SqlCommand cmd = new SqlCommand("INSERT INTO Accounts(ScreenName,ProfileName,LocationDescription,Description,Followers,Following,ProfileImageURL,SpecialAccountId) VALUES(@ScreenName,@ProfileName,@LocationDescription,@Description,@Followers,@Following,@ProfileImageURL,@SpecialAccountId)", con))
            using (SqlCommand cmd = new SqlCommand("INSERT INTO Accounts(ScreenName,ProfileName,LocationDescription,Description,Followers,Following,SpecialAccountId,ProfileImageURL) VALUES(@ScreenName,@ProfileName,@LocationDescription,@Description,@Followers,@Following,@SpecialAccountId,@ProfileImageURL)", con))
            {
                cmd.Parameters.AddWithValue("@ScreenName", doc["screen_name"].ToString());
                cmd.Parameters.AddWithValue("@ProfileName", doc["name"].ToString());
                cmd.Parameters.AddWithValue("@LocationDescription", doc["location"].ToString());
                cmd.Parameters.AddWithValue("@Description", doc["description"].ToString());
                cmd.Parameters.AddWithValue("@Followers", int.Parse(doc["followers_count"].ToString()));
                cmd.Parameters.AddWithValue("@Following", int.Parse(doc["favourites_count"].ToString()));
                cmd.Parameters.AddWithValue("@ProfileImageURL", imageProfile);
                cmd.Parameters.AddWithValue("@SpecialAccountId", doc["id"].ToString());
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                cmd.ExecuteNonQuery();

                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();


            }
        }

        private static void UpdateAccountByScreenName(SqlConnection con, BsonDocument doc)
        {
            string imageProfile = doc["profile_image_url"].ToString().Replace("_normal", "_400x400");

            //using (SqlCommand cmd = new SqlCommand("INSERT INTO Accounts(ScreenName,ProfileName,LocationDescription,Description,Followers,Following,ProfileImageURL,SpecialAccountId) VALUES(@ScreenName,@ProfileName,@LocationDescription,@Description,@Followers,@Following,@ProfileImageURL,@SpecialAccountId)", con))
            using (SqlCommand cmd = new SqlCommand("Update Accounts SET ProfileName= @ProfileName,LocationDescription= @LocationDescription,Description= @Description,Followers= @Followers,Following= @Following,SpecialAccountId= @SpecialAccountId,ProfileImageURL= @ProfileImageURL  WHERE ScreenName = @ScreenName", con))
            {
                cmd.Parameters.AddWithValue("@ScreenName", doc["screen_name"].ToString());
                cmd.Parameters.AddWithValue("@ProfileName", doc["name"].ToString());
                cmd.Parameters.AddWithValue("@LocationDescription", doc["location"].ToString());
                cmd.Parameters.AddWithValue("@Description", doc["description"].ToString());
                cmd.Parameters.AddWithValue("@Followers", int.Parse(doc["followers_count"].ToString()));
                cmd.Parameters.AddWithValue("@Following", int.Parse(doc["favourites_count"].ToString()));
                cmd.Parameters.AddWithValue("@ProfileImageURL", imageProfile);
                cmd.Parameters.AddWithValue("@SpecialAccountId", doc["id"].ToString());
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                cmd.ExecuteNonQuery();

                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }


        private static void UpdateAccountByAccountId(SqlConnection con, BsonDocument doc)
        {
            string imageProfile = doc["profile_image_url"].ToString().Replace("_normal", "_400x400");

            //using (SqlCommand cmd = new SqlCommand("INSERT INTO Accounts(ScreenName,ProfileName,LocationDescription,Description,Followers,Following,ProfileImageURL,SpecialAccountId) VALUES(@ScreenName,@ProfileName,@LocationDescription,@Description,@Followers,@Following,@ProfileImageURL,@SpecialAccountId)", con))
            using (SqlCommand cmd = new SqlCommand("Update Accounts SET ProfileName= @ProfileName,LocationDescription= @LocationDescription,Description= @Description,Followers= @Followers,Following= @Following,ScreenName = @ScreenName,ProfileImageURL= @ProfileImageURL  WHERE SpecialAccountId = @SpecialAccountId", con))
            {
                cmd.Parameters.AddWithValue("@ScreenName", doc["screen_name"].ToString());
                cmd.Parameters.AddWithValue("@ProfileName", doc["name"].ToString());
                cmd.Parameters.AddWithValue("@LocationDescription", doc["location"].ToString());
                cmd.Parameters.AddWithValue("@Description", doc["description"].ToString());
                cmd.Parameters.AddWithValue("@Followers", int.Parse(doc["followers_count"].ToString()));
                cmd.Parameters.AddWithValue("@Following", int.Parse(doc["favourites_count"].ToString()));
                cmd.Parameters.AddWithValue("@ProfileImageURL", imageProfile);
                cmd.Parameters.AddWithValue("@SpecialAccountId", doc["id"].ToString());
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
            using (SqlCommand cmd = new SqlCommand("SELECT [LastImportedProfilesMongoId] FROM [MediaStat].[dbo].[GeneralConfig]", con))
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
            using (SqlCommand cmd = new SqlCommand("Update [MediaStat].[dbo].[GeneralConfig] SET [LastImportedProfilesMongoId] = '" + _lastId + "'", con))
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
