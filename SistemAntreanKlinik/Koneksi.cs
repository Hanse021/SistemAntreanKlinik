using System.Data.SqlClient;

namespace SistemAntreanKlinik
{
    internal class Koneksi
    {
        private static readonly string connectionString =
            @"Data Source=.\SQLEXPRESS;
              Initial Catalog=DB_AntreanKlinik;
              Integrated Security=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}