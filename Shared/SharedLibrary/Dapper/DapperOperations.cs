using Dapper;
using Npgsql;

namespace SharedLibrary.Dapper
{
    public static class DapperOperations
    {
        public static string connString = "Host=localhost;Port=5433;Database=users_db;Username=postgres;Password=postgres";
        public static async Task ExecuteAsync(string sql, object model)
        {
            using (var connection = new NpgsqlConnection(connString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(sql, model);
            }
        }

        public static async Task<T?> QueryScalarAsync<T>(string sql, object model)
        {
            using (var connection = new NpgsqlConnection(connString))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<T>(sql, model);
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(string sql, object model)
        {
            using (var connection = new NpgsqlConnection(connString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<T>(sql, model);
            }
        }
    }
}
