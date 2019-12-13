using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderCrateAPI.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Order = new HashSet<Order>();
        }

        public int ID { get; set; }
        [Required]
        [StringLength(80)]
        public string Name { get; set; }
        [Required]
        [StringLength(10)]
        public string Title { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [StringLength(15)]
        public string Phone { get; set; }
        public string Address { get; set; }
        public int BusinessID { get; set; }
        [Required]
        [StringLength(15)]
        public string Platform { get; set; }

        [ForeignKey("BusinessID")]
        [InverseProperty("Customer")]
        public virtual Business Business { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<Order> Order { get; set; }
    }
}