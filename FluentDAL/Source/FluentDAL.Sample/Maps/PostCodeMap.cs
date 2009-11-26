using System;
using System.Collections.Generic;
using System.Text;
using FluentDAL.Sample.Entities;
using FluentDAL;
using FluentDAL.Mapping;

namespace FluentDAL.Sample.Maps
{
    public class PostCodeMap : DataReaderMap<PostCode>
    {
        public PostCodeMap()
        {
            Map(x => x.InwardCode, "PS_IN");
            Map(x => x.OutwardCode, "PS_OUT");
        }
    }
}
