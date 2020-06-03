using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Courses.Data.UnitTests.DatabaseMock
{
    public class InMemoryDbAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _innerEnumerator;
        private bool _disposed;

        public InMemoryDbAsyncEnumerator(IEnumerator<T> enumerator)
        {
            _innerEnumerator = enumerator;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            return Task.FromResult(_innerEnumerator.MoveNext());
        }

        public T Current => _innerEnumerator.Current;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                    _innerEnumerator.Dispose();
                }

                _disposed = true;
            }
        }

    }
}