using Microsoft.EntityFrameworkCore;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Microsoft.EntityFrameworkCore.EntityState;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vrossi.ScaffoldCore.Infrastructure.Multitenancy;

namespace Vrossi.ScaffoldCore.Infrastructure.EntityFrameworkCore
{
    internal static class EntityFrameworkExtensions
    {
        public static void AddQueryFilterToAllEntitiesAssignableFrom<T>(this ModelBuilder modelBuilder, Expression<Func<T, bool>> expression)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (!typeof(T).IsAssignableFrom(entityType.ClrType))
                    continue;

                var parameterType = Expression.Parameter(entityType.ClrType);
                var expressionFilter = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), parameterType, expression.Body);

                var currentQueryFilter = entityType.GetQueryFilter();

                if (currentQueryFilter != null)
                {
                    var currentExpressionFilter = ReplacingExpressionVisitor.Replace(currentQueryFilter.Parameters.Single(), parameterType, currentQueryFilter.Body);
                    expressionFilter = Expression.AndAlso(currentExpressionFilter, expressionFilter);
                }

                var lambdaExpression = Expression.Lambda(expressionFilter, parameterType);
                entityType.SetQueryFilter(lambdaExpression);
            }
        }
        public static void MarkIfTenantNeeded(this DbContext context, string columnName, Func<Guid?> tenantIdFunc)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentNullException(nameof(columnName));

            if (tenantIdFunc == null)
                throw new ArgumentNullException(nameof(tenantIdFunc));

            var tenantId = tenantIdFunc.Invoke();
            if (tenantId == null) return;

            var entriesAdded = context.ChangeTracker.Entries().Where(e => e.State == Added);

            foreach (var addedEntry in entriesAdded)
            {
                if (addedEntry.Entity is ITenantEntity entity)
                    context.Entry(entity).Property<Guid>(columnName).CurrentValue = tenantId.Value;
            }
        }
        public static void ApplyTenantProperty<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, string columnName)
            where TEntity : Entity
        {
            if (entityTypeBuilder == null)
                throw new ArgumentNullException(nameof(entityTypeBuilder));

            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentNullException(nameof(columnName));

            if (!typeof(ITenantEntity).IsAssignableFrom(typeof(TEntity)))
                return;

            entityTypeBuilder.Property<Guid>(columnName).IsRequired();
        }
        public static void ChangeModifiedDateOnModifiedState(this DbContext context, string columnName)
        {
            var modifiedEntities = context.ChangeTracker.Entries().Where(x => x.State == Modified).OfType<Entity>();

            if (!modifiedEntities.Any())
                return;

            var currentDate = DateTime.Now;

            foreach (var aggregateRoot in modifiedEntities)
                context.Entry(aggregateRoot).Property<DateTime>(columnName).CurrentValue = currentDate;
        }
    }
}
