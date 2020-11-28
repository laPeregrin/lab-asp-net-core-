using ObjectContainer.Objects;
using BO_BusinessObjects_.ORMs.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BO_BusinessObjects_.ORMs
{
    public class ContactDAO : BasedDAO, IContactDAO
    {
        public Contact Add(Contact newObj)
        {
            try
            {
                command.CommandText = "Contact_Add";
                SetParameters(newObj);
                command.ExecuteNonQuery();
                conn.Close();
                return newObj;
            }
            catch (Exception e)
            {
                var ex = e.Message;
                return null;
            }


        }
        public int Update(Contact newObj)
        {
            try
            {
                conn.Open();
                command.CommandText = "Contact_Update";
                SetParameters(newObj);
                var insertedRows = command.ExecuteNonQuery();
                conn.Close();
                return insertedRows;
            }
            catch(Exception e)
            {
                return -1;
            }
            
        }

        public bool Delete(Guid id)
        {
            try
            {
                command.CommandText = "Contact_Delete";
                SqlParameter objParam = command.Parameters.Add("@id", SqlDbType.UniqueIdentifier);
                objParam.Value = id;
                int result = command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public Contact Get(Guid id)
        {
            try
            {
                Contact resContact = null;
                command.CommandText = "Contact_Get";
                SqlParameter objParam = command.Parameters.Add("id", SqlDbType.UniqueIdentifier);
                objParam.Value = id;
                SqlDataReader objReader = command.ExecuteReader();
                if (objReader.Read())
                {
                    resContact = getItem(objReader);
                    conn.Close();
                    return resContact;

                }
                conn.Close();
                return resContact;

            }
            catch (Exception e)
            {
                var message = e.Message.ToString();
                var message2 = e.ToString();
                return null;
            }

        }

        public ICollection<Contact> GetAll()
        {
            var list = new List<Contact>();
            command.CommandText = "Contact_GetAll";
            var objReader = command.ExecuteReader();
            while (objReader.Read())
            {
                list.Add(getItem(objReader));
            }
            conn.Close();
            return list;
        }

        public Contact getItem(SqlDataReader objReader)
        {
            Contact contact = new Contact();
            object ob = null;
            contact.id = new Guid(objReader.GetValue(0).ToString());
            if ((ob = objReader.GetValue(1)) != DBNull.Value)
                contact.FirstName = Convert.ToString(ob);
            if ((ob = objReader.GetValue(2)) != DBNull.Value)
                contact.LastName = Convert.ToString(ob);
            if ((ob = objReader.GetValue(3)) != DBNull.Value)
                contact.StateOrProvince = Convert.ToString(ob);
            if ((ob = objReader.GetValue(4)) != DBNull.Value)
                contact.Region = Convert.ToString(ob);
            if ((ob = objReader.GetValue(5)) != DBNull.Value)
                contact.City = Convert.ToString(ob);
            if ((ob = objReader.GetValue(6)) != DBNull.Value)
                contact.Address = Convert.ToString(ob);
            if ((ob = objReader.GetValue(7)) != DBNull.Value)
                contact.Email = Convert.ToString(ob);
            if ((ob = objReader.GetValue(8)) != DBNull.Value)
                contact.Birthdate = Convert.ToDateTime(ob);
            if ((ob = objReader.GetValue(9)) != DBNull.Value)
                contact.HomePhone = Convert.ToString(ob);
            if ((ob = objReader.GetValue(10)) != DBNull.Value)
                contact.Phone = Convert.ToString(ob);
            if ((ob = objReader.GetValue(11)) != DBNull.Value)
                contact.Visible = Convert.ToBoolean(ob);
            return contact;
        }

        public ICollection<Contact> getItems()
        {
            var objReader = command.ExecuteReader();
            var list = new List<Contact>();
            while (objReader.Read())
            {
                list.Add(getItem(objReader));
            }
            conn.Close();
            return list;
        }

        public ICollection<Contact> getVisibleItems()
        {
            var list = new List<Contact>();
            command.CommandText = "Contact_GetAll";
            var objReader = command.ExecuteReader();
            while (objReader.Read())
            {
                var p = getItem(objReader);
                if (p.Visible == true)
                    list.Add(p);

            }
            conn.Close();
            return list;
        }



        public void SetParameters(Contact ob)
        {
            command.Parameters.Add("@iD", SqlDbType.UniqueIdentifier).Value = ob.id;
            command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = ob.FirstName;
            command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = ob.LastName;
            command.Parameters.Add("@StateOrProvince", SqlDbType.NVarChar).Value = ob.StateOrProvince;
            command.Parameters.Add("@Region", SqlDbType.NVarChar).Value = ob.Region;
            command.Parameters.Add("@City", SqlDbType.NVarChar).Value = ob.City;
            command.Parameters.Add("@Address", SqlDbType.NVarChar).Value = ob.Address;
            command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = ob.Email;
            command.Parameters.Add("@Birthdate", SqlDbType.DateTime).Value = ob.Birthdate;
            command.Parameters.Add("@HomePhone", SqlDbType.NVarChar).Value = ob.HomePhone;
            command.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = ob.Phone;
            command.Parameters.Add("@Visible", SqlDbType.Bit).Value = ob.Visible;//12
            command.Parameters.Add("RETURN VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue; //13

        }

    }
}










//SqlParameter objParam1 = command.Parameters.Add("@iD", SqlDbType.UniqueIdentifier);
//objParam1.Value = ob.id;
//SqlParameter objParam2 = command.Parameters.Add("@FirstName", SqlDbType.NVarChar);
//objParam2.Value = ob.FirstName;
//SqlParameter objParam3 = command.Parameters.Add("@LastName", SqlDbType.NVarChar);
//objParam3.Value = ob.LastName;
//SqlParameter objParam4 = command.Parameters.Add("@StateOrProvince", SqlDbType.NVarChar);
//objParam4.Value = ob.StateOrProvince;
//SqlParameter objParam5 = command.Parameters.Add("@Region", SqlDbType.NVarChar);
//objParam5.Value = ob.Region;
//SqlParameter objParam6 = command.Parameters.Add("@City", SqlDbType.NVarChar);
//objParam6.Value = ob.City;
//SqlParameter objParam7 = command.Parameters.Add("@Address", SqlDbType.NVarChar);
//objParam7.Value = ob.Address;
//SqlParameter objParam8 = command.Parameters.Add("@Email", SqlDbType.NVarChar);
//objParam8.Value = ob.Email;
//SqlParameter objParam9 = command.Parameters.Add("@Birthdate", SqlDbType.DateTime);
//objParam9.Value = ob.Birthdate;
//SqlParameter objParam10 = command.Parameters.Add("@HomePhone", SqlDbType.NVarChar);
//objParam10.Value = ob.HomePhone;
//SqlParameter objParam11 = command.Parameters.Add("@Phone", SqlDbType.NVarChar);
//objParam11.Value = ob.Phone;
//SqlParameter objParam12 = command.Parameters.Add("@Visible", SqlDbType.Bit);
//objParam12.Value = ob.Visible;
//SqlParameter objParam13 = command.Parameters.Add("RETURN VALUE", SqlDbType.Int);
//objParam13.Direction = ParameterDirection.ReturnValue;