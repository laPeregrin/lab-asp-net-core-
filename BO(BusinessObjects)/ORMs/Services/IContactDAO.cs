using ObjectContainer.Objects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BO_BusinessObjects_.ORMs.Services
{
    public interface IContactDAO
    {
        public Contact Get(Guid id);
        public ICollection<Contact> GetAll();
        public Contact Add(Contact newObj);
        public int Update(Contact newObj);
        public bool Delete(Guid id);
        public ICollection<Contact> getItems();
        public ICollection<Contact> getVisibleItems();
        public Contact getItem(SqlDataReader objReader);
        public void SetParameters(Contact obj);
    }
}
