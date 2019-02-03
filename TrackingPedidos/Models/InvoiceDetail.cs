using System.Collections.Generic;

namespace TrackingPedidos.Models
{
    public class InvoiceDetail
    {
        public int invoice_detail_id { get; set; }
        public int quantity { get; set; }
        public string sub_total { get; set; }
        public int product_id { get; set; }
        public int discounts_id { get; set; }
        public int invoice_header_id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public List<Product> product { get; set; }
    }
}