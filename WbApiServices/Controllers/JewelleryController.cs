using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WbApiServices.Models.WebApiServices.Models;

namespace WbApiServices.Controllers
{
    //http://localhost:1618/api/Jewellery?xml=true
    //http://localhost:1618/api/Jewellery?xml=true&$top=2
    //http://localhost:1618/api/Jewellery?xml=true&$filter=ID%20eq%2018
    public class JewelleryController : ApiController
    {
        // GET api/values
        [Queryable]
        public IQueryable<Jewellery> GetAllJewellery()
        {
            List<Jewellery> model = new List<Jewellery>();

            //Cache
            var context = HttpContext.Current;
            if (context != null)
            {
                if (context.Cache["Product"] == null)
                {
                    using (JewelleryStoreDB dbContext = new JewelleryStoreDB())
                    {
                        context.Cache.Add("Product", dbContext.tblProduct.OrderBy(pr => pr.Name).ToList(), null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 1, 0), System.Web.Caching.CacheItemPriority.Default, null);
                    }
                }
            }
            // Bitti Cache

            List<tblProduct> cac = (List<tblProduct>)context.Cache["Product"];

            foreach (var item in cac)
            {
                Jewellery mod = new Jewellery();
                mod.ID = item.ID;
                mod.Name = item.Name;
                mod.Price = item.Price;
                mod.Description = item.Description;
                mod.ImageUrl = item.ImageUrl;
                model.Add(mod);
            }
            return model.AsQueryable();
        }

        // GET api/values/5
        //http://localhost:1618/api/Jewellery/18?xml=true        
        public Jewellery GetJewellery(int ID)
        {
            var context = HttpContext.Current;
            if (context != null)
            {
                if (context.Cache["Product"] == null)
                {
                    using (JewelleryStoreDB dbContext = new JewelleryStoreDB())
                    {
                        context.Cache.Add("Product", dbContext.tblProduct.OrderBy(pr => pr.Name).ToList(), null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 1, 0), System.Web.Caching.CacheItemPriority.Default, null);
                    }
                }
            }
            // Bitti Cache
            List<tblProduct> cac = (List<tblProduct>)context.Cache["Product"];

            return cac.Where(pr => pr.ID == ID).Select(re => new  Jewellery{ ID = re.ID, Name = re.Name, Price = re.Price,Description=re.Description, ImageUrl=re.ImageUrl }).FirstOrDefault();
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
