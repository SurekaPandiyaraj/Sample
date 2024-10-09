using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalManagementSystem
{
    public class CarRepository
    {
        private readonly string connectionString = "Server=(localdb)\\MSSQLLocalDB;database=master;"; // Use master to create the new database

        public void InitializeDatabase()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // SQL command to create the database
                string createDatabaseSql = "IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'CarRentalManagement') " +
                                            "CREATE DATABASE CarRentalManagement;";
                using (SqlCommand command = new SqlCommand(createDatabaseSql, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Database 'CarRentalManagement' created successfully or already exists.");
                }

                // Change the connection string to point to the new database
                connection.ChangeDatabase("CarRentalManagement");

                // SQL command to create the Cars table
                string createTableSql = @"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Cars')
                CREATE TABLE Cars (
                    CarId INT PRIMARY KEY IDENTITY(1,1),
                    Brand NVARCHAR(100),
                    Model NVARCHAR(100),
                    RentalPrice DECIMAL(18, 2)
                );";
                using (SqlCommand command = new SqlCommand(createTableSql, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Table 'Cars' created successfully or already exists.");
                }
            }
        }

        public decimal ValidateCarRentalPrice()
        {
            decimal rentalPrice;
            do
            {
                Console.Write("Enter rental price: ");
                while (!decimal.TryParse(Console.ReadLine(), out rentalPrice) || rentalPrice <= 0)
                {
                    Console.WriteLine("Invalid price. Please enter a positive value.");
                }
            } while (rentalPrice <= 0);

            return rentalPrice;
        }

        public void CreateCar(Car car)
        {
            try
            {
                car.Brand = CapitalizeBrand(car.Brand);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.ChangeDatabase("CarRentalManagement"); // Ensure you're in the correct database
                    string query = "INSERT INTO Cars (Brand, Model, RentalPrice) VALUES (@Brand, @Model, @RentalPrice)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Brand", car.Brand);
                        command.Parameters.AddWithValue("@Model", car.Model);
                        command.Parameters.AddWithValue("@RentalPrice", car.RentalPrice);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while creating car: {ex.Message}");
            }
        }

        public List<Car> GetAllCars()
        {
            List<Car> cars = new List<Car>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.ChangeDatabase("CarRentalManagement"); // Ensure you're in the correct database
                    string query = "SELECT * FROM Cars";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Car car = new Car(
                                (int)reader["CarId"],
                                (string)reader["Brand"],
                                (string)reader["Model"],
                                (decimal)reader["RentalPrice"]
                            );
                            cars.Add(car);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrieving cars: {ex.Message}");
            }

            Console.WriteLine($"Retrieved {cars.Count} cars from the database.");
            return cars;
        }

        public Car GetCarById(int carId)
        {
            Car car = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.ChangeDatabase("CarRentalManagement"); // Ensure you're in the correct database
                    string query = "SELECT * FROM Cars WHERE CarId = @CarId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CarId", carId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                car = new Car(
                                    (int)reader["CarId"],
                                    (string)reader["Brand"],
                                    (string)reader["Model"],
                                    (decimal)reader["RentalPrice"]
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrieving car with ID {carId}: {ex.Message}");
            }
            return car;
        }

        public void UpdateCar(Car car)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.ChangeDatabase("CarRentalManagement"); // Ensure you're in the correct database
                    string query = "UPDATE Cars SET Brand = @Brand, Model = @Model, RentalPrice = @RentalPrice WHERE CarId = @CarId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CarId", car.CarId);
                        command.Parameters.AddWithValue("@Brand", CapitalizeBrand(car.Brand));
                        command.Parameters.AddWithValue("@Model", car.Model);
                        command.Parameters.AddWithValue("@RentalPrice", car.RentalPrice);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while updating car: {ex.Message}");
            }
        }

        public void DeleteCar(int carId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.ChangeDatabase("CarRentalManagement"); // Ensure you're in the correct database
                    string query = "DELETE FROM Cars WHERE CarId = @CarId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CarId", carId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while deleting car: {ex.Message}");
            }
        }

        public string CapitalizeBrand(string brand)
        {
            if (string.IsNullOrEmpty(brand)) return brand;
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(brand.ToLower());
        }
    }
}