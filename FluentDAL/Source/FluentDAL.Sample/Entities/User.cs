using System;
using System.Collections.Generic;
using System.Text;

namespace FluentDAL.Sample.Entities
{
    /// <summary>
    /// Represents a system user
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets/sets the entity id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets/sets the user's name
        /// </summary>
        public string Name { get; set;}

        public Address Address { get; set; }

        public List<Account> Accounts {get; set;}
    }
}
