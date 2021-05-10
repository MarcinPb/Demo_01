using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Model
{
    [Serializable]
    public class Order
    {
        public int OrderId { get; set; }

        //public string CustomerName { get; set; }

        public int CustomerId { get; set; }

        public string OrderDesc { get; set; }

        public decimal Price { get; set; }

        public DateTime OrderDate { get; set; }

        public bool IsPaid { get; set; }


        public Order()
        {
            OrderDate = DateTime.Today;
        }

        public object Clone()
        {
            return new Order()
            {
                OrderId = OrderId,
                CustomerId = CustomerId,
                OrderDesc = OrderDesc,
                Price = Price,
                OrderDate = OrderDate,
                IsPaid = IsPaid,
            };
        }

        public bool Update(Order order)
        {
            CustomerId = order.CustomerId;
            OrderDesc = order.OrderDesc;
            Price = order.Price;
            OrderDate = order.OrderDate;
            IsPaid = order.IsPaid;

            return true;
        }


    }
}
