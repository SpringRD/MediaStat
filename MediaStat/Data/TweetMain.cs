using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStat.Data
{
    public class TweetMain
    {

        [Key]
        public int TweetId { get; set; }
        public int AccountId { get; set; }

        public string FullText { get; set; }
        public string SpecialText { get; set; }
        public string TweetSpecialId { get; set; }
        public int LikesCount { get; set; }
        public int RetweetCount { get; set; }
        public int CommentsCount { get; set; }
        public int DateId { get; set; }
        public int TimeId { get; set; }
        public int Hashtag1 { get; set; }
        public int Hashtag2 { get; set; }
        public int Hashtag3 { get; set; }
        public int Mention1 { get; set; }
        public int Mention2 { get; set; }
        public int Link1 { get; set; }
        public int Link2 { get; set; }

    }
}
