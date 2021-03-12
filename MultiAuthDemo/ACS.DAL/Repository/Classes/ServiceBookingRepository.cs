﻿using ACS.DAL.Repository.Interfaces;
using ASC.Common;
using ASC.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;

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


                    string FilePath = "D:/Projects/BookinMail.html";
                    StreamReader str = new StreamReader(FilePath);
                    string MailText = str.ReadToEnd();
                    str.Close();

                    MailMessage mail = new MailMessage();
                    mail.To.Add("arjun.chandarana.ac@gmail.com");
                    mail.From = new MailAddress("automobile.onthego@gmail.com");
                    mail.Subject = "Appontment Booked";
                    string Body = MailText;
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("automobile.onthego@gmail.com", "Password@123");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);


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
