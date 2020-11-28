using ObjectContainer.Objects;
using BO_BusinessObjects_.ORMs.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BO_BusinessObjects_.ORMs
{
    public class EFContact_ORM 
    {
        private EfDAO _service;

        public int Add(Contact newObj)
        {
            try
            {
                using (_service = new EfDAO())
                {
                    if (newObj != null)
                    {
                        _service.Contacts.Add(newObj);
                        _service.SaveChanges();
                        return 1;
                    }
                    return 0;
                }

            }
            catch
            {
                /////////
                return 0;
            }
        }

        public int Delete(Guid id)
        {
            try
            {
                using (_service = new EfDAO())
                {
                    var resObj = _service.Contacts.FirstOrDefault(x => x.id == id);
                    if (resObj != null)
                    {
                        _service.Remove(resObj);
                        _service.SaveChanges();
                        return 1;
                    }
                }
                return 0;
            }
            catch
            {
                /////////
                return 0;
            }
        }

        public Contact Get(Guid id)
        {  
            try
            {
                using (_service = new EfDAO())
                {
                    var resObj = _service.Contacts.FirstOrDefault(x => x.id == id);
                    if (resObj != null)
                    {

                        return resObj;
                    }
                }
                return null;
            }
            catch
            {
                ////////////
                return null;
            }
        }

        public ICollection<Contact> GetAll()
        {
            using (_service = new EfDAO())
            {
               var collection = _service.Contacts.ToList();
               return collection;
            }
        }

        public Contact getItem(SqlDataReader objReader)
        {
            throw new NotImplementedException();
        }

        public ICollection<Contact> getItems()
        {
            throw new NotImplementedException();
        }

        public ICollection<Contact> getVisibleItems()
        {
            throw new NotImplementedException();
        }

        public void SetParameters(Contact obj)
        {
            throw new NotImplementedException();
        }

        public int Update(Contact newObj)
        {
            using (_service = new EfDAO())
            {
                try
                {
                    if(newObj != null)
                    {
                        _service.Contacts.Update(newObj);
                        return 1;
                    }
                    return 0;
                }
                catch
                {
                    return 0;
                }

            }
        }
    }
}
