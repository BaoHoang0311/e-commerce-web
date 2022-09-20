using e_commerce_web.Data.ViewModel;
using e_commerce_web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Data
{
    public class ProductServices
    {
        private readonly dbMarketsContext _context;
        public ProductServices(dbMarketsContext context)
        {
            _context = context;
        }
        public IQueryable<Product> XL_trang_san_pham(SearchVM searchVM)
        
        {
            IQueryable<Product> LsProducts = _context.Products
                                                    .AsNoTracking()
                                                    .Include(x => x.Cat)
                                                    .Where(p => p.Active == true && p.UnitsInStock > 0)
                                                    .OrderByDescending(x => x.DateCreated);

            if (searchVM.keySearch != null)
            {
                LsProducts = LsProducts.Where(p => p.ProductName.Contains(searchVM.keySearch));
            }
            if (searchVM.Cat != null)
            {
                LsProducts = LsProducts.Where(p => p.Cat.CatId == int.Parse(searchVM.Cat));
            }
            if( searchVM.A != null && searchVM.B  != null)
            {
                LsProducts = LsProducts.Where(p => p.Price.Value >= searchVM.A.Value*1000 && p.Price.Value <= searchVM.B.Value*1000);
            }
            if(searchVM.sortOrder != null)
            {
                LsProducts = LsProducts.OrderBy(x => x.Price.Value);
            }
            return LsProducts;
        }
    }
}
