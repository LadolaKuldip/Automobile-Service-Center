using ASC.Common;
using ASC.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;

namespace MultiAuthDemo.Areas.UsersArea.Controllers
{
    //[Authorize(Roles = "Customer")]
    public class UserController : Controller
    {
        // GET: UsersArea/User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail()
        {
            string userId = User.Identity.GetUserId();

            Customer customer = new Customer();
            if (userId != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44318/");
                    //HTTP GET

                    var responseTask = client.GetAsync("Customer/GetUserId/" + userId);

                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Customer>();
                        readTask.Wait();
                        customer = readTask.Result;

                    }
                    else
                    {
                        customer = null;
                        ModelState.AddModelError(string.Empty, "Server error occured while retriving data");
                    }
                }
            }

            return View(customer);
        }

        public ActionResult Vehicles()
        {
            string userId = User.Identity.GetUserId();

            IEnumerable<Vehicle> vehicles;

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("https://localhost:44318/Vehicle/");
                //HTTP GET
                var responseTask = client.GetAsync("GetVehiclesOfUser/" + userId);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IEnumerable<Vehicle>>();
                    readTask.Wait();
                    vehicles = readTask.Result;
                }
                else
                {
                    vehicles = Enumerable.Empty<Vehicle>();
                    ModelState.AddModelError(string.Empty, "Server error occured while retriving data");
                }
            }

            return View(vehicles);
        }

        public ActionResult Appointment()
        {
            IEnumerable<ServiceBooking> serviceBookings;
            string userId = User.Identity.GetUserId();
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("https://localhost:44318/ServiceBooking/");
                //HTTP GET
                var responseTask = client.GetAsync("GetUserBookings/" + userId);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IEnumerable<ServiceBooking>>();
                    readTask.Wait();
                    serviceBookings = readTask.Result;
                }
                else
                {
                    serviceBookings = Enumerable.Empty<ServiceBooking>();
                    ModelState.AddModelError(string.Empty, "Server error occured while retriving data");
                }
            }

            return View(serviceBookings);
        }


        // GET: AdminsArea/Appointment/Details/5
        public ActionResult AppointmentDetails(int id)
        {
            ServiceBookingDetailModel model;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("https://localhost:44318/ServiceBooking/");
                //HTTP GET
                var responseTask = client.GetAsync("GetDetail/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ServiceBookingDetailModel>();
                    readTask.Wait();
                    model = readTask.Result;
                }
                else
                {
                    model = new ServiceBookingDetailModel();
                    ModelState.AddModelError(string.Empty, "Server error occured while retriving data");
                }
            }
            return PartialView(model);
        }
        public ActionResult EditAppointment(int id)
        {
            return RedirectToAction("Index");
        }

        public ActionResult DeleteAppointment(int id)
        {
            return RedirectToAction("Index");
        }
    }
}