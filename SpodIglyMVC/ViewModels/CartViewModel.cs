using SpodIglyMVC.Models;
using System.Collections.Generic;

namespace SpodIglyMVC.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal TotalPrice{ get; set; }
    }
}