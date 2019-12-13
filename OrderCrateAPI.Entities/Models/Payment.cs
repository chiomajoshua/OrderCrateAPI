using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderCrateAPI.Entities
{
    public partial class Payment
    {
        public int ID { get; set; }
        public int BusinessID { get; set; }
        public int OrderID { get; set; }
        public double Amount { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Date_Payed { get; set; }

        [ForeignKey("BusinessID")]
        [InverseProperty("Payment")]
        public virtual Business Business { get; set; }
        [ForeignKey("OrderID")]
        [InverseProperty("Payment")]
        public virtual Order Order { get; set; }
    }
}