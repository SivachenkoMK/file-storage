using System;
using Microsoft.EntityFrameworkCore;

namespace Profile.Storage.Persistence.Tools
{
    public static class EfMappingExtensions
    {
        public static ModelBuilder RegisterEntityMapping<TEntity, TMapping>(this ModelBuilder builder)
            where TMapping : IEntityMappingConfiguration<TEntity>
            where TEntity : class
        {
            var mapper = (IEntityMappingConfiguration<TEntity>)Activator.CreateInstance(typeof(TMapping));
            mapper?.Map(builder.Entity<TEntity>());
            return builder;
        }
    }
}