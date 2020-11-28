using ObjectContainer.Objects;
using BO_BusinessObjects_.ORMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BL_BusinessLogic_
{
    public class ContactBLL 
    {
        public IEnumerable<Contact> GetAllVisible(Expression<Func<Contact, bool>> expression)
        {
            var collection = new ContactDAO().getVisibleItems();
            return collection;
        }
        public ICollection<Contact> GetAll()
        {
            return new ContactDAO().GetAll();
        }
        public Contact GetById(Guid iD)
        {
            return new ContactDAO().Get(iD);
        }
        public Contact GetById(string iD)
        {
            return new ContactDAO().Get((new Guid(iD)));
        }
        public ICollection<Contact> GetByIdAsCollection(Guid sId)
        {
            Contact item = new ContactDAO().Get(sId);
            ICollection<Contact> ContactCollection = new List<Contact>();
            ContactCollection.Add(item);
            return ContactCollection;
        }
        public bool Delete(Guid iD)
        {
            return new ContactDAO().Delete(iD);
        }
        public bool Delete(string iD)
        {
            return new ContactDAO().Delete(new Guid(iD));
        }
        public int Update(Contact contact)
        {
            return new ContactDAO().Update(contact);
        }
        public Contact Add(Contact contact)
        {
            return new ContactDAO().Add(contact);
        }

    }
}
