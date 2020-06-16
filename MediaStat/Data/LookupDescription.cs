using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStat.Data
{
    public class LookupDescription
    {
        [Key]
        public int LookupDescriptionId { get; set; }
        [Required]
        public string LookupDescriptionDetail { get; set; }

        public LookupCode LokkupCode { get; set; }
        [Required]
        public int LookupCodeId { get; set; }
    }
}
