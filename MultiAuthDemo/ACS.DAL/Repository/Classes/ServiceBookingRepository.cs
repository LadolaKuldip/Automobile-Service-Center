using ACS.DAL.Repository.Interfaces;
using ASC.Common;
using ASC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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
                    entity.Status = "Pending";
                    _DbContext.ServiceBookings.Add(entity);
                    _DbContext.SaveChanges();
                    int id = entity.Id;
                    double totalAmmount = 0;
                    foreach (var I in serviceBookingModel.servicesIds)
                    {
                        Database.Service service = _DbContext.Services.Find(I);
                        Database.SelectedService selectedService = new Database.SelectedService();
                        selectedService.ServiceBookingId = entity.Id;
                        selectedService.ServiceId = service.Id;
                        _DbContext.SelectedServices.Add(selectedService);
                        _DbContext.SaveChanges();

                        totalAmmount += service.Amount;
                    }

                    entity.TotalAmmount = totalAmmount;
                    _DbContext.SaveChanges();
                    return "created";
                }
                return "null";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public IEnumerable<ServiceBooking> GetBookings()
        {
            List<ServiceBooking> serviceBookings = new List<ServiceBooking>();
            IEnumerable<Database.ServiceBooking> entities = _DbContext.ServiceBookings.Include("Dealer").Include("Vehicle").ToList();

            if (entities != null)
            {
                foreach (var item in entities)
                {
                    ServiceBooking serviceBooking = new ServiceBooking();
                    serviceBooking = AutoMapperConfig.ServiceBookingMapper.Map<ServiceBooking>(item);
                    serviceBookings.Add(serviceBooking);
                }
            }
            return serviceBookings;
        }

        public IEnumerable<ServiceBooking> GetDealerBookings(string userId)
        {
            List<ServiceBooking> serviceBookings = new List<ServiceBooking>();
            IEnumerable<Database.ServiceBooking> entities = _DbContext.ServiceBookings.Include("Dealer").Include("Vehicle").Where(x => x.Dealer.UserId == userId).ToList();

            if (entities != null)
            {
                foreach (var item in entities)
                {
                    ServiceBooking serviceBooking = new ServiceBooking();
                    serviceBooking = AutoMapperConfig.ServiceBookingMapper.Map<ServiceBooking>(item);
                    serviceBookings.Add(serviceBooking);
                }
            }
            return serviceBookings;
        }

        public ServiceBookingDetailModel GetDetail(int id)
        {
            ServiceBooking serviceBooking;
            List<Service> services = new List<Service>();
            Database.ServiceBooking entity = _DbContext.ServiceBookings.Include("Dealer").Include("Vehicle").Where(x => x.Id == id).FirstOrDefault();
            if (entity != null)
            {
                serviceBooking = AutoMapperConfig.ServiceBookingMapper.Map<ServiceBooking>(entity);

                IEnumerable<Database.SelectedService> entities = _DbContext.SelectedServices.Where(x => x.ServiceBookingId == entity.Id).ToList();
                foreach (var item in entities)
                {
                    Service service = AutoMapperConfig.ServiceMapper.Map<Service>(item.Service);
                    services.Add(service);
                }
            }
            else
            {
                serviceBooking = null;
                services = null;
            }
            ServiceBookingDetailModel model = new ServiceBookingDetailModel
            {
                ServiceBooking = serviceBooking,
                Services = services
            };
            return model;
        }

        public IEnumerable<ServiceBooking> GetUserBookings(string userId)
        {
            List<ServiceBooking> serviceBookings = new List<ServiceBooking>();
            IEnumerable<Database.ServiceBooking> entities = _DbContext.ServiceBookings.Include("Dealer").Include("Vehicle").Where(x => x.Vehicle.Customer.UserId == userId).ToList();

            if (entities != null)
            {
                foreach (var item in entities)
                {
                    ServiceBooking serviceBooking = new ServiceBooking();
                    serviceBooking = AutoMapperConfig.ServiceBookingMapper.Map<ServiceBooking>(item);
                    serviceBookings.Add(serviceBooking);
                }
            }
            return serviceBookings;
        }
    }
}
