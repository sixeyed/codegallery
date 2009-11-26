using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Rhino.Mocks;

namespace FluentDAL.Sample.StoredProcedures
{
    public class GetUser : IDisposable
    {
        public Guid UserId { get; set; }

        public IDataReader Execute()
        {
            string userName = Guid.NewGuid().ToString();
            Guid userId = Guid.NewGuid();
            string line1 = Guid.NewGuid().ToString();
            string postcodeInward = "PL21";
            string postcodeOutward = "9TY";

            return GetStubReader(userName, userId, line1, postcodeInward, postcodeOutward);
        }

        private static IDataReader GetStubReader(string userName, Guid userId, string line1, string postcodeInward, string postcodeOutward)
        {
            IDataReader stubReader = MockRepository.GenerateStub<IDataReader>();
            stubReader.Stub(x => x.FieldCount).Return(5);
            stubReader.Stub(x => x.GetName(0)).Return("UserId");
            stubReader.Stub(x => x.GetName(1)).Return("UserName");
            stubReader.Stub(x => x.GetName(2)).Return("AddressLine1");
            stubReader.Stub(x => x.GetName(3)).Return("PS_IN");
            stubReader.Stub(x => x.GetName(4)).Return("PS_OUT");
            stubReader.Stub(x => x.GetValue(0)).Return(userId);
            stubReader.Stub(x => x.GetValue(1)).Return(userName);
            stubReader.Stub(x => x.GetValue(2)).Return(line1);
            stubReader.Stub(x => x.GetValue(3)).Return(postcodeInward);
            stubReader.Stub(x => x.GetValue(4)).Return(postcodeOutward);
            return stubReader;
        }

        #region IDisposable Members

        public void Dispose()
        {
            //do nothing
        }

        #endregion
    }
}
