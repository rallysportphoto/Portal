/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Areas.Shop.Models
{
    public enum OrderState {
        Draft,
        Placed,
        Paid,
        Ordered,
        Received,
        SentToCustomer,
        Fulfilled,
        Canceled
    }
    public class ShopOrder
    {
        public int Id { get; set; }
        public User Customer { get; set; }
        public OrderState State { get; set; }
        public decimal Total { get; set; }
        public decimal DeliveryTotal { get; set; }
        public string InternalDeliveryNumber { get; set;}
        public string DeliveryNumber { get; set;}
        public string Notes { get; set;}
        public string InternalNotes { get; set;}

        public PaymentProvider PaymentProvider { get; set; }
        public DeliveryProvider DeliveryProvider { get; set; }

        public List<OrderItem> Items { get; set; }
        public List<OrderHistoryLine> History { get; set; }
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal DeliveryPrice { get; set; }
        
    }

    public class OrderHistoryLine
    {
        public int Id { get; set; }
        public OrderState State { get; set; }
        public User ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public string Description { get; set; }
        public string Snapshot { get; set; }
        public string TransactionId { get; set; }
    }
}