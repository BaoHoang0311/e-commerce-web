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
        public IQueryable<Product> XL_trang_san_pham(string keySearch, int? CatId, string sortOrder, string Cat)
        {
            IQueryable<Product> LsProducts = _context.Products
                                                    .AsNoTracking()
                                                    .Include(x => x.Cat)
                                                    .Where(p => p.Active == true && p.UnitsInStock > 0)
                                                    .OrderByDescending(x => x.DateCreated);

            if (keySearch != null)
            {
                LsProducts = LsProducts.Where(p => p.ProductName.Contains(keySearch));
            }
            if (Cat != null)
            {
                LsProducts = LsProducts.Where(p => p.Cat.CatId == int.Parse(Cat));
            }
            return LsProducts;
        }
    }
}
