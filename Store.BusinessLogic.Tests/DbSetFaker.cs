using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Store.BusinessLogic.Tests
{
    internal static class DbSetFaker
    {
        public static Mock<DbSet<T>> MockDbSet<T>(IEnumerable<T> data) where T : class
        {
            var dataList = new List<T>(data).AsQueryable();

            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(dataList.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(dataList.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(dataList.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(dataList.GetEnumerator());

            return mockSet;
        }
    }
}
