﻿

namespace SpodIglyMVC.ViewModels
{
    public class CartRemoveViewModel
    {
        public decimal CartTotal { get; set; }
        public int CartItemsCount { get; set; }
        public int RemovedItemCount { get; set; }
        public int RemovedItemId { get; set; }
    }
}