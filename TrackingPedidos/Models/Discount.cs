﻿namespace TrackingPedidos.Models
{
    public class Discount
    {
        public int discounts_id { get; set; }
        public int percentage { get; set; }
        public string description { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
}