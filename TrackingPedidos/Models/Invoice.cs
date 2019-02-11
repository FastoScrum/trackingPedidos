using System.Collections.Generic;

namespace TrackingPedidos.Models
{
    public class Invoice
    {
        public int invoice_header_id { get; set; }
        public string invoice_number { get; set; }
        public int iva_id { get; set; }
        public string total { get; set; }
        public string payment_account { get; set; }
        public object shipping_cost { get; set; }
        public object shipping_kind { get; set; }
        public object gift { get; set; }
        public int payment_kind_id { get; set; }
        public int? discounts_id { get; set; }
        public int client_id { get; set; }
        public int cashier_id { get; set; }
        public string special_contributor { get; set; }
        public string accounting { get; set; }
        public string number_authorization { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string sub_total { get; set; }
        public int module { get; set; }
        public List<InvoiceDetail> invoice_details { get; set; }
        public PaymentKind payment_kind { get; set; }
        public Discount discount { get; set; }
        public Iva iva { get; set; }
        public UserClient user_client { get; set; }
    }
}