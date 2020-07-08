using System;
using System.Collections.Generic;
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

        public  ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_innerEnumerator.MoveNext());
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

        public ValueTask DisposeAsync()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            return new ValueTask();
        }
    }
}
