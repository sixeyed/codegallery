using System;
using System.Collections.Generic;
using System.Text;
using FluentDAL.Sample.Entities;
using FluentDAL;
using FluentDAL.Mapping;

namespace FluentDAL.Sample.Maps
{
    public class AddressMap : DataReaderMap<Address>
    {
        public AddressMap()
        {
            Map(x => x.Line1, "AddressLine1");
            References<PostCode>(x => x.PostCode, new PostCodeMap());
        }
    }
}
