using ACS.DAL.Repository.Interfaces;
using ASC.BAL.Repository.Interfaces;
using ASC.Common;
using ASC.Entities;
using System.Collections.Generic;

namespace ASC.BAL.Repository.Classes
{
    public class ServiceBookingManager : IServiceBookingManager
    {
        private readonly IServiceBookingRepository _serviceBookingRepository;

        public ServiceBookingManager(IServiceBookingRepository serviceBookingRepository)
        {
            _serviceBookingRepository = serviceBookingRepository;
        }
        public string AddBooking(ServiceBookingModel serviceBookingModel)
        {
            return _serviceBookingRepository.AddBooking(serviceBookingModel);
        }

        public IEnumerable<ServiceBooking> GetBookings()
        {
            return _serviceBookingRepository.GetBookings();
        }

        public ServiceBookingDetailModel GetDetail(int id)
        {
            return _serviceBookingRepository.GetDetail(id);
        }
    }
}
