using System;

namespace ObjectContainer.Objects
{
    public class Contact : ContactDomainObj
    {
        public string FirstName { get; set; }
        public string LastName{ get; set; }
        public string StateOrProvince{ get; set; }
        public string Region{ get; set; }
        public string City{ get; set; }
        public string Address { get; set; }
        public DateTime Birthdate{ get; set; }
        public string HomePhone{ get; set; }
        public string Phone{ get; set; }

    }
}
