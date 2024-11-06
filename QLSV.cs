using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

class QLSV
{
    private string connectionString = @"Data Source=ADMIN\BAHGIAHUY;Initial Catalog=QLSV;Integrated Security=True;TrustServerCertificate=True";
    private DBHelper DB;

    public QLSV()
    {
        DB = new DBHelper(connectionString);
    }

    public void Show()
    {
        string query = "SELECT * FROM SV";
        DataTable records = DB.GetRecords(query);
        foreach (DataRow row in records.Rows)
        {
            string MSSV = row["MSSV"].ToString();
            string NameSv = row["NameSV"].ToString();
            string DTB = row["DTB"].ToString();
            Console.WriteLine($"{MSSV} - {NameSv} - {DTB}");
        }
    }

    public DataTable Search(int idLop, string nameSV)
    {
        string query = "SELECT * FROM SV WHERE ID_lop = @idLop AND NameSV LIKE @nameSV";
        SqlParameter[] parameters =
        {
            new SqlParameter("@idLop", SqlDbType.Int) { Value = idLop },
            new SqlParameter("@nameSV", SqlDbType.NVarChar) { Value = $"%{nameSV}%" }
        };
        return DB.GetRecords(query, parameters);
    }

    public void Update(string maSV, string NameSV, float DTB)
    {
        string query = "UPDATE SV SET NameSV = @NameSV, DTB = @DTB WHERE MSSV = @maSV";
        SqlParameter[] parameters =
        {
            new SqlParameter("@NameSV", SqlDbType.NVarChar) { Value = NameSV },
            new SqlParameter("@DTB", SqlDbType.Float) { Value = DTB },
            new SqlParameter("@maSV", SqlDbType.NVarChar) { Value = maSV }
        };
        DB.ExecuteDB(query, parameters);
    }

    public void Delete(string maSV)
    {
        string query = "DELETE FROM SV WHERE MSSV = @maSV";
        SqlParameter parameter = new SqlParameter("@maSV", SqlDbType.NVarChar) { Value = maSV };
        DB.ExecuteDB(query, parameter);
    }

    public DataTable Sort()
    {
        string query = "SELECT * FROM SV ORDER BY NameSV ASC";
        return DB.GetRecords(query);
    }
}

internal class DBHelper
{
    private string connectionString;

    public DBHelper(string connectionString)
    {
        this.connectionString = connectionString;
    }

    internal void ExecuteDB(string query, params SqlParameter[] parameters)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error executing query: " + ex.Message);
                }
            }
        }
    }

    internal DataTable GetRecords(string query, params SqlParameter[] parameters)
    {
        DataTable dataTable = new DataTable();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error retrieving records: " + ex.Message);
                }
            }
        }
        return dataTable;
    }
}
