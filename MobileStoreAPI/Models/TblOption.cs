using System;
using System.Collections.Generic;

namespace MobileStoreAPI.Models
{
    public partial class TblOption
    {
        public int OptionId { get; set; }
        public string MobileId { get; set; }
        public string Ram { get; set; }
        public string Memory { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }
        public double ExtraPrice { get; set; }

        public virtual TblMobile Mobile { get; set; }
    }
}
