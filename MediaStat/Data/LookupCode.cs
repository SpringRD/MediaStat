using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStat.Data
{
    public class LookupCode
    {
        [Key]
        public int LookupCodeId { get; set; }
        [Required]
        public string LookupCodeDescription { get; set; }

        public ICollection<LookupDescription> LookupDescriptions { get; set; }

    }
}
