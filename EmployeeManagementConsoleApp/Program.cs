using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

internal class Program
{
    private static void Main(string[] args)
    {
        // Build configuration
        var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();

        // Read connection string
        string connectionString = config.GetConnectionString("MyConnectionString");


        while (true)
        {
            Console.WriteLine("____________________________________________");
            Console.WriteLine("Choose an operation:");
            Console.WriteLine("1. View Employees");
            Console.WriteLine("2. Insert Employee");
            Console.WriteLine("3. Update Employee");
            Console.WriteLine("4. Delete Employee");
            Console.WriteLine("5. Exit");
            Console.WriteLine("____________________________________________");

            // Read choice input from user
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a valid operation number.");
                continue;
            }


            switch (choice)
            {
                case 1:
                    ViewEmployees(connectionString);
                    break;
                case 2:
                    InsertEmployee(connectionString);
                    break;
                case 3:
                    UpdateEmployee(connectionString);
                    break;
                case 4:
                    DeleteEmployee(connectionString);
                    break;
                case 5:
                    Console.WriteLine("Exiting the program.");
                    return;
                default:
                    Console.WriteLine("Invalid operation number. Please choose a valid operation.");
                    break;
            }
        }

        // display employee details
        void ViewEmployees(string connectionString)
        {
            string query = "SELECT [Id], [Name], [Designation], [Address] FROM [dbo].[Employees]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        Console.WriteLine("\n List of Employees:");
                        Console.WriteLine("----------------------------------------------------------------------");
                        Console.WriteLine("|    Id    |         Name         |   Designation   |     Address    |");
                        Console.WriteLine("----------------------------------------------------------------------");

                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("Id"));
                            string name = reader.GetString(reader.GetOrdinal("Name"));
                            string designation = reader.GetString(reader.GetOrdinal("Designation"));
                            string address = reader.GetString(reader.GetOrdinal("Address"));

                            // Use string formatting to align columns
                            Console.WriteLine($"| {id,-8} | {name,-20} | {designation,-15} | {address,-14} |");
                        }

                        Console.WriteLine("----------------------------------------------------------------------");
                    }
                    else
                    {
                        Console.WriteLine("No employees found.");
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
        // Add new employee details
        void InsertEmployee(string connectionString)
        {
            Console.WriteLine("____________________________________________");           
            Console.WriteLine("Enter Employee Name:");
            string name = Console.ReadLine().Trim();

            Console.WriteLine("Enter Employee Designation:");
            string designation = Console.ReadLine().Trim();

            Console.WriteLine("Enter Employee Address:");
            string address = Console.ReadLine().Trim();

            string query = "INSERT INTO [dbo].[Employees] ([Name], [Designation], [Address], [RecordCreatedOn]) VALUES (@Name, @Designation, @Address, GETDATE())";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Designation", designation);
                command.Parameters.AddWithValue("@Address", address);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) inserted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
        // Update employee details
        void UpdateEmployee(string connectionString)
        {
            Console.WriteLine("Enter Employee Id to update:");
            if (!int.TryParse(Console.ReadLine(), out int employeeId))
            {
                Console.WriteLine("Invalid input. Please enter a valid Employee Id.");
                return;
            }

            Console.WriteLine("Enter new Employee Name:");
            string name = Console.ReadLine().Trim();

            Console.WriteLine("Enter new Employee Designation:");
            string designation = Console.ReadLine().Trim();

            Console.WriteLine("Enter new Employee Address:");
            string address = Console.ReadLine().Trim();

            string query = "UPDATE [dbo].[Employees] SET [Name] = @Name, [Designation] = @Designation, [Address] = @Address WHERE [Id] = @EmployeeId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeId", employeeId);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Designation", designation);
                command.Parameters.AddWithValue("@Address", address);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) updated successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
        // Delete employee details
        void DeleteEmployee(string connectionString)
        {
            Console.WriteLine("Enter Employee Id to delete:");
            if (!int.TryParse(Console.ReadLine(), out int employeeId))
            {
                Console.WriteLine("Invalid input. Please enter a valid Employee Id.");
                return;
            }

            string query = "DELETE FROM [dbo].[Employees] WHERE [Id] = @EmployeeId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeId", employeeId);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}