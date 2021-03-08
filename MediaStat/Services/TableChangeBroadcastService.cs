using MediaStat.Data;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

namespace MediaStat.Services
{
    public class TableChangeBroadcastService : TweetsChangeEventArgs.ITableChangeBroadcastService
    {
        private const string TableName = "TweetMain";
        private SqlTableDependency<TweetMain> _notifier;
        private IConfiguration _configuration;

        public event TweetsChangeDelegate OnTweetsChanged;

        public TableChangeBroadcastService(IConfiguration configuration)
        {
            _configuration = configuration;

            // SqlTableDependency will trigger an event 
            // for any record change on monitored table  
            _notifier = new SqlTableDependency<TweetMain>(
                 MyAppData.MyConnectionString,
                 TableName);
            _notifier.OnChanged += this.TableDependency_Changed;
            _notifier.Start();
        }

        // This method will notify the Blazor component about the TweetMain price change TweetMain
        private void TableDependency_Changed(object sender, RecordChangedEventArgs<TweetMain> e)
        {
            this.OnTweetsChanged(this, new TweetsChangeEventArgs(e.Entity, e.EntityOldValues));
        }

        // This method is used to populate the HTML view 
        // when it is rendered for the first time
        public IList<TweetMain> GetCurrentValues()
        {
            var result = new List<TweetMain>();

            using (var sqlConnection = new SqlConnection(MyAppData.MyConnectionString))
            {
                sqlConnection.Open();

                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM " + TableName;
                    command.CommandType = CommandType.Text;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result.Add(new TweetMain
                                {
                                    FullText = reader.GetString(reader.GetOrdinal("FullText")),
                                    SpecialText = reader.GetString(reader.GetOrdinal("SpecialText")),
                                    LikesCount = reader.GetInt32(reader.GetOrdinal("LikesCount"))
                                });
                            }
                        }
                    }
                }
            }

            return result;
        }

        public void Dispose()
        {
            _notifier.Stop();
            _notifier.Dispose();
        }
    }
}
