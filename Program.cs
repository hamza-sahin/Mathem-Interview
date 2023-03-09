using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MathemDelivery
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new DeliveryService();

            var products = new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    Name = "Product 1",
                    DeliveryDays = new List<int> { 1, 2, 3, 4, 5 },
                    ProductType = "normal",
                    DaysInAdvance = 0
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Product 2",
                    DeliveryDays = new List<int> { 2, 3, 4, 5, 6 },
                    ProductType = "external",
                    DaysInAdvance = 5
                },
                new Product
                {
                    ProductId = 3,
                    Name = "Product 3",
                    DeliveryDays = new List<int> { 4, 5, 6 },
                    ProductType = "temporary",
                    DaysInAdvance = 0
                }
            };

            int postalCode = 12345;

            var deliveryDates = service.CalculateDeliveryDates(postalCode, products);

            Console.WriteLine(JsonConvert.SerializeObject(deliveryDates, Formatting.Indented));
        }
    }
}
