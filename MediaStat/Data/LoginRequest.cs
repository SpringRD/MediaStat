using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStat.Data
{
    public class LoginRequest
    {
        [Key]
        public int LoginRequestId { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }


        [StringLength(4000)]
        public string AccessToken { get; set; }

        [StringLength(4000)]
        public string Id { get; set; }

        public bool IsAdmin { get; set; }


        public LoginRequest()
        {
            IsAdmin = false;
        }
    }
}
