using ASC.Common;
using ASC.Entities;
using System.Collections.Generic;

namespace ACS.DAL.Repository.Interfaces
{
    public interface IServiceBookingRepository
    {
        string AddBooking(ServiceBookingModel serviceBookingModel);
        IEnumerable<ServiceBooking> GetBookings();
        ServiceBookingDetailModel GetDetail(int id);
    }
}
