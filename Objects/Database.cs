using System.Data;
using System.Data.SqlClient;

namespace Airline
{
  public class DB
  {
    private static SqlConnection _conn;

    public static SqlConnection GetConnection()
    {

      return _conn;
    }
    public static void CreateConnection()
    {
      _conn = new SqlConnection(DBConfiguration.ConnectionString);
    }

    public static void OpenConnection()
    {
      _conn.Open();
    }

    public static void CloseConnection()
    {
      if(_conn != null)
      {
        _conn.Close();
      }
    }
  }
}
