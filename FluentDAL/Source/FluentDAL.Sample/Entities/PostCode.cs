using System;
using System.Collections.Generic;
using System.Text;

namespace FluentDAL.Sample.Entities
{
    public class PostCode
    {
        public string InwardCode { get; set; }

        public string OutwardCode { get; set; }

        public string Value
        {
            get { return string.Format("{0} {1}", InwardCode, OutwardCode); }
        }
    }
}
