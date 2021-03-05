using System;

namespace ASC.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }
        public string Address { get; set; }
        public Nullable<int> DealerId { get; set; }
        public bool IsActive { get; set; }

        public virtual Dealer Dealer { get; set; } 
    }
}
