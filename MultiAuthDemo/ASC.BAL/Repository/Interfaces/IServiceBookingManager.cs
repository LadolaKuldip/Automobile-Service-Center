using ASC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASC.BAL.Repository.Interfaces
{
    public interface IServiceBookingManager
    {
        string AddBooking(ServiceBookingModel serviceBookingModel);
    }
}
