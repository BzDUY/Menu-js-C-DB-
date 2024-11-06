using System;
using System.Data;

public class Program
{
    public static void Menu()
    {
        Console.WriteLine("------------------Menu------------------");
        Console.WriteLine("1. Hiển thị");
        Console.WriteLine("2. Tìm kiếm theo phòng ban và họ");
        Console.WriteLine("3. Cập nhật");
        Console.WriteLine("4. Xóa");
        Console.WriteLine("5. Sắp xếp");
        Console.WriteLine("6. Thoát");
    }

    public static void Main(string[] args)
    {
        QLNS q = new QLNS();
        while (true)
        {
            Console.Clear();
            Menu();
            Console.Write("Nhập lựa chọn của bạn: ");
            if (!int.TryParse(Console.ReadLine(), out int option))
            {
                Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng nhập lại.");
                Console.ReadLine();
                continue;
            }

            switch (option)
            {
                case 1:
                    q.Show();
                    break;
                case 2:
                    Console.Write("Nhập ID phòng ban: ");
                    int departmentID = int.Parse(Console.ReadLine());
                    Console.Write("Nhập họ nhân viên: ");
                    string lastName = Console.ReadLine();
                    DataTable searchResult = q.Search(departmentID, lastName);
                    foreach (DataRow row in searchResult.Rows)
                    {
                        Console.WriteLine($"{row["EmployeeID"]} - {row["FirstName"]} {row["LastName"]}");
                    }
                    break;
                case 3:
                    Console.Write("Nhập mã nhân viên: ");
                    string employeeID = Console.ReadLine();
                    Console.Write("Nhập tên nhân viên: ");
                    string firstName = Console.ReadLine();
                    Console.Write("Nhập họ nhân viên: ");
                    string lastName = Console.ReadLine();
                    Console.Write("Nhập ID phòng ban: ");
                    int departmentID = int.Parse(Console.ReadLine());
                    Console.Write("Nhập vị trí: ");
                    string position = Console.ReadLine();
                    Console.Write("Nhập lương: ");
                    decimal salary = decimal.Parse(Console.ReadLine());
                    Console.Write("Nhập ngày tuyển dụng: ");
                    DateTime hireDate = DateTime.Parse(Console.ReadLine());
                    q.Update(employeeID, firstName, lastName, departmentID, position, salary, hireDate);
                    q.Show();
                    break;
                case 4:
                    Console.Write("Nhập mã nhân viên cần xóa: ");
                    string empID = Console.ReadLine();
                    q.Delete(empID);
                    q.Show();
                    break;
                case 5:
                    DataTable sortResult = q.Sort();
                    foreach (DataRow row in sortResult.Rows)
                    {
                        Console.WriteLine($"{row["EmployeeID"]} - {row["FirstName"]} {row["LastName"]} - {row["Position"]} - {row["DepartmentID"]} - {row["Salary"]} - {row["HireDate"]}");
                    }
                    break;
                case 6:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng nhập lại.");
                    break;
            }
            Console.ReadLine();
        }
    }
}