using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class StripeModel
    {
        //public StripeModel() { }

        //public StripeModel(string name, string amount, string email, int v1, int v2)
        //{
        //    Name = "shabeer";
        //    Amount = "29";
        //    Email = "shabir@gmail.com";
        //    IsActive = Convert.ToByte(v1);
        //    Role = v2;
        //    CreatedDate = DateTime.Now;
        //}

        public int Id { get; set; } = 0;
        public string Name { get; set; } = "name";
        public string Amount { get; set; }= "23";
        public string Email { get; set; } = "abc@gmail.com";
        public byte IsActive { get; set; } = 1;
        public int? Role { get; set; } = 1;
        public DateTime CreatedDate { get; set; }=DateTime.Now;
    }
}