// This using directive will allow the code to use classes and methods in the System namespace without specifying the full namespace path.
using System;
// This using directive will allow the code to use classes and methods in the System.Collections.Generic namespace for collection objects like List<T>.
using System.Collections.Generic;
// This using directive will allow the code to use classes and methods provided by the Microsoft.Data.Sqlite library to interact with SQLite databases.
using Microsoft.Data.Sqlite;

// Define a class named DatabaseManager to encapsulate database operations.
public class DatabaseManager
{
    // This statement will declare a constant string for the SQLite connection string to connect to the Northwind.db database.
    private const string connectionString = "Data Source=Northwind.db";

    // This method will retrieve a list of Customer objects from the database.
    public static List<Customer> GetCustomers()
    {
        // This statement will create a new list of Customer objects.
        var customers = new List<Customer>();
        // This using statement will ensure that the SQLite connection is properly closed and disposed after use.
        using (var connection = new SqliteConnection(connectionString))
        {
            // This statement will open the database connection.
            connection.Open();
            // This statement will create a new SQL command to be executed.
            var command = connection.CreateCommand();
            // This statement will set the SQL command text to select customer details from the Customers table.
            command.CommandText = "SELECT CustomerID, CompanyName, ContactName, ContactTitle, Address, City FROM Customers";
            // This using statement will ensure the SQL data reader is properly closed and disposed after use.
            using (var reader = command.ExecuteReader())
            {
                // This loop will read each row returned by the SQL query.
                while (reader.Read())
                {
                    // This statement will add a new Customer object to the list, initializing it with data read from the database.
                    customers.Add(new Customer
                    {
                        CustomerId = reader["CustomerID"]?.ToString() ?? string.Empty,
                        CompanyName = reader["CompanyName"]?.ToString() ?? string.Empty,
                        ContactName = reader["ContactName"]?.ToString() ?? string.Empty,
                        ContactTitle = reader["ContactTitle"]?.ToString() ?? string.Empty,
                        Address = reader["Address"]?.ToString() ?? string.Empty,
                        City = reader["City"]?.ToString() ?? string.Empty,
                    });
                }
            }
        }
        // This statement will return the list of Customer objects populated from the database.
        return customers;
    }

    // This method will insert a new Customer object into the database.
    internal static void CreateCustomer(Customer customer)
    {
        // This using statement will ensure that the SQLite connection is properly closed and disposed after use.
        using (var connection = new SqliteConnection(connectionString))
        {
            // This statement will open the database connection.
            connection.Open();
            
            // This using statement will start a database transaction to ensure data integrity during the insert operation.
            using (var transaction = connection.BeginTransaction())
            {
                // This statement will create a new SQL command within the context of the transaction.
                var command = connection.CreateCommand();
                command.Transaction = transaction;

                // This statement will set the SQL command text to insert a new customer record into the Customers table.
                command.CommandText = @"INSERT INTO Customers (CustomerID, CompanyName, ContactName, ContactTitle, Address, City) VALUES (@CustomerId, @CompanyName, @ContactName, @ContactTitle, @Address, @City)";

                // These statements will add parameters to the SQL command to prevent SQL injection and ensure data integrity.
                command.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
                command.Parameters.AddWithValue("@CompanyName", customer.CompanyName);
                command.Parameters.AddWithValue("@ContactName", customer.ContactName);
                command.Parameters.AddWithValue("@ContactTitle", customer.ContactTitle);
                command.Parameters.AddWithValue("@Address", customer.Address);
                command.Parameters.AddWithValue("@City", customer.City);

                try
                {
                    // This statement will execute the SQL command and insert the customer data into the database.
                    command.ExecuteNonQuery();
                    // This statement will commit the transaction if the insert operation is successful.
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // This block will handle any exceptions that occur during the insert operation, printing an error message and rolling back the transaction.
                    Console.WriteLine("An error occurred: " + ex.Message);
                    transaction.Rollback();
                }
            }
        }
    }
}
