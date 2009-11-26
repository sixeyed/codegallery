using System;
using System.Collections.Generic;
using System.Text;
using FluentDAL.Sample.Entities;
using FluentDAL;
using FluentDAL.Sample.Maps.Conversion;
using FluentDAL.Mapping;

namespace FluentDAL.Sample.Maps
{
    public class AccountMap : DataReaderMap<Account>
    {
        public AccountMap()
        {
            Map(x => x.Id, "AccountID");
            Map(x => x.Name, "AccountName");
            Map<string, bool>(x => x.Activated, "AccountActivated", Legacy.FromBoolean);
        }
    }
}
