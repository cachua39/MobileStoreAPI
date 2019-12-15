using System;
using System.Collections.Generic;

namespace MobileStoreAPI.Models
{
    public partial class TblCoupon
    {
        public TblCoupon()
        {
            TblTransaction = new HashSet<TblTransaction>();
        }

        public string Code { get; set; }
        public double Value { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

        public virtual ICollection<TblTransaction> TblTransaction { get; set; }
    }
}
