using BO_BusinessObjects_.ORMs.Services;
using ObjectContainer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace BO_BusinessObjects_.ORMs
{
    public class ManagerDAO : BasedDAO, IManagerDAO
    {
        public  async Task<bool> Add(Manager newObj)
        {
            try
            {
                command.CommandText = "Manager_Add";
                SetParameters(newObj);
                await command.ExecuteNonQueryAsync();
                await conn.CloseAsync();
                return true;
            }
            catch(Exception e)
            {
                await conn.CloseAsync();
                return false;
            }
        }

        public async Task<Manager> Get(Guid id)
        {
            try
            {
                Manager manager = null;
                command.CommandText = "Manager_Get";
                SqlParameter objParam = command.Parameters.Add("id", SqlDbType.UniqueIdentifier);
                objParam.Value = id;
                SqlDataReader objReader = await command.ExecuteReaderAsync();
                if(await objReader.ReadAsync())
                {
                    manager = getItem(objReader);
                    await conn.CloseAsync();
                    return manager;
                }
                await conn.CloseAsync();
                return manager;
            }
            catch(Exception e)
            {
               return null;
            }
           
        }

        public async Task<IEnumerable<Manager>> GetAll()
        {
            var list = new List<Manager>();
            try
            {
                command.CommandText = "Manager_GetAll";
                var objReader = await command.ExecuteReaderAsync();
                while (await objReader.ReadAsync())
                {
                    list.Add(getItem(objReader));
                }
                await conn.CloseAsync();
                return list;
            }
            catch(Exception e)
            {
                list.Clear();
                return list;
            }
           
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                command.CommandText = "Manager_Delete";
                SqlParameter objParam = command.Parameters.Add("@id", SqlDbType.UniqueIdentifier);
                objParam.Value = id;
                await command.ExecuteNonQueryAsync();
                await conn.CloseAsync();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
            
        }



        public Manager getItem(SqlDataReader objReader)
        {
            Manager contact = new Manager();
            object ob = null;
            contact.id = new Guid(objReader.GetValue(0).ToString());
            if ((ob = objReader.GetValue(1)) != DBNull.Value)
                contact.FullName = Convert.ToString(ob);
            if ((ob = objReader.GetValue(2)) != DBNull.Value)
                contact.Email = Convert.ToString(ob);
            if ((ob = objReader.GetValue(3)) != DBNull.Value)
                contact.Phone = Convert.ToString(ob);
            if ((ob = objReader.GetValue(4)) != DBNull.Value)
                contact.Addres = Convert.ToString(ob);
            if ((ob = objReader.GetValue(5)) != DBNull.Value)
                contact.Position = Convert.ToString(ob);
            if ((ob = objReader.GetValue(6)) != DBNull.Value)
                contact.Remark = Convert.ToString(ob);
            return contact;
        }

        public void SetParameters(Manager ob)
        {
            command.Parameters.Add("@iD", SqlDbType.UniqueIdentifier).Value = ob.id;
            command.Parameters.Add("@FullName", SqlDbType.NVarChar).Value = ob.FullName;
            command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = ob.Email;
            command.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = ob.Phone;
            command.Parameters.Add("@Addres", SqlDbType.NVarChar).Value = ob.Addres;
            command.Parameters.Add("@Position", SqlDbType.NVarChar).Value = ob.Position;
            command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = ob.Remark;
        }

        public async Task<bool> Update(Manager obj)
        {
            try
            {
                await conn.OpenAsync();
                command.CommandText = "Manager_Update";
                SetParameters(obj);
                await command.ExecuteNonQueryAsync();
                await conn.CloseAsync();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
           
        }
    }
}
