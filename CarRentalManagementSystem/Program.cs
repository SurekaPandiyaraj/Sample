using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalManagementSystem
{
     class Program
    {
        static void Main(string[] args)
        {
            CarRepository carRepository = new CarRepository();
            carRepository.InitializeDatabase();
            int choice;

            do
            {
                Console.Clear();
                Console.WriteLine("Car Rental Management System:");
                Console.WriteLine("1. Add a Car");
                Console.WriteLine("2. View All Cars");
                Console.WriteLine("3. Update a Car");
                Console.WriteLine("4. Delete a Car");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");

                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 5)
                {
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 5.");
                }

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Car newCar = new Car();
                        Console.Write("Enter car brand: ");
                        newCar.Brand = Console.ReadLine();
                        Console.Write("Enter car model: ");
                        newCar.Model = Console.ReadLine();
                        newCar.RentalPrice = carRepository.ValidateCarRentalPrice();
                        carRepository.CreateCar(newCar);
                        break;
                    case 2:
                        Console.Clear();
                        var cars = carRepository.GetAllCars();
                        foreach (var car in cars)
                        {
                            Console.WriteLine(car);
                        }
                        break;
                    case 3:
                        Console.Clear();
                        Console.Write("Enter Car ID to update: ");
                        int updateId = int.Parse(Console.ReadLine());
                        Car carToUpdate = carRepository.GetCarById(updateId);
                        if (carToUpdate != null)
                        {
                            Console.Write("Enter new brand (leave empty to keep current): ");
                            string brand = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(brand)) carToUpdate.Brand = brand;

                            Console.Write("Enter new model (leave empty to keep current): ");
                            string model = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(model)) carToUpdate.Model = model;

                            carToUpdate.RentalPrice = carRepository.ValidateCarRentalPrice();
                            carRepository.UpdateCar(carToUpdate);
                        }
                        else
                        {
                            Console.WriteLine("Car not found.");
                        }
                        break;
                    case 4:
                        Console.Clear();
                        Console.Write("Enter Car ID to delete: ");
                        int deleteId = int.Parse(Console.ReadLine());
                        carRepository.DeleteCar(deleteId);
                        break;
                    case 5:
                        Console.Clear();
                        Console.WriteLine("Exiting the program. Goodbye!");
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();

            } while (choice != 5);





        }
    }
}
