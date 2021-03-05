﻿using ASC.BAL.Repository.Interfaces;
using ASC.Common;
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

        [HttpGet]
        [Route("Get")]
        public IHttpActionResult Get()
        {
            var response = _serviceBookingManager.GetBookings();
            if (response == null)
            {
                return InternalServerError();
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("GetDetail/{id}")]
        public IHttpActionResult GetDetail(int id)
        {
            var response = _serviceBookingManager.GetDetail(id);
            if (response == null)
            {
                return InternalServerError();
            }
            return Ok(response);
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
