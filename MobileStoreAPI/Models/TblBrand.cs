using System;
using System.Collections.Generic;

namespace MobileStoreAPI.Models
{
    public partial class TblBrand
    {
        public TblBrand()
        {
            TblMobile = new HashSet<TblMobile>();
        }

        public int BrandId { get; set; }
        public string BrandName { get; set; }

        public virtual ICollection<TblMobile> TblMobile { get; set; }
    }
}
