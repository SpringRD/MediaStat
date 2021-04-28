using MediaStat.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStat.Data
{
    public class TweetHashtagDimervice
    {
        private readonly ApplicationDbContext _db;

        public TweetHashtagDimervice(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task< List<TweetHashtagDim>> GetHashtags()
        {
            return _db.TweetHashtagDim.ToList();
        }

        public string Create(TweetHashtagDim objHashtags)
        {
            _db.TweetHashtagDim.Add(objHashtags);
            _db.SaveChanges();
            return "Save successfully";
        }

        public TweetHashtagDim GetHashtagById(int id)
        {
            TweetHashtagDim hashtag = _db.TweetHashtagDim.FirstOrDefault(s => s.Id == id);
            return hashtag;
        }

        public string UpdateHashtag(TweetHashtagDim objHashtags)
        {
            _db.TweetHashtagDim.Update(objHashtags);
            _db.SaveChanges();
            return "Update successfully";
        }       
        
        public string CancelUpdateHashtag(TweetHashtagDim objHashtags)
        {
            var accEntry = _db.Entry(objHashtags);
            accEntry.CurrentValues.SetValues(accEntry.OriginalValues);
            accEntry.State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
            return "Cancel Update successfully";
        }

        public string DeleteHashtagInfo(TweetHashtagDim objHashtags)
        {
            _db.TweetHashtagDim.Remove(objHashtags);
            _db.SaveChanges();
            return "Update successfully";
        }


    }
}
