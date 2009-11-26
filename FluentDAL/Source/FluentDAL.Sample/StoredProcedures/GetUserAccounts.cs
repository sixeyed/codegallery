using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Rhino.Mocks;

namespace FluentDAL.Sample.StoredProcedures
{
    public class GetUserAccounts : IDisposable
    {
        public Guid UserId { get; set; }

        public IDataReader Execute()
        {
            return GetStubReader();
        }

        private static int ReadCount = 0;

        private static IDataReader GetStubReader()
        {
            IDataReader stubReader = MockRepository.GenerateStub<IDataReader>();
            stubReader.Stub(x => x.FieldCount).Return(3);
            stubReader.Stub(x => x.GetName(0)).Return("AccountID");
            stubReader.Stub(x => x.GetName(1)).Return("AccountName");
            stubReader.Stub(x => x.GetName(2)).Return("AccountActivated");
            stubReader.Stub(x => x.Read()).Return(true).WhenCalled(i => { 
                GetUserAccounts.ReadCount++; 
                i.ReturnValue = GetUserAccounts.ReadCount < 4; });
            stubReader.Stub(x => x.GetValue(0)).Return(GetId()).WhenCalled(i => i.ReturnValue = GetId());
            stubReader.Stub(x => x.GetValue(1)).Return(GetName()).WhenCalled(i => i.ReturnValue = GetName());
            stubReader.Stub(x => x.GetValue(2)).Return(GetActivated()).WhenCalled(i => i.ReturnValue = GetActivated()); ;
            return stubReader;
        }

        private static Guid GetId()
        {
            return Guid.NewGuid();
        }

        private static string GetName()
        {
            return string.Format("Account {0}", GetUserAccounts.ReadCount);
        }

        private static string GetActivated()
        {
            return GetUserAccounts.ReadCount == 3 ? "Y" : "N";
        }

        #region IDisposable Members

        public void Dispose()
        {
            //do nothing
        }

        #endregion
    }
}
