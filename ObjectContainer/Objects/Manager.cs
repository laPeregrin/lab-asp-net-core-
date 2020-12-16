using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectContainer.Objects
{
    public class Manager : ManagerDomainObject
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Addres { get; set; }
        public string Position { get; set; }
        public string Remark { get; set; }
    }
}
