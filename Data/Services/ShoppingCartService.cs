using e_commerce_web.Data.ViewModel;
using e_commerce_web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace e_commerce_web.Data.Services
{
    public class ShoppingCartService
    {
        private readonly dbMarketsContext _context;
        public ShoppingCartService(dbMarketsContext context)
        {
            _context = context;
            listcartitemVM = new();
        }
        public ListCartItemVM listcartitemVM { get; set; }

        public decimal TongTien()
        {
            var total = listcartitemVM.ListCart.Select(x => x.Tien).Sum();
            return total;
        }
        public ListCartItemVM Cong_SP(int ProductID, int? ammount, int Detail, ListCartItemVM GioHang)
        {
            // tìm sp có trong giỏ hàng chưa
            CartItemVM cartitem = GioHang.ListCart.FirstOrDefault(p => p.sanpham.ProductId == ProductID);

            Product product = new();
            product = _context.Products.FirstOrDefault(p => p.ProductId == ProductID);
            if (cartitem != null) // có rồi -> cập nhập số lượng
            {
                // nếu Addcart có số lượng thì thêm + với với lượng hiện tại
                if (ammount.HasValue && Detail == 0)
                {
                    cartitem.amount = ammount.Value;
                }
                else if( Detail == 1 )
                {
                    cartitem.amount += ammount.Value;
                }
                // nếu Addcart không có số lượng ( theo dấu (+) )
                else
                {
                    cartitem.amount++;
                }

                if (cartitem.amount > product.UnitsInStock) cartitem.amount = product.UnitsInStock.Value;

                cartitem.Tien = cartitem.Total();
            }
            else // chưa có
            {
                cartitem = new();


                cartitem.sanpham = product;


                if (ammount.HasValue)
                {
                    cartitem.amount = ammount.Value;
                }
                else
                {
                    cartitem.amount = 1;
                }

                GioHang.ListCart.Add(cartitem);
                cartitem.Tien = cartitem.Total();
            }
            // vào cái list trên để nó thực hiện hàm tính tiền
            listcartitemVM = GioHang;
            GioHang.TongTien = TongTien();
            return GioHang;
        }
        public ListCartItemVM RemoveCartItem(int ProductID, int? ammount, ListCartItemVM GioHang)
        {
            // tìm sp trong giỏ hàng chưa
            CartItemVM cartitem = GioHang.ListCart.FirstOrDefault(p => p.sanpham.ProductId == ProductID);
            // vào cái list trên để nó thực hiện hàm tính tiền
            GioHang.ListCart.Remove(cartitem);
            listcartitemVM = GioHang;
            GioHang.TongTien = TongTien();
            return GioHang;
        }
    }
}
