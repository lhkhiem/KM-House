using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class ContactDao
    {
        DBContext db = null;
        public ContactDao()
        {
            db = new DBContext();
        }
        public IEnumerable<Contact> ListAll()
        {
            return db.Contacts.ToList();
        }
        public bool Insert(Contact entity)
        {
            try
            {
                entity.Status = false;
                db.Contacts.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Update(Contact entity)
        {
            try
            {
                var model = db.Contacts.Find(entity.ID);
                model.Name = entity.Name;
                model.Content = entity.Content;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public Contact GetByID(int id)
        {
            return db.Contacts.Find(id);
        }
        public bool Delete(int id)
        {
            try
            {
                var contact = db.Contacts.Find(id);
                if (contact.Status != true)
                {
                    db.Contacts.Remove(contact);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ChangeStatus(long id)
        {
            var contact = db.Contacts.Find(id);
            var listContact = db.Contacts.ToList();
            contact.Status = true;
            foreach(var item in listContact)
            {
                if ((item.ID)!=id)
                    item.Status = false;
            }
            db.SaveChanges();
            return contact.Status;
        }

    }
}
