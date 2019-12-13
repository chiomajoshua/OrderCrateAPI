using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderCrateAPI.Entities
{
    public partial class Business
    {
        public Business()
        {
            Customer = new HashSet<Customer>();
            Delivery = new HashSet<Delivery>();
            Order = new HashSet<Order>();
            Payment = new HashSet<Payment>();
            Transaction = new HashSet<Transaction>();
        }

        public int ID { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [Required]
        [StringLength(150)]
        public string Description { get; set; }
        [Required]
        [StringLength(50)]
        public string Industry { get; set; }
        [Required]
        [StringLength(15)]
        public string Phone { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        [InverseProperty("Business")]
        public virtual User User { get; set; }
        [InverseProperty("Business")]
        public virtual ICollection<Customer> Customer { get; set; }
        [InverseProperty("Business")]
        public virtual ICollection<Delivery> Delivery { get; set; }
        [InverseProperty("Business")]
        public virtual ICollection<Order> Order { get; set; }
        [InverseProperty("Business")]
        public virtual ICollection<Payment> Payment { get; set; }
        [InverseProperty("Business")]
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}