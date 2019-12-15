using System;
using System.Collections.Generic;

namespace MobileStoreAPI.Models
{
    public partial class TblTransaction
    {
        public TblTransaction()
        {
            TblTransactionDetail = new HashSet<TblTransactionDetail>();
        }

        public int TransactionId { get; set; }
        public string UserName { get; set; }
        public double Total { get; set; }
        public string CouponCode { get; set; }
        public double CouponValue { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual TblCoupon CouponCodeNavigation { get; set; }
        public virtual AspNetUsers UserNameNavigation { get; set; }
        public virtual ICollection<TblTransactionDetail> TblTransactionDetail { get; set; }
    }
}
