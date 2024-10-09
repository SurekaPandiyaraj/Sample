using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalManagementSystem
{
    public class Car
    {
        public static int TotalCars { get; private set; } = 0;

        public int CarId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal RentalPrice { get; set; }

        public Car(int carId, string brand, string model, decimal rentalPrice)
        {
            CarId = carId;
            Brand = brand;
            Model = model;
            RentalPrice = rentalPrice;
            TotalCars++;
        }

        public Car() // Default constructor for creating instances without parameters
        {
            TotalCars++;
        }

        public virtual string DisplayCarInfo()
        {
            return $"ID: {CarId}, Brand: {Brand}, Model: {Model}, RentalPrice: {RentalPrice:C}";
        }

        public override string ToString()
        {
            return DisplayCarInfo(); // Use the virtual method for the ToString output
        }
    }


    public class ElectricCar : Car
    {
        public int BatteryCapacity { get; set; }
        public int MotorPower { get; set; }

        public ElectricCar(int carId, string brand, string model, decimal rentalPrice, int batteryCapacity, int motorPower)
            : base(carId, brand, model, rentalPrice)
        {
            BatteryCapacity = batteryCapacity;
            MotorPower = motorPower;
        }

        public override string DisplayCarInfo()
        {
            return $"{base.DisplayCarInfo()}, BatteryCapacity: {BatteryCapacity} kWh, MotorPower: {MotorPower} HP";
        }
    }

    public class PetrolCar : Car
    {
        public int FuelTankCapacity { get; set; }
        public int EngineCapacity { get; set; }

        public PetrolCar(int carId, string brand, string model, decimal rentalPrice, int fuelTankCapacity, int engineCapacity)
            : base(carId, brand, model, rentalPrice)
        {
            FuelTankCapacity = fuelTankCapacity;
            EngineCapacity = engineCapacity;
        }

        public override string DisplayCarInfo()
        {
            return $"{base.DisplayCarInfo()}, FuelTankCapacity: {FuelTankCapacity} L, EngineCapacity: {EngineCapacity} cc";
        }
    }

}
