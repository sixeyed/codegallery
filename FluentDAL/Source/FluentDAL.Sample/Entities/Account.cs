using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentDAL.Sample.Entities
{
    public class Account
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool Activated { get; set; }
    }
}
