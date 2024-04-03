using Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ItemService
    {
        private HttpClient client;
        public ItemService()
        {
           client = new HttpClient();
           client.BaseAddress = new Uri("http://localhost:8080");
        }
        public void ListAllItems()
        {
            try
            {
                HttpResponseMessage resp = client.GetAsync("api/items").Result;
                var message = resp.EnsureSuccessStatusCode();

                if (message.IsSuccessStatusCode)
                {

                    var items = resp.Content.ReadAsAsync<IEnumerable<Hosting.Item>>().Result;
                    if (items == null || items.Count() == 0)
                    {
                        Console.WriteLine("There is no items in stock\r\n");
                    }
                    else
                    {
                        Console.WriteLine("Avalible items information:");
                        foreach (var item in items)
                        {
                            Console.WriteLine($"ID: {item.Id}, Name:{item.Name}, Price: {item.Price.ToString(".00")}, Werehouse status: {item.WerehouseStatus}");
                        }
                        Console.WriteLine(" ");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Items not found\r\n");
            }
        }

        public Item GetItem(int id)
        {
            try
            {
                HttpResponseMessage resp = client.GetAsync($"api/items/{id}").Result;
                var message = resp.EnsureSuccessStatusCode();

                if (message.IsSuccessStatusCode)
                {
                    var item = resp.Content.ReadAsAsync<Hosting.Item>().Result;
                    if (item == null)
                    {
                        Console.WriteLine("Item not found\r\n");
                    }
                    else
                    {
                        Console.WriteLine("Selected item information:");
                        Console.WriteLine($"ID: {item.Id}, Name:{item.Name}, Price: {item.Price.ToString(".00")}, Werehouse status: {item.WerehouseStatus}\r\n");
                        return item;
                    }
                }
                else
                {
                    Console.WriteLine("Item not found\r\n");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Item not found \r\n");
            }
            return null;
        }

        public Item GetItem(string name)
        {
            try
            {
                Console.WriteLine($"Searching for '{name}'");

                string query = $"api/items?name={name.ToLower()}";

                HttpResponseMessage resp = client.GetAsync(query).Result;
                var message = resp.EnsureSuccessStatusCode();

                if (message.IsSuccessStatusCode)
                {
                    var item = resp.Content.ReadAsAsync<Hosting.Item>().Result;

                    if (item == null)
                    {
                        Console.WriteLine("Item not found\r\n");
                    }
                    else
                    {
                        Console.WriteLine("Found item information:");
                        Console.WriteLine($"ID: {item.Id}, Name:{item.Name}, Price: {item.Price.ToString(".00")}, Werehouse status: {item.WerehouseStatus}\r\n");
                        return item;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Item not found \r\n");
            }
            return null;
        }
        public void PostItem(Item item)
        {

            try
            {
                Console.WriteLine($"Adding {item.Name}");
                string json = JsonConvert.SerializeObject(item);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage resp = client.PostAsync(string.Format("api/items"), httpContent).Result;
                if(resp.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    Console.WriteLine($"Error! Product aleady exists\r\n");
                    return;
                }
                var message = resp.EnsureSuccessStatusCode();
                if (message.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Product added successfully\r\n");
                }                
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Adding failed. {ex.Message}\r\n");
            }
        }
        public void PutItem(Item item)
        {

            try
            {
                Console.WriteLine($"Changing item {item.Name}");
                string json = JsonConvert.SerializeObject(item);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage resp = client.PutAsync(string.Format($"api/items"), httpContent).Result;
                var message = resp.EnsureSuccessStatusCode();
                if (message.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Product changed successfully\r\n");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Change failed. {ex.Message}\r\n");
            }
        }

        public void DeleteItem(string name)
        {
            Console.WriteLine($"Deleting '{name}'");

            string query = $"api/items?name={name}";

            HttpResponseMessage resp = client.DeleteAsync(query).Result;
            var message = resp.EnsureSuccessStatusCode();

            if (message.IsSuccessStatusCode)
            {
                Console.WriteLine("Delete successfull\r\n");
            }

        }
        public void DeleteAll()
        {
            Console.WriteLine("Deleting all items");

            string query = "api/items";

            HttpResponseMessage resp = client.DeleteAsync(query).Result;
            var message = resp.EnsureSuccessStatusCode();

            if (message.IsSuccessStatusCode)
            {
                Console.WriteLine("All items have been removed from database\r\n");
            }

        }

    }
}
