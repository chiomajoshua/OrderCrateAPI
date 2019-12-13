using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderCrateAPI.Entities
{
    public partial class Transaction
    {
        public int ID { get; set; }
        public double Amount { get; set; }
        [Required]
        [StringLength(7)]
        public string DebitCredit { get; set; }
        [Required]
        public string Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }
        public int BusinessID { get; set; }

        [ForeignKey("BusinessID")]
        [InverseProperty("Transaction")]
        public virtual Business Business { get; set; }
    }
}