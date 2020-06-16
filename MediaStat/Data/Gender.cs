using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStat.Data
{
    public class Gender
    {
        [Key]
        public int GenderId { get; set; }
        [Required]
        public string GenderDescription { get; set; }
    }
}
