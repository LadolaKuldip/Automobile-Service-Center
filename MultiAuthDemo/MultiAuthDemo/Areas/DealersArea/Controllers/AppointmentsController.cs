﻿using ASC.Common;
using ASC.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MultiAuthDemo.Areas.DealersArea.Controllers
{
    public class AppointmentsController : Controller
    {
        // GET: AdminsArea/Appointment
        public ActionResult Index()
        {
            IEnumerable<ServiceBooking> serviceBookings;

            using (var client = new HttpClient())
            {
                string userId = User.Identity.GetUserId();

                client.BaseAddress = new Uri("https://localhost:44318/ServiceBooking/");
                //HTTP GET
                var responseTask = client.GetAsync("GetDealerBookings/"+ userId);
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

        // GET: AdminsArea/Appointment/Create
        public ActionResult Create(string searchData)
        {
            ServiceBooking serviceBooking = new ServiceBooking();
            CustomerVehicles customerVehicles = null;
            IEnumerable<Dealer> dealers;
            IEnumerable<Service> services;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("https://localhost:44318/");
                //HTTP GET
                var responseTask = client.GetAsync("Dealer/Get");
                var responseTask2 = client.GetAsync("Service/Get");
                responseTask.Wait();
                responseTask2.Wait();
                var result = responseTask.Result;
                var result2 = responseTask2.Result;
                if (result.IsSuccessStatusCode && result2.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IEnumerable<Dealer>>();
                    readTask.Wait();
                    dealers = readTask.Result;

                    var readTask2 = result2.Content.ReadAsAsync<IEnumerable<Service>>();
                    readTask.Wait();
                    services = readTask2.Result;
                }
                else
                {
                    dealers = Enumerable.Empty<Dealer>();
                    services = Enumerable.Empty<Service>();
                    ModelState.AddModelError(string.Empty, "Server error occured while retriving data");
                }
            }
            if (searchData != null)
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri("https://localhost:44318/Customer/");
                    //HTTP GET
                    var responseTask = client.PostAsJsonAsync("GetName/", searchData);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<CustomerVehicles>();
                        readTask.Wait();
                        customerVehicles = readTask.Result;
                    }
                    else
                    {
                        customerVehicles = null;
                        ModelState.AddModelError(string.Empty, "User is not found with [" + searchData + "] .Please Try Again !!");
                    }
                }
            }
            AppointmentViewModel appointment = new AppointmentViewModel
            {
                ServiceBooking = serviceBooking,
                CustomerVehicles = customerVehicles,
                Dealers = dealers,
                Services = services
            };
            return View(appointment);
        }


        [HttpPost]
        public ActionResult Create(ServiceBooking ServiceBooking, int[] servicesIds)
        {
            try
            {
                ServiceBookingModel serviceBookingModel = new ServiceBookingModel
                {
                    ServiceBooking = ServiceBooking,
                    servicesIds = servicesIds
                };
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44318/ServiceBooking/");

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<ServiceBookingModel>("Create", serviceBookingModel);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        TempData["Type"] = 0;
                        TempData["Message"] = "Model Added successfully";
                        return RedirectToAction("Index");
                    }
                }
                TempData["Type"] = 2;
                TempData["Message"] = "Error Occured While Creating";
                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }

        }

        // GET: AdminsArea/Appointment/Details/5
        public ActionResult Details(int id)
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
    }
}