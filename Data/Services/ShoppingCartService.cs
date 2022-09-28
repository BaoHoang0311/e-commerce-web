using e_commerce_web.Data.ViewModel;
using e_commerce_web.Models;
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
        }
        public ListCartItemVM listcartitemVM { get; set; }

        public decimal TongTien()
        {
            var total = listcartitemVM.ListCart.Select(x => x.Tien).Sum();
            return total;
        }
        public ListCartItemVM Cong_SP(int ProductID, int? ammount, ListCartItemVM GioHang)
        {
            listcartitemVM = new();
            // tìm sp có trong giỏ hàng chưa
            CartItemVM cartitem = GioHang.ListCart.FirstOrDefault(p => p.sanpham.ProductId == ProductID);
            if (cartitem != null) // có rồi -> cập nhập số lượng
            {
                // nếu Addcart có số lượng thì thêm + với với lượng hiện tại
                if (ammount.HasValue)
                {
                    cartitem.amount += ammount.Value;
                }
                // nếu Addcart không có số lượng ( theo dấu (+) )
                else
                {
                    cartitem.amount++;
                }
                cartitem.Tien = cartitem.Total();
            }
            else // chưa có
            {
                cartitem = new();
                Product product = new();
                product = _context.Products.FirstOrDefault(p => p.ProductId == ProductID);

                cartitem.sanpham = product;

                // số lượng
                // nếu Addcart có số lượng thì thêm + với với lượng hiện tại
                if (ammount.HasValue)
                {
                    cartitem.amount = ammount.Value;
                }
                // nếu Addcart không có số lượng ( theo dấu (+) )
                else
                {
                    cartitem.amount = 1;
                }

                GioHang.ListCart.Add(cartitem);
                cartitem.Tien = cartitem.Total();
            }

            listcartitemVM = GioHang;
            GioHang.TongTien = TongTien();
            return GioHang;
        }
        public List<CartItemVM> Tru_SP(int ProductID, int? ammount, List<CartItemVM> GioHang)
        {
            return null;
        }
    }
}
