using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicSP.Sample.Entities
{
    public class Employee
    {
        public int Id {get; set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ManagerId { get; set; }
        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }
    }
}
