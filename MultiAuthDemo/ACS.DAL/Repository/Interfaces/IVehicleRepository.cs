using ASC.Entities;
using System.Collections.Generic;

namespace ACS.DAL.Repository.Interfaces
{
    public interface IVehicleRepository
    {
        IEnumerable<Vehicle> GetVehicles();
        IEnumerable<Vehicle> GetVehiclesOfUser(string userId);
        Vehicle GetVehicle(int id);
        string CreateVehicle(Vehicle vehicle);
        string EditVehicle(Vehicle vehicle);
        string DeleteVehicle(int id);
    }
}
