using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Language.Flow;

namespace SFA.DAS.Courses.Data.UnitTests.DatabaseMock
{
    public static class MoqExtensions
    {
        public static IReturnsResult<ICoursesDataContext> ReturnsDbSet<TEntity>(this ISetup<ICoursesDataContext, DbSet<TEntity>> setupResult, IEnumerable<TEntity> entities) where TEntity : class
        {
            var entitiesAsQueryable = entities.AsQueryable();
            var dbSetMock = new Mock<DbSet<TEntity>>();

            dbSetMock.As<IAsyncEnumerable<TEntity>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new InMemoryDbAsyncEnumerator<TEntity>(entitiesAsQueryable.GetEnumerator()));

            dbSetMock.As<IQueryable<TEntity>>()
                .Setup(m => m.Provider)
                .Returns(new InMemoryAsyncQueryProvider<TEntity>(entitiesAsQueryable.Provider));

            dbSetMock.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(entitiesAsQueryable.Expression);
            dbSetMock.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(entitiesAsQueryable.ElementType);
            dbSetMock.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesAsQueryable.GetEnumerator());

            return setupResult.Returns(dbSetMock.Object);
        }
    }
}
