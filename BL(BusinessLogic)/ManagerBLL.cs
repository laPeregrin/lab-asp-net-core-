using BO_BusinessObjects_.ORMs;
using ObjectContainer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BL_BusinessLogic_
{
    public class ManagerBLL
    {
        public async Task<Boolean> Add(Manager newManager)
        {
            return await new ManagerDAO().Add(newManager);
        }
        public async Task<Manager> GetById(Guid id)
        {
            var manager = await new ManagerDAO().Get(id);
            return manager;
        }
        public async Task<IEnumerable<Manager>> GetAll()
        {
            var collection = await Task.Run(() =>new ManagerDAO().GetAll());
            return collection;
        }

        public async Task<Boolean> Delete(Guid id)
        {
            return await new ManagerDAO().Delete(id);
        }
    }
}
