using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class UnitDao
    {
        DBContext db = null;
        public UnitDao()
        {
            db = new DBContext();
        }
        public IEnumerable<Unit> ListAll()
        {
            return db.Units.ToList();
        }
        public byte Insert(Unit entity)
        {
            db.Units.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public byte Update(Unit entity)
        {
            var model = db.Units.Find(entity.ID);
            model.Name = entity.Name;
            db.SaveChanges();
            return entity.ID;
        }
        public Unit GetByID(int id)
        {
            return db.Units.Find(id);
        }
        public bool CheckIsUsed(int id)
        {
            var ck = db.Products.Where(x => x.UnitID == id);
            if (ck.Count() > 0) return true;
            else return false;
        }
        public bool Delete(int id)
        {
            try
            {
                var unit = db.Units.Find(id);
                db.Units.Remove(unit);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
