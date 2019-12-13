using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderCrateAPI.Entities
{
    public partial class Delivery
    {
        public int ID { get; set; }
        [Required]
        [StringLength(15)]
        public string Status { get; set; }
        [Required]
        [StringLength(50)]
        public string Vendor { get; set; }
        [Required]
        [StringLength(10)]
        public string Type { get; set; }
        public int OrderID { get; set; }
        public int BusinessID { get; set; }

        [ForeignKey("BusinessID")]
        [InverseProperty("Delivery")]
        public virtual Business Business { get; set; }
        [ForeignKey("OrderID")]
        [InverseProperty("Delivery")]
        public virtual Order Order { get; set; }
    }
}