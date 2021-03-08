using MediaStat.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStat.Services
{
    public delegate void TweetsChangeDelegate(object sender, TweetsChangeEventArgs args);

    public class TweetsChangeEventArgs
    {
        public TweetMain NewValue { get; }
        public TweetMain OldValue { get; }

        public TweetsChangeEventArgs(TweetMain newValue, TweetMain oldValue)
        {
            this.NewValue = newValue;
            this.OldValue = oldValue;
        }

        public interface ITableChangeBroadcastService : IDisposable
        {
            event TweetsChangeDelegate OnTweetsChanged;
            IList<TweetMain> GetCurrentValues();
        }
    }
}
