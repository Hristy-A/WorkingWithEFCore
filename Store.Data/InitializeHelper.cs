using Microsoft.EntityFrameworkCore;
using Store.Data.Entities;
using System;

namespace Store.Data.Extensions
{
    internal static class InitializeHelper
    {
        public static void InitilizeEnumTable<TEnumType>(ModelBuilder modelBuilder) where TEnumType : struct, Enum
        {
            foreach (EventType eventType in Enum.GetValues<EventType>())
            {
                modelBuilder.Entity<EventTypeInfo>().HasData(new EventTypeInfo(eventType));
            }
        }
    }
}
