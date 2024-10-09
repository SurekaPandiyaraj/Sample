using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalManagementSystem
{
    public class CarManager
    {
        private List<Car> cars = new List<Car>();
        private int nextId = 1;

        public void CreateCar()
        {
            Console.Write("Enter car brand: ");
            string brand = Console.ReadLine();

            Console.Write("Enter car model: ");
            string model = Console.ReadLine();

            decimal rentalPrice = ValidateCarRentalPrice();

            Car car = new Car(nextId++, brand, model, rentalPrice);
            cars.Add(car);
            Console.WriteLine("Car added successfully!");
        }

        public void ReadCars()
        {
            if (cars.Count == 0)
            {
                Console.WriteLine("No cars available.");
                return;
            }

            foreach (var car in cars)
            {
                Console.WriteLine(car);
            }
        }

        public void UpdateCar()
        {
            Console.Write("Enter Car ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int carId))
            {
                var car = cars.Find(c => c.CarId == carId);
                if (car != null)
                {
                    Console.Write("Enter new brand (leave empty to keep current): ");
                    string brand = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(brand)) car.Brand = brand;

                    Console.Write("Enter new model (leave empty to keep current): ");
                    string model = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(model)) car.Model = model;

                    decimal rentalPrice = ValidateCarRentalPrice();
                    car.RentalPrice = rentalPrice;

                    Console.WriteLine("Car updated successfully!");
                }
                else
                {
                    Console.WriteLine("Car not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Car ID.");
            }
        }

        public void DeleteCar()
        {
            Console.Write("Enter Car ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int carId))
            {
                var car = cars.Find(c => c.CarId == carId);
                if (car != null)
                {
                    cars.Remove(car);
                    Console.WriteLine("Car deleted successfully!");
                }
                else
                {
                    Console.WriteLine("Car not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Car ID.");
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
    }
}
