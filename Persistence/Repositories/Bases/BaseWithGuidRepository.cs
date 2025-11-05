using System.Data;
using System.Data.Common;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Commons.Enums;
using Domain.Entities.Bases;
using Application.Infrastructure.Persistence.Bases;

namespace Persistence.Repositories.Bases
{
    public class BaseWithGuidRepository<T> :
        IBaseWithGuidRepository<T> where T : BaseEntityWithGuid
    {
        private readonly PostgresDbContext _sqlServerDbContext;

        public BaseWithGuidRepository(PostgresDbContext sqlServerDbContext)
        {
            _sqlServerDbContext = sqlServerDbContext;
        }

        public async Task<DbConnection> EnsureConnectionOpenAsync(DbContext dbContext)
        {
            var connection = dbContext.Database.GetDbConnection();
            if (dbContext.Database.GetDbConnection().State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            return connection;
        }

        public string ConvertOperatorToSQL(FilterOperator filterOperator)
        {
            if (filterOperator == FilterOperator.Contains || filterOperator == FilterOperator.StartsWith || filterOperator == FilterOperator.EndsWith)
            {
                return "LIKE";
            }

            return filterOperator.GetEnumDescription();
        }

        public string ConvertValueToSQL(FilterOperator filterOperator, object? filterOperand)
        {
            if (filterOperand == null)
            {
                return "NULL";
            }

            if (filterOperand is JsonElement json)
            {
                if (json.ValueKind == JsonValueKind.String)
                {
                    var str = json.GetString();

                    // For datetime
                    if (DateTime.TryParse(str, out DateTime dt))
                    {
                        return $"'{dt:yyyy-MM-dd HH:mm:ss.fffffff}'";
                    }

                    // For normal strings
                    if (filterOperator == FilterOperator.Contains)
                    {
                        return $"'%{str?.Replace("'", "''")}%'";
                    }
                    else if (filterOperator == FilterOperator.StartsWith)
                    {
                        return $"'{str?.Replace("'", "''")}%'";
                    }
                    else if (filterOperator == FilterOperator.EndsWith)
                    {
                        return $"'%{str?.Replace("'", "''")}'";
                    }

                    return $"'{str?.Replace("'", "''")}'";
                }

                // For number
                if (json.ValueKind == JsonValueKind.Number)
                {
                    return json.GetRawText();
                }

                // For booleans
                if (json.ValueKind == JsonValueKind.True || json.ValueKind == JsonValueKind.False)
                {
                    return json.GetBoolean() ? "1" : "0";
                }

                // For arrays
                if (json.ValueKind == JsonValueKind.Array)
                {
                    var items = json.EnumerateArray().Select(element => ConvertValueToSQL(FilterOperator.Equals, element));

                    return $"({string.Join(", ", items)})";
                }
            }

            return filterOperand.ToString() ?? "NULL";
        }

        public virtual async Task<T?> CreateAsync(T entity)
        {
            if (entity is BaseEntityWithGuid tracked)
            {
                tracked.CreatedAt = DateTime.UtcNow;
            }
            var entry = await _sqlServerDbContext.Set<T>().AddAsync(entity);
            await _sqlServerDbContext.SaveChangesAsync();

            return entry.Entity;
        }

        public virtual async Task<int> DeleteAsync(IEnumerable<Guid> ids)
        {
            var entities = await _sqlServerDbContext.Set<T>().Where(e => ids.Contains(e.Id.Value)).ToListAsync();
            _sqlServerDbContext.Set<T>().RemoveRange(entities);
            var affectedRows = await _sqlServerDbContext.SaveChangesAsync();

            return affectedRows;
        }

        public virtual async Task<T?> UpdateAsync(T existingEntity)
        {
            existingEntity.ModifiedAt = DateTime.UtcNow;
            _sqlServerDbContext.Set<T>().Update(existingEntity);
            await _sqlServerDbContext.SaveChangesAsync();

            return existingEntity;
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            var entity = await _sqlServerDbContext.Set<T>().SingleOrDefaultAsync(t => t.Id == id);

            return entity;
        }
    }
}
