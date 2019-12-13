using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderCrateAPI.Entities
{
    public partial class Order
    {
        public int ID { get; set; }
        [Required]
        [StringLength(30)]
        public string InvoiceNumber { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [StringLength(30)]
        public string OrderPlatform { get; set; }
        [Required]
        [StringLength(15)]
        public string Status { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }
        public double? Discount { get; set; }
        public int? CustomerID { get; set; }
        public int BusinessID { get; set; }

        [ForeignKey("BusinessID")]
        [InverseProperty("Order")]
        public virtual Business Business { get; set; }
        [ForeignKey("CustomerID")]
        [InverseProperty("Order")]
        public virtual Customer Customer { get; set; }
        [InverseProperty("Order")]
        public virtual Delivery Delivery { get; set; }
        [InverseProperty("Order")]
        public virtual Payment Payment { get; set; }
    }
}