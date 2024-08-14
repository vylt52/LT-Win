using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassSupport
{
    public class OrderDetail
    {
        public string Id { get; set; }
        public string InvoiceId { get; set; }
        public string BookName { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public string TotalEntrySlipDetail { get; set; }
    }
}
