using ACS.DAL.Repository.Interfaces;
using ASC.BAL.Repository.Interfaces;
using ASC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
