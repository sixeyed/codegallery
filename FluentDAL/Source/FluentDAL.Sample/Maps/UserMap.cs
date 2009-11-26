using System;
using System.Collections.Generic;
using System.Text;
using FluentDAL.Sample.Entities;
using FluentDAL;
using FluentDAL.Mapping;

namespace FluentDAL.Sample.Maps
{
    /// <summary>
    /// Maps a <see cref="User"/> instance from a populated <see cref="IDataReader"/>
    /// </summary>
    public class UserMap : DataReaderMap<User>
    {
        /// <summary>
        /// Default constructor, sets up mappings
        /// </summary>
        public UserMap()
        {
            Map(x => x.Name, "UserName");
            Map(x => x.Id, "UserId");
            References(x => x.Address, new AddressMap());
        }
    }

}
