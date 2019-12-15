using System;
using System.Collections.Generic;

namespace MobileStoreAPI.Models
{
    public partial class TblMobile
    {
        public TblMobile()
        {
            TblOption = new HashSet<TblOption>();
            TblTransactionDetail = new HashSet<TblTransactionDetail>();
        }

        public string MobileId { get; set; }
        public string MobileName { get; set; }
        public double UnitPrice { get; set; }
        public string Description { get; set; }
        public int BrandId { get; set; }
        public string Status { get; set; }
        public string Photo { get; set; }
        public string ScreenResolution { get; set; }
        public string ScreenSize { get; set; }
        public string OperatingSystem { get; set; }
        public string RearCamera { get; set; }
        public string FrontCamera { get; set; }
        public string Cpu { get; set; }
        public string BateryCapacity { get; set; }
        public string Sim { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual TblBrand Brand { get; set; }
        public virtual ICollection<TblOption> TblOption { get; set; }
        public virtual ICollection<TblTransactionDetail> TblTransactionDetail { get; set; }
    }
}
