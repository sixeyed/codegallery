using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Domain.Ef.Tests
{
    public class NoLockScope : IDisposable
    {
        private TransactionScope _scope;

        public NoLockScope()
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };
            _scope = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions);
        }

        public void Dispose()
        {
            if (_scope != null)
            {
                _scope.Complete();
                _scope.Dispose();
                _scope = null;
            }
        }
    }
}
