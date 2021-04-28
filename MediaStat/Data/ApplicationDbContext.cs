using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStat.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        //public DbSet<EmployeeInfo> Employees { get; set; }
        //public DbSet<Gender> Genders { get; set; }
        public DbSet<LookupCode> LookupCodes { get; set; }
        public DbSet<LookupDescription> LookupDescriptions { get; set; }

        public DbSet<AccountInfo> Accounts { get; set; }

        public DbSet<AccountLink> AccountLinks { get; set; }
        
        public DbSet<LoginRequest> LoginRequests { get; set; }
        
        public DbSet<TweetHashtagDim> TweetHashtagDim { get; set; }
    }
}
