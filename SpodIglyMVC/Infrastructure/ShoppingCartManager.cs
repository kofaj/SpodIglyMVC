using SpodIglyMVC.DAL;
using SpodIglyMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpodIglyMVC.Infrastructure
{
    public class ShoppingCartManager
    {
        private StoreContext db;
        private ISessionManager session;

        public ShoppingCartManager(ISessionManager session, StoreContext db)
        {
            this.session = session;
            this.db = db;
        }

        public void AddToCart(int albumId)
        {
            var cart = this.GetCart();

            var cartItem = cart.Find(f => f.Album.AlbumId == albumId);

            if (cartItem != null)
                cartItem.Quantity++;
            else
            {
                var albumToAdd = db.Albums.Where(w => w.AlbumId == albumId).SingleOrDefault();
                if (albumToAdd != null)
                {
                    var newCartItem = new CartItem()
                    {
                        Album = albumToAdd,
                        Quantity = 1,
                        TotalPrice = albumToAdd.Price
                    };

                    cart.Add(newCartItem);
                }
            }

            session.Set(Const.CartSessionKey, cart);
        }

        public List<CartItem>GetCart()
        {
            List<CartItem> cart;

            if (session.Get<List<CartItem>>(Const.CartSessionKey) == null)
            {
                cart = new List<CartItem>();
            }
            else
            {
                cart = session.Get<List<CartItem>>(Const.CartSessionKey) as List<CartItem>;
            }

            return cart;
        }

        private int RemoveFromCart(int albumId)
        {
            var cart = this.GetCart();

            var cartItem = cart.Find(f => f.Album.AlbumId == albumId);

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    return cartItem.Quantity;
                }
                else
                {
                    cart.Remove(cartItem);
                }
            }

            // Return count of removed item currently inside cart
            return 0;
        }

        public decimal GetCartTotalPrice()
        {
            var cart = this.GetCart();
            decimal totalPrice = cart.Sum(s => s.TotalPrice * s.Quantity);
            return totalPrice;
        }

        public int GetCartItemsCount()
        {
            var cart = this.GetCart();
            return cart.Sum(s => s.Quantity);
        }

        public Order CreateOrder(Order newOrder, string userId)
        {
            var cart = this.GetCart();

            newOrder.DateCreated = DateTime.Now;
            newOrder.UserId = userId;

            this.db.Orders.Add(newOrder);

            if (newOrder.OrderItems == null)
            {
                newOrder.OrderItems = new List<OrderItem>();
            }

            decimal cartTotal = 0;

            foreach (var item in cart)
            {
                var nweOrderItem = new OrderItem()
                {
                    AlbumId = item.Album.AlbumId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Album.Price
                };

                cartTotal += (item.Quantity * item.Album.Price);

                newOrder.OrderItems.Add(nweOrderItem);
            }

            newOrder.TotalPrice = cartTotal;
            this.db.SaveChanges();

            return newOrder; 
        }

        public void EmptyCart()
        {
            session.Set<List<CartItem>>(Const.CartSessionKey, null);
        }
    }
}