using ASC.Common;
using ASC.Entities;
using System.Collections.Generic;

namespace ASC.BAL.Repository.Interfaces
{
    public interface IServiceBookingManager
    {
        string AddBooking(ServiceBookingModel serviceBookingModel);
        IEnumerable<ServiceBooking> GetBookings();
        ServiceBookingDetailModel GetDetail(int id);
    }
}
