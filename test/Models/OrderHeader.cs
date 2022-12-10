﻿using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set;}
        public double OrderTotal { get; set; }
        public string? OrderStatus { get; set; }   
        public string? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime PaymnetDueDate { get; set;}

        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }

        public string PhoneNumber { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
