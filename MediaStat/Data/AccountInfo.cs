using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStat.Data
{
    public class AccountInfo
    {
        [Key]
        public int AccountId { get; set; }
        //[Column(TypeName = "nvarchar(250)")]
        //[Required(ErrorMessage = "This field is required")]
        [DisplayName("Screen Name")]
        public string ScreenName { get; set; }

        //[Column(TypeName = "nvarchar(250)")]
        [DisplayName("Profile Name")]
        public string ProfileName { get; set; }

        //[Column(TypeName = "date")]
        [DisplayName("Joined")]
        public DateTime? Joined { get; set; }

        //[Column(TypeName = "date")]
        [DisplayName("Date Of Birth")]
        public DateTime? DateOfBirth { get; set; }

        //[Column(TypeName = "int")]
        [DisplayName("Location")]
        public int? Location { get; set; }

        //[Column(TypeName = "int")]
        [DisplayName("LocationDescription")]
        public string LocationDescription { get; set; }

        //[Column(TypeName = "nvarchar(MAX)")]
        [DisplayName("Description")]
        public string Description { get; set; }

        //[Column(TypeName = "int")]
        [DisplayName("Party")]
        public int? Party { get; set; }

        //[Column(TypeName = "int")]
        [DisplayName("Account type")]
        public int? AccountType { get; set; }

        //[Column(TypeName = "nvarchar(4000)")]
        [DisplayName("Account Url")]
        public string AccountUrl { get; set; }

        //[Column(TypeName = "int")]
        [DisplayName("Classification1")]
        public int? Classification1 { get; set; }

        //[Column(TypeName = "int")]
        [DisplayName("Classification2")]
        public int? Classification2 { get; set; }

        //[Column(TypeName = "int")]
        [DisplayName("Followers")]
        public int? Followers { get; set; }

        //[Column(TypeName = "int")]
        [DisplayName("Following")]
        public int? Following { get; set; }

        //[Column(TypeName = "nvarchar(4000)")]
        [DisplayName("Link")]
        public string Link { get; set; }

        //[Column(TypeName = "nvarchar(MAX)")]
        [DisplayName("Links")]
        public string AllLinks { get; set; }

        //[Column(TypeName = "nvarchar(4000)")]
        [DisplayName("ProfileImageURL")]
        public string ProfileImageURL { get; set; }

        [Column(TypeName = "nvarchar(4000)")]
        [DisplayName("SpecialAccountId")]
        public string SpecialAccountId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [DisplayName("Account Phone")]
        public string AccountPhone { get; set; }

        [DisplayName("LastChanged")]
        public DateTime? LastChanged { get; set; }
        //


        public ICollection<AccountLink> AccountLinks { get; set; }
    }
}
