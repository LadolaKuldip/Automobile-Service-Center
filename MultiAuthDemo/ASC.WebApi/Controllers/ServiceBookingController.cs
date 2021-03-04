using ASC.BAL.Repository.Interfaces;
using ASC.Common;
using ASC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ASC.WebApi.Controllers
{
    [RoutePrefix("ServiceBooking")]
    public class ServiceBookingController : ApiController
    {
        private readonly IServiceBookingManager _serviceBookingManager;
        public ServiceBookingController(IServiceBookingManager serviceBookingManager)
        {
            _serviceBookingManager = serviceBookingManager;
        }
        [HttpPost]
        [Route("Create")]
        public IHttpActionResult Create(ServiceBookingModel serviceBookingModel)
        {
            string response = _serviceBookingManager.AddBooking(serviceBookingModel);
            if (response == "already")
            {
                return Conflict();
            }
            else if (response != "created")
            {
                return InternalServerError();
            }
            return Ok();
        }
    }
}
