using Dapper;
using Npgsql;

namespace SharedLibrary.Dapper
{
    public static class DapperOperations
    {
        public static async Task ExecuteAsync(string sql, object model, string conn)
        {
            using (var connection = new NpgsqlConnection(conn))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(sql, model);
            }
        }

        public static async Task<T?> QueryScalarAsync<T>(string sql, object model, string conn)
        {
            using (var connection = new NpgsqlConnection(conn))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<T>(sql, model);
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(string sql, object model, string conn)
        {
            using (var connection = new NpgsqlConnection(conn))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<T>(sql, model);
            }
        }
    }
}
