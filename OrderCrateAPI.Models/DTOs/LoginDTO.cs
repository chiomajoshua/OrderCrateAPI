using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCrateAPI.Models.DTOs
{
    public class LoginDto
    {
        public string Username { get; set; }
        public int UserId { get; set; }
        public bool Status { get; set; }
        public UserDto User { get; set; }
        public BusinessDto Business { get; set; }
    }

    public class UserDto
    {
        public int ID { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Gender { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Email { get; set; }
        public DateTime? Date_Joined { get; set; }
    }

    public class UserBusinessDto
    {
        public int ID { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Gender { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Email { get; set; }
        public DateTime? Date_Joined { get; set; }
        public BusinessOrderTransactionDTO Business {get; set;}
    }

    public class FullProfileDTO
    {
        public string Username { get; set; }
        public int UserID { get; set; }
        public bool Status { get; set; }
        public UserBusinessDto User { get; set; }
    }
    public class BusinessOrderTransactionDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Industry { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public IEnumerable<OrderDTO> Order { get; set; }
        public IEnumerable<TransactionDTO> Transaction { get; set; }
    }

    public class BusinessDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Industry { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    public class OrderDTO
    {
        public int ID { get; set; }
        public string InvoiceNumber { get; set; }
        public string Description { get; set; }
        public string OrderPlatform { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        public double? Discount { get; set; }
    }

    public class OrderPaymentDeliveryDTO
    {
        public int ID { get; set; }
        public string InvoiceNumber { get; set; }
        public string Description { get; set; }
        public string OrderPlatform { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        public double? Discount { get; set; }
        public PaymentDTO Payment { get; set; }
        public DeliveryDTO Delivery { get; set; }
    }

    public class PaymentDTO
    {
        public int ID { get; set; }
        public double Amount { get; set; }
        public string Date_Payed { get; set; }
    }
    public class DeliveryDTO
    {
        public int ID { get; set; }
        public string Status { get; set; }
        public string Vendor { get; set; }
        public string Type { get; set; }
    }
    public class TransactionDTO
    {
        public int ID { get; set; }
        public double Amount { get; set; }
        public string DebitCredit { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
    }
}
