using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WbApiServices.Models
{
    namespace WebApiServices.Models
    {
        public class Jewellery
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string ImageUrl { get; set; }
            public decimal? Price { get; set; }
            public string Description { get; set; }

            public DateTime? CreatedDate { get; set; }
        }
    }
}