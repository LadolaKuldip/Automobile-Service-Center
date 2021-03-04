using ACS.DAL.Repository.Interfaces;
using ASC.Common;
using ASC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.DAL.Repository.Classes
{
    public class ServiceBookingRepository : IServiceBookingRepository
    {
        private readonly Database.SampleDBEntities _DbContext;

        public ServiceBookingRepository()
        {
            _DbContext = new Database.SampleDBEntities();
        }
        public string AddBooking(ServiceBookingModel serviceBookingModel)
        {
            try
            {
                if (serviceBookingModel.ServiceBooking != null)
                {
                    var res = _DbContext.ServiceBookings.Where(x => x.VehicleId == serviceBookingModel.ServiceBooking.VehicleId
                    && x.BookingDate == serviceBookingModel.ServiceBooking.BookingDate).FirstOrDefault();
                    if (res != null)
                    {
                        return "already";
                    }
                    Database.ServiceBooking entity = new Database.ServiceBooking();
                    entity = AutoMapperConfig.ServiceBookingMapper.Map<Database.ServiceBooking>(serviceBookingModel.ServiceBooking);
                    entity.Status = "Panding";
                    _DbContext.ServiceBookings.Add(entity);
                    _DbContext.SaveChanges();
                    int id = entity.Id;
                    foreach (var I in serviceBookingModel.servicesIds)
                    {
                        Database.Service service = _DbContext.Services.Find(I);
                        Database.SelectedService selectedService = new Database.SelectedService();
                        selectedService.ServiceBookingId = entity.Id;
                        selectedService.ServiceId = service.Id;
                        _DbContext.SelectedServices.Add(selectedService);
                        _DbContext.SaveChanges();
                    }
                    
                    return "created";
                }
                return "null";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
