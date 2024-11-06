using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

class QLNS
{
    private string connectionString = @"Data Source=ADMIN\BAHGIAHUY;Initial Catalog=QLNS;Integrated Security=True;TrustServerCertificate=True";
    private DBHelper DB;

    public QLNS()
    {
        DB = new DBHelper(connectionString);
    }

    public void Show()
    {
        string query = "SELECT * FROM Employees";
        DataTable records = DB.GetRecords(query);
        foreach (DataRow row in records.Rows)
        {
            string employeeID = row["EmployeeID"].ToString();
            string firstName = row["FirstName"].ToString();
            string lastName = row["LastName"].ToString();
            string departmentID = row["DepartmentID"].ToString();
            string position = row["Position"].ToString();
            string salary = row["Salary"].ToString();
            string hireDate = row["HireDate"].ToString();
            Console.WriteLine($"{employeeID} - {firstName} {lastName} - {position} - {departmentID} - {salary} - {hireDate}");
        }
    }

    public DataTable Search(int departmentID, string lastName)
    {
        string query = "SELECT * FROM Employees WHERE DepartmentID = @departmentID AND LastName LIKE @lastName";
        SqlParameter[] parameters =
        {
            new SqlParameter("@departmentID", SqlDbType.Int) { Value = departmentID },
            new SqlParameter("@lastName", SqlDbType.NVarChar) { Value = $"%{lastName}%" }
        };
        return DB.GetRecords(query, parameters);
    }

    public void Update(string employeeID, string firstName, string lastName, int departmentID, string position, decimal salary, DateTime hireDate)
    {
        string query = "UPDATE Employees SET FirstName = @firstName, LastName = @lastName, DepartmentID = @departmentID, Position = @position, Salary = @salary, HireDate = @hireDate WHERE EmployeeID = @employeeID";
        SqlParameter[] parameters =
        {
            new SqlParameter("@firstName", SqlDbType.NVarChar) { Value = firstName },
            new SqlParameter("@lastName", SqlDbType.NVarChar) { Value = lastName },
            new SqlParameter("@departmentID", SqlDbType.Int) { Value = departmentID },
            new SqlParameter("@position", SqlDbType.NVarChar) { Value = position },
            new SqlParameter("@salary", SqlDbType.Decimal) { Value = salary },
            new SqlParameter("@hireDate", SqlDbType.Date) { Value = hireDate },
            new SqlParameter("@employeeID", SqlDbType.Int) { Value = employeeID }
        };
        DB.ExecuteDB(query, parameters);
    }

    public void Delete(string employeeID)
    {
        string query = "DELETE FROM Employees WHERE EmployeeID = @employeeID";
        SqlParameter parameter = new SqlParameter("@employeeID", SqlDbType.Int) { Value = employeeID };
        DB.ExecuteDB(query, parameter);
    }

    public DataTable Sort()
    {
        string query = "SELECT * FROM Employees ORDER BY LastName ASC";
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