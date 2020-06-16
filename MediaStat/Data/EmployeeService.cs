using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStat.Data
{
    public class EmployeeService
    {

        private readonly ApplicationDbContext _db;

        public EmployeeService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<EmployeeInfo> GetEmployee()
        {
            var empList = _db.Employees.ToList();
            return empList;
        }

        public string Create(EmployeeInfo objEmployee)
        {
            _db.Employees.Add(objEmployee);
            _db.SaveChanges();
            return "Save successfully";
        }

        public EmployeeInfo GetEmployeeById(int id)
        {
            EmployeeInfo employee = _db.Employees.FirstOrDefault(s => s.EmployeeId == id);
            return employee;
        }

        public string UpdateEmployee(EmployeeInfo objEmployee)
        {
            _db.Employees.Update(objEmployee);
            _db.SaveChanges();
            return "Update successfully";
        }

        public string DeleteEmpInfo(EmployeeInfo objEmployee)
        {
            _db.Remove(objEmployee);
            _db.SaveChanges();
            return "Update successfully";
        }

        public List<Gender> GetGenders()
        {
            var gnrList = _db.Genders.ToList();
            return gnrList;
        }

        public List<LookupCode> GetLookupCodes()
        {
            var codesList = _db.LookupCodes.ToList();
            return codesList;
        }

        public List<LookupDescription> GetLookupDescriptions()
        {
            var lkpDesList = _db.LookupDescriptions.ToList();
            return lkpDesList;
        }

        public string CreateLookupDetail(LookupDescription objLookupDescription)
        {
            _db.LookupDescriptions.Add(objLookupDescription);
            _db.SaveChanges();
            return "Save successfully";
        }

    }
}
