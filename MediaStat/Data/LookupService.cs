using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStat.Data
{
    public class LookupService
    {
        private readonly ApplicationDbContext _db;
        //private IEnumerable<LookupDescription> locations;
        //private IEnumerable<LookupDescription> parties;
        //private IEnumerable<LookupDescription> accountTypes;
        //private IEnumerable<LookupDescription> classification1s;
        //private IEnumerable<LookupDescription> classification2s;

        public IEnumerable<LookupDescription> Locations { get; set; }
        public IEnumerable<LookupDescription> Parties { get; set; }
        public IEnumerable<LookupDescription> AccountTypes { get; set; }
        public IEnumerable<LookupDescription> Classification1s { get; set; }
        public IEnumerable<LookupDescription> Classification2s { get; set; }

        public LookupService(ApplicationDbContext db)
        {
            _db = db;
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

        public List<LookupDescription> GetLookupDescriptionsByCodeId(int lookupId)
        {
            var lkpDesList = from item in _db.LookupDescriptions.ToList()
                             where item.LookupCodeId == lookupId
                             select item;
            return lkpDesList.ToList();
        }

        public LookupCode GetLookupCodeByCodeId(int lookupId)
        {
            var lkpDesList = from item in _db.LookupCodes.ToList()
                             where item.LookupCodeId == lookupId
                             select item;
            return lkpDesList.FirstOrDefault();
        }

        public void FillAllLookups()
        {
            Locations = from item in _db.LookupDescriptions.ToList()
                             where item.LookupCodeId == 1
                             select item;
            Parties = from item in _db.LookupDescriptions.ToList()
                        where item.LookupCodeId == 2
                        select item;
            AccountTypes = from item in _db.LookupDescriptions.ToList()
                        where item.LookupCodeId == 3
                        select item;
            Classification1s = from item in _db.LookupDescriptions.ToList()
                        where item.LookupCodeId == 4
                        select item;
            Classification2s = from item in _db.LookupDescriptions.ToList()
                        where item.LookupCodeId == 5
                        select item;
        }


        //public string UpdateLookupDescription(LookupCode objLookup)
        //{
        //    _db.LookupCodes.Update(objLookup);
        //    _db.SaveChanges();
        //    return "Update successfully";
        //}

        public string UpdateLookupDescription(LookupDescription objLookupDescription)
        {
            _db.LookupDescriptions.Update(objLookupDescription);
            _db.SaveChanges();
            return "Update successfully";
        }

        public string DeleteLookupDescription(LookupDescription objLookupDescription)
        {
            _db.LookupDescriptions.Remove(objLookupDescription);
            _db.SaveChanges();
            return "Update successfully";
        }

        public string CancelUpdateLookupCode(LookupCode objLookup)
        {
            var accEntry = _db.Entry(objLookup);
            accEntry.CurrentValues.SetValues(accEntry.OriginalValues);
            accEntry.State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
            return "Cancel Update successfully";
        }

        public string CancelUpdateLookupDescription(LookupDescription objLookupDescription)
        {
            var accEntry = _db.Entry(objLookupDescription);
            accEntry.CurrentValues.SetValues(accEntry.OriginalValues);
            accEntry.State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
            return "Cancel Update successfully";
        }




    }
}
