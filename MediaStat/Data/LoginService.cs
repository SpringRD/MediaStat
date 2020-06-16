using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStat.Data
{
    public class LoginService
    {
        private readonly ApplicationDbContext _db;

        public LoginService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<LoginRequest> GetAllUsers()
        {
            var users = _db.LoginRequests.ToList();
            return users;
        }

        public string CreateUser(LoginRequest objLoginRequest)
        {
            objLoginRequest.Id = "test";
            objLoginRequest.AccessToken = "test";
            _db.LoginRequests.Add(objLoginRequest);
            _db.SaveChanges();
            return "Save successfully";
        }

        public LoginRequest GetUserById(int id)
        {
            LoginRequest user = _db.LoginRequests.FirstOrDefault(s => s.LoginRequestId == id);
            return user;
        }

        public LoginRequest GetUserByEmailAndPassword(string email,string password)
        {
            LoginRequest user = _db.LoginRequests.FirstOrDefault(s => string.Compare(s.Email, email) ==0 && string.Compare(s.Password, password) == 0);
            return user;
        }

        public string UpdateUser(LoginRequest objUser)
        {
            _db.LoginRequests.Update(objUser);
            _db.SaveChanges();
            return "Update successfully";
        }

        public string CancelUpdateUser(LoginRequest objUser)
        {
            var userEntry = _db.Entry(objUser);
            userEntry.CurrentValues.SetValues(userEntry.OriginalValues);
            userEntry.State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
            return "Cancel Update successfully";
        }

        public string DeleteEmpInfo(LoginRequest objUser)
        {
            _db.Remove(objUser);
            _db.SaveChanges();
            return "Update successfully";
        }

    }
}
