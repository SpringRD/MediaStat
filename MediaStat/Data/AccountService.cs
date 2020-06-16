using MediaStat.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStat.Data
{
    public class AccountService
    {
        private readonly ApplicationDbContext _db;

        public AccountService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<AccountInfo> GetAccount()
        {
            var empList = _db.Accounts.ToList();
            var accountLinksList = _db.AccountLinks.ToList();
            var newEmpList = new List<AccountInfo>();
            foreach (var emp in empList)
            {
                emp.AccountLinks = accountLinksList.Where(s => s.AccountInfoId == emp.AccountId).ToList();
                newEmpList.Add(emp);
            }
            return newEmpList;
        }

        public string Create(AccountInfo objAccount)
        {
            _db.Accounts.Add(objAccount);
            _db.SaveChanges();
            return "Save successfully";
        }

        public AccountInfo GetAccountById(int id)
        {
            AccountInfo Account = _db.Accounts.FirstOrDefault(s => s.AccountId == id);
            return Account;
        }

        public string UpdateAccount(AccountInfo objAccount)
        {
            _db.Accounts.Update(objAccount);
            _db.SaveChanges();
            return "Update successfully";
        }       
        
        public string CancelUpdateAccount(AccountInfo objAccount)
        {
            var accEntry = _db.Entry(objAccount);
            accEntry.CurrentValues.SetValues(accEntry.OriginalValues);
            accEntry.State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
            return "Cancel Update successfully";
        }

        public string DeleteAccountInfo(AccountInfo objAccount)
        {
            _db.Remove(objAccount);
            _db.SaveChanges();
            return "Update successfully";
        }



        public string CancelUpdateAccountLink(AccountLink objAccountLink)
        {
            var accEntry = _db.Entry(objAccountLink);
            accEntry.CurrentValues.SetValues(accEntry.OriginalValues);
            accEntry.State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
            return "Cancel Update successfully";
        }


    }
}
