using System;
using System.Collections.Generic;

namespace MobileStoreAPI.Models
{
    public partial class TblTransactionDetail
    {
        public int TransactionDetailId { get; set; }
        public int TransactionId { get; set; }
        public string Ram { get; set; }
        public string Color { get; set; }
        public string Memory { get; set; }
        public string MobileId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double SubTotal { get; set; }

        public virtual TblMobile Mobile { get; set; }
        public virtual TblTransaction Transaction { get; set; }
    }
}
