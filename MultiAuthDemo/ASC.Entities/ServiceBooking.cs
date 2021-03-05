using System;

namespace ASC.Entities
{
    public class ServiceBooking
    {
        public int Id { get; set; }
        public System.DateTime BookingDate { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public double TotalAmmount { get; set; }
        public string Status { get; set; }
        public string PickupAddress { get; set; }
        public string DropAddress { get; set; }
        public string Feedback { get; set; }
        public int VehicleId { get; set; }
        public int DealerId { get; set; }

        public virtual Dealer Dealer { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}
