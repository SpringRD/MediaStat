using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStat.Data
{
    public class EmployeeInfo
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public int GenderId { get; set; }

        public int Classification1 { get; set; }
        public int Classification2 { get; set; }
    }
}
