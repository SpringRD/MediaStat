using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStat.Data
{
    public class AccountLink
    {
        [Key]
        public int AccountLinkId { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        //[Required(ErrorMessage = "This field is required")]
        [DisplayName("Link Description")]
        public string LinkDescription { get; set; }
        //[Required]
        //public int AccountId { get; set; }
        //public AccountInfo AccountInfo { get; set; }

        public AccountInfo AccountInfo { get; set; }
        [Required]
        public int AccountInfoId { get; set; }


    }
}
