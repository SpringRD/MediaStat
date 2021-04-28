using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStat.Data
{
    public class TweetHashtagDim
    {


        [Key]
        public int Id { get; set; }

        [StringLength(4000)]
        public string HashtagText { get; set; }

        [DisplayName("Classification")]
        public int? Classification { get; set; }

        [DisplayName("FreeClassification")]
        [StringLength(4000)]
        public string FreeClassification { get; set; }

        [DisplayName("Description")]
        [StringLength(4000)]
        public string Description { get; set; }



    }
}
