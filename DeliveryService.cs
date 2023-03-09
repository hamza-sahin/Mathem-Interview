using System;
using System.Collections.Generic;

namespace MathemDelivery
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public List<int> DeliveryDays { get; set; }
        public string ProductType { get; set; }
        public int DaysInAdvance { get; set; }
    }

    public class DeliveryDate
    {
        public int PostalCode { get; set; }
        public DateTime DeliveryDateValue { get; set; }
        public bool IsGreenDelivery { get; set; }
    }

     public class DeliveryService
    {
        private readonly int DaysInWeek = 7;
        private readonly List<DayOfWeek> GreenDays = new List<DayOfWeek> { DayOfWeek.Wednesday };

        private bool IsGreenDelivery(DateTime deliveryDate) =>
            GreenDays.Contains(deliveryDate.DayOfWeek);

        public List<DeliveryDate> CalculateDeliveryDates(int postalCode, List<Product> products)
        {
            var deliveryDates = new List<DeliveryDate>();
            var today = DateTime.Today;

            for (var i = 0; i < 14; i++)
            {
                var deliveryDate = today.AddDays(i);

                bool isValidDeliveryDate = true;
                foreach (var product in products)
                {
                    if (!product.DeliveryDays.Contains((int)deliveryDate.DayOfWeek))
                    {
                        isValidDeliveryDate = false;
                        break;
                    }

                    if ((deliveryDate - today).Days < product.DaysInAdvance)
                    {
                        isValidDeliveryDate = false;
                        break;
                    }

                    if (product.ProductType == "external" && (deliveryDate - today).Days < 5)
                    {
                        isValidDeliveryDate = false;
                        break;
                    }

                    if (product.ProductType == "temporary" && (int)deliveryDate.DayOfWeek >= (int)today.DayOfWeek + DaysInWeek)
                    {
                        isValidDeliveryDate = false;
                        break;
                    }
                }

                if (isValidDeliveryDate)
                {
                    var deliveryDay = new DeliveryDate
                    {
                        PostalCode = postalCode,
                        DeliveryDateValue = deliveryDate,
                        IsGreenDelivery = IsGreenDelivery(deliveryDate)
                    };

                    deliveryDates.Add(deliveryDay);
                }
            }

            deliveryDates.Sort((d1, d2) =>
            {
                if (d1.IsGreenDelivery && !d2.IsGreenDelivery)
                {
                    return -1;
                }

                if (!d1.IsGreenDelivery && d2.IsGreenDelivery)
                {
                    return 1;
                }

                var compareDate = today.AddDays(3);

                if (d1.DeliveryDateValue < compareDate && d2.DeliveryDateValue >= compareDate)
                {
                    return -1;
                }

                if (d1.DeliveryDateValue >= compareDate && d2.DeliveryDateValue < compareDate)
                {
                    return 1;
                }

                return DateTime.Compare(d1.DeliveryDateValue, d2.DeliveryDateValue);
            });

            return deliveryDates;
        }
    }
}