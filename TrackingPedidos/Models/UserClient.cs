namespace TrackingPedidos.Models
{
    public class UserClient
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lastname { get; set; }
        public string id_card { get; set; }
        public object birthdate { get; set; }
        public Address address { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string status { get; set; }
        public object email_verified_at { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public object admin_belong { get; set; }
    }
}