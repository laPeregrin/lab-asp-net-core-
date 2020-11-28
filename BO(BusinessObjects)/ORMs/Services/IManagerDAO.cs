using ObjectContainer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO_BusinessObjects_.ORMs.Services
{
    public interface IManagerDAO
    {
        public Task<Manager> Get(Guid id);
        public Task<IEnumerable<Manager>> GetAll();
        public Task<bool> Add(Manager newObj);
        public Task<bool> Delete(Guid id);
        public Task<bool> Update(Manager obj);

    }
}
