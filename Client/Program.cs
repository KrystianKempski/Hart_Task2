using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Hosting;
using Newtonsoft.Json;
namespace Client
{
    class Program
    {
        
        static void Main(string[] args)
        {
            int selectedAction = 0;
            Console.WriteLine("############################################################");
            Console.WriteLine("######################## API CLIENT ########################");
            Console.WriteLine("############################################################");
            for (; ; )
            {

                try
                {
                    ItemService itemService = new ItemService();
                    Console.WriteLine("\r\nWhat You want to do? Select action:");
                    Console.WriteLine("\t1 - Add new item");
                    Console.WriteLine("\t2 - Clear items list");
                    Console.WriteLine("\t3 - Erase item");
                    Console.WriteLine("\t4 - List all items");
                    Console.WriteLine("\t5 - Search items by name");
                    Console.WriteLine("\t6 - Change item price");
                    Console.WriteLine("\t7 - Change item werehouse status");
                    Console.WriteLine("Type a number and press enter");
                    selectedAction = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();
                    string itemName;
                    Item item = null;
                    switch (selectedAction)
                    {
                        default:
                            throw new OverflowException();
                        case 1:
                            item = new Item();
     
                            Console.WriteLine("Type item name:");
                            item.Name = InputString();
                            Console.WriteLine("Type item werehouse status:");
                            item.WerehouseStatus = InputString();
                            Console.WriteLine("Input item price:");
                            item.Price = InputDecimal();
                           
                            itemService.PostItem(item);
                            break;
                        case 2:
                            itemService.DeleteAll();
                            break;
                        case 3:
                            Console.WriteLine("Type name of item to delete:");
                            itemName = InputString();
                            if(!string.IsNullOrEmpty(itemName))
                                itemService.DeleteItem(itemName);
                            break;
                        case 4:
                            itemService.ListAllItems();
                            break;
                        case 5:
                            Console.WriteLine("Type name of item to search:");
                            itemName = InputString();
                            if (!string.IsNullOrEmpty(itemName))
                                itemService.GetItem(itemName);
                            break;
                        case 6:
                            Console.WriteLine("Type name of item to change price:");
                            itemName = InputString();
                            if (!string.IsNullOrEmpty(itemName))
                            {
                                item = itemService.GetItem(itemName);
                                if(item != null)
                                {
                                    Console.WriteLine("Input new item price:");
                                    item.Price = InputDecimal();
                                    if (item.Price == 0)
                                        break;
                                    itemService.PutItem(item);
                                }
                            }

                            break;
                        case 7:
                            Console.WriteLine("Type name of item to change werehouse status:");
                            itemName = InputString();
                            if (!string.IsNullOrEmpty(itemName))
                            {
                                item = itemService.GetItem(itemName);
                                if (item != null)
                                {
                                    Console.WriteLine("Input new item werehouse status:");
                                    item.WerehouseStatus = InputString();
                                    if(item.WerehouseStatus == string.Empty)
                                        break;
                                    itemService.PutItem(item);
                                }
                            }
                            break;
                    }

                }
                catch( ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine("\r\nTyped input is to long");
                }
                catch (OverflowException ex)
                {
                    Console.WriteLine("\r\nWrong number. Type letters from 1 to 7");
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("\r\nWrong format. Type letters from 1 to 7");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.InnerException.Message}"); ;
                }
            }
        }

        static string InputString()
        {
            string inputString = string.Empty;
            for (; ; )
            {
                inputString = Console.ReadLine();
                if (string.IsNullOrEmpty(inputString))
                {
                    inputString = string.Empty;
                    Console.WriteLine("\r\nWrong input. Try again or type EXIT to go to main menu");
                }
                else if (inputString.ToUpper() == "EXIT")
                {
                    inputString = string.Empty;
                    break;
                }
                else
                {
                    break;
                }
            }
            return inputString;
        }
        static decimal InputDecimal()
        {
            decimal input = 0;
            for (; ; )
            {
                try
                {
                    string line = Console.ReadLine();
                    if (line.ToUpper() == "EXIT")
                    {
                        input = 0; 
                        break;
                    }
                    input = Convert.ToDecimal(line);
                    if (input<1)
                    {
                        input = 0;
                        Console.WriteLine("\r\nWrong input. Try again or type EXIT to go to main menu");
                    }
                    else
                    {
                        break;
                    }
                }
                catch (OverflowException ex)
                {
                    Console.WriteLine("\r\nTo big of a number. Try again or type EXIT to go to main menu");
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("\r\nWrong format. Write a number. Try again or type EXIT to go to main menu");
                }
            }
            return input;
        }

    }
}
