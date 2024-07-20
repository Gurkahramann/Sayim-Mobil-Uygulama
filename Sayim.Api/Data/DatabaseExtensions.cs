using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Sayim.Api.Data
{
    public static class DatabaseExtensions
    {
        public static async Task<T> ExecuteScalarAsync<T>(this DatabaseFacade databaseFacade, string sql, CancellationToken cancellationToken = default)
        {
            using (var command = databaseFacade.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                await databaseFacade.OpenConnectionAsync(cancellationToken);
                var result = await command.ExecuteScalarAsync(cancellationToken);
                return (T)Convert.ChangeType(result, typeof(T));
            }
        }
    }
}
