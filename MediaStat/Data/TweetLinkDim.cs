using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStat.Data
{
    public class TweetLinkDim
    {

        [Key]
        public int Id { get; set; }

        public string LinkText { get; set; }

    }
}
