using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Xml.Linq;
namespace Hosting
{
     public class ItemsController : ApiController
     {
        private static List<Item> items = new List<Item>  
  
        {  
            new Item { Id = 1, Name = "Apple", WerehouseStatus = "InStock", Price =1 },  
            new Item{ Id = 2, Name = "Tomato", WerehouseStatus = "InStock", Price =4 },  
            new Item{ Id = 3, Name = "T-Shirt", WerehouseStatus = "OutOfStock", Price =33 }  
        };

        public IEnumerable<Item> GetAllItems()
        {
            return items;
        }

        public Item GetItemById(int id)
        {
            var item = items.FirstOrDefault((i) => i.Id == id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }

        public Item GetItemByName(string name)
        {
            var item = items.FirstOrDefault(i => i.Name.ToLower() == name.ToLower());

            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }
        [HttpPost]
        public void Post([FromBody] Item value)
        {
            if (items.FirstOrDefault(i => i.Name.ToLower() == value.Name.ToLower()) != null)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }    
            value.Id = (items.OrderByDescending(i => i.Id).First().Id + 1);
            items.Add(value);
        }
        [HttpDelete]
        public void Delete(string name)
        {
            items.Remove(items.FirstOrDefault(i => i.Name==name));
        }
        [HttpDelete]
        public void Delete()
        {
            items.Clear();
        }
        [HttpPut]
        public void Put([FromBody] Item value)
        {
            var index =  items.FindIndex(i => i.Id == value.Id);
            if(index !=-1)
            {
                items[index] = value;
            }
        }

    }
}
