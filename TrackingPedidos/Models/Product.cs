namespace TrackingPedidos.Models
{
    public class Product
    {
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string product_description { get; set; }
        public string product_characteristic { get; set; }
        public string price { get; set; }
        public int stock { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
}